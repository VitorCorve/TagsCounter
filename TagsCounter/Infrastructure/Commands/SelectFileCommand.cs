using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TagsCounter.Models.Services;
using TagsCounter.ViewModels;

namespace TagsCounter.Infrastructure.Commands
{
    public class SelectFileCommand : ICommand
    {
        private readonly MainWindowViewModel ViewModel;
        public event EventHandler CanExecuteChanged;
        public SelectFileCommand(MainWindowViewModel viewModel) => ViewModel = viewModel;
        public bool CanExecute(object parameter) => true;
        public async void Execute(object parameter)
        {
            await Task.Run(() =>
            {
                try
                {
                    var result = FileManager.ReadSelectedFile();

                    if (result is null)
                    {
                        return;
                    }
                    ViewModel.FilesList = FileManager.CreateObservableCollection(result);
                }
                catch (Exception e)
                {
                    ViewModel.StatusMessage = e.Message;
                }
            });

        }

    }
}
