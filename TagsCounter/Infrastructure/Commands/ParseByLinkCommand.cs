using System;
using System.Threading.Tasks;
using System.Windows.Input;
using TagsCounter.Models.Services;
using TagsCounter.ViewModels;

namespace TagsCounter.Infrastructure.Commands
{
    public class ParseByLinkCommand : ICommand
    {
        private readonly MainWindowViewModel ViewModel;
        public event EventHandler CanExecuteChanged;
        public ParseByLinkCommand(MainWindowViewModel viewModel) => ViewModel = viewModel;
        public bool CanExecute(object parameter) => true;
        public async void Execute(object parameter)
        {
            if (ViewModel.URL is null)
            {
                ViewModel.StatusMessage = "URL is empty";
                return;
            }

            ViewModel.StatusMessage = null;

            await Task.Run(() =>
            {
                ViewModel.ParsingAvailable = false;
                try
                {
                    ViewModel.Performance.Start();
                    ViewModel.ProgressBar.FillByStep();

                    ViewModel.ItemsCount = Parser.PasreItemsCount(parameter.ToString());
                    ViewModel.StatusMessage = $"{ViewModel.URL} Succesfully parsed. {ViewModel.ItemsCount} <a> tags finded.";

                    ViewModel.Performance.Stop();
                    ViewModel.ProgressBar.FillFinaly();
                }
                catch (Exception e)
                {
                    ViewModel.ProgressBar.Hide();
                    ViewModel.Performance.Stop();
                    ViewModel.StatusMessage = e.Message;
                }
                ViewModel.ParsingAvailable = true;
            });

        }
    }
}
