using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TagsCounter.Infrastructure.Additional;
using TagsCounter.Models;
using TagsCounter.Models.Interfaces;
using TagsCounter.Models.Services;
using TagsCounter.ViewModels;

namespace TagsCounter.Infrastructure.Commands
{
    public class ParseAllLinksFromFileCommand : ICommand
    {
        public CancellationTokenSource CancelTokenSource { get; private set; }
        private CancellationToken CancelToken;
        public delegate void Function();
        private readonly MainWindowViewModel ViewModel;
        public event EventHandler CanExecuteChanged;
        public ParseAllLinksFromFileCommand(MainWindowViewModel viewModel) => ViewModel = viewModel;
        public bool CanExecute(object parameter) => true;
        public async void Execute(object parameter)
        {
            if (ViewModel.FilesList is null)
            {
                ViewModel.StatusMessage = "No file selected";
                return;
            }

            ViewModel.StatusMessage = null;

            CancelTokenSource = new CancellationTokenSource();
            CancelToken = CancelTokenSource.Token;

            await Task.Run(() =>
            {
                ViewModel.ParsingAvailable = false;
                try
                {
                    ViewModel.Diagramm.Reset();
                    Action(() => ViewModel.ParsedItemsList = new ObservableCollection<IParsedItemViewModel>());
                    ViewModel.Performance.Start();
                    ViewModel.ProgressBar.FillByStep();

                    ParallelOptions parallelOption = new ParallelOptions
                    {
                        CancellationToken = CancelTokenSource.Token,
                        MaxDegreeOfParallelism = Environment.ProcessorCount
                    };

                    Parallel.ForEach(ViewModel.FilesList, parallelOption, (item) =>
                    {
                        var parsedItem = new ParsedItemViewModel
                        {
                            URL = item
                        };
                        try
                        {
                            parsedItem.ItemsCounts = Parser.PasreItemsCount(item);
                            parsedItem.Status = OPERATION_STATUS.Done;
                            parallelOption.CancellationToken.ThrowIfCancellationRequested();
                        }
                        catch (OperationCanceledException)
                        {
                            return;
                        }
                        catch (Exception e)
                        {
                            parsedItem.Status = OPERATION_STATUS.Error;
                            parsedItem.StatusMessage = e.Message.ToString();
                        }
                        Action(() => ViewModel.ParsedItemsList.Add(parsedItem));
                    });
                    ViewModel.Diagramm.Build(ViewModel.ParsedItemsList);
                    ViewModel.Performance.Stop();
                    ViewModel.ProgressBar.FillFinaly();

                    ViewModel.StatusMessage = "Success.";
                }
                catch (Exception e)
                {
                    ViewModel.StatusMessage = e.Message;
                }
                ViewModel.ParsingAvailable = true;
            });
        }
        private void Action(Function func = null)
        {
            Application.Current.Dispatcher.BeginInvoke(
            System.Windows.Threading.DispatcherPriority.Background,
            new Action(() =>
            {
                func?.Invoke();
            }));
        }
        public void Cancel()
        {
            if (CancelTokenSource is null) return;
            Action(() => CancelTokenSource.Cancel());
            ViewModel.Diagramm.StopDrawing();
            ViewModel.Performance.Stop();
            ViewModel.ProgressBar.Hide();
        }
    }
}
