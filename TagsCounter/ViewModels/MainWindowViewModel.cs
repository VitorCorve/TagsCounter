using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using TagsCounter.Infrastructure.Commands;
using TagsCounter.Models;
using TagsCounter.Models.Interfaces;
using TagsCounter.Models.Services;
using TagsCounter.ViewModels.Basic;

namespace TagsCounter.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        private CancellationTokenSource CancelTokenSource;
        private CancellationToken CancelToken;
        private int _ItemsCount;
        private string _URL;
        private string _StatusMessage;
        private string _SelectedURL;
        private ObservableCollection<string> _FilesList;
        private ObservableCollection<IParsedItemViewModel> _ParsedItemsList;
        private bool _ParsingAvailable;
        public int ItemsCount { get => _ItemsCount; set { _ItemsCount = value; OnPropertyChanged(); } }
        public string URL { get => _URL; set { _URL = value; OnPropertyChanged(); } }
        public string StatusMessage { get => _StatusMessage; set { _StatusMessage = value; OnPropertyChanged(); } }
        public ObservableCollection<string> FilesList { get => _FilesList; set { _FilesList = value; OnPropertyChanged(); } }
        public ObservableCollection<IParsedItemViewModel> ParsedItemsList { get => _ParsedItemsList; set { _ParsedItemsList = value; OnPropertyChanged(); } }
        public string SelectedURL { get => _SelectedURL; set { _SelectedURL = value; OnPropertyChanged(); } }
        public bool ParsingAvailable { get => _ParsingAvailable; set { _ParsingAvailable = value; OnPropertyChanged(); } }
        public ProgressBarPreviewModel ProgressBar { get; set; }
        public PerformanceMaster Performance { get; set; }
        public ResultsDiagramm Diagramm { get; set; }
        public ParseByLinkCommand ParseByLinkCommand { get; private set; }
        public ParseSelectedURLCommand ParseSelectedURLCommand { get; private set; }
        public ParseAllLinksFromFileCommand ParseAllLinksFromFileCommand { get; private set; }
        public SelectFileCommand SelectFileCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand CloseApplication { get; private set; }
        public MainWindowViewModel()
        {
            ProgressBar = new ProgressBarPreviewModel();
            ParsedItemsList = new ObservableCollection<IParsedItemViewModel>();
            Performance = new PerformanceMaster();
            Diagramm = new ResultsDiagramm();
            ParsingAvailable = true;

            ParseByLinkCommand = new ParseByLinkCommand(this);
            ParseSelectedURLCommand = new ParseSelectedURLCommand(this);
            ParseAllLinksFromFileCommand = new ParseAllLinksFromFileCommand(this);
            SelectFileCommand = new SelectFileCommand(this);
            CancelCommand = new RelayCommand(() => ParseAllLinksFromFileCommand.Cancel());
            CloseApplication = new RelayCommand(() => Application.Current.Shutdown());
        }
    }
}
