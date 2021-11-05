using System;
using System.Threading.Tasks;
using System.Windows.Input;
using TagsCounter.Models.Services;
using TagsCounter.ViewModels;


namespace TagsCounter.Infrastructure.Commands
{
    public class ParseSelectedURLCommand : ICommand
    {
        private readonly MainWindowViewModel ViewModel;
        public event EventHandler CanExecuteChanged;
        public ParseSelectedURLCommand(MainWindowViewModel viewModel) => ViewModel = viewModel;
        public bool CanExecute(object parameter) => true;
        public async void Execute(object parameter)
        {
            if (ViewModel.SelectedURL is null)
            {
                ViewModel.StatusMessage = "Selected URL is empty";
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

                    ViewModel.ItemsCount = Parser.PasreItemsCount(ViewModel.SelectedURL);
                    ViewModel.StatusMessage = $"{ViewModel.SelectedURL} Succesfully parsed. {ViewModel.ItemsCount} <a> tags finded." ;

                    ViewModel.Performance.Stop();
                    ViewModel.ProgressBar.FillFinaly();
                }
                catch (Exception e)
                {
                    ViewModel.Performance.Stop();
                    ViewModel.ProgressBar.Hide();
                    ViewModel.StatusMessage = e.Message;
                }
                ViewModel.ParsingAvailable = true;
            });

        }
    }
}
