using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using TagsCounter.Models.Interfaces;
using TagsCounter.ViewModels.Basic;

namespace TagsCounter.Models
{
    public class ResultsDiagramm : ViewModel
    {
        public delegate void Function();
        private readonly Timer DrawTimer;

        private ObservableCollection<IParsedItemViewModel> _Results;
        public ObservableCollection<IParsedItemViewModel> Results { get => _Results; set { _Results = value; OnPropertyChanged(); } }
        public ResultsDiagramm()
        {
            DrawTimer = new Timer(5);
            DrawTimer.Elapsed += DrawTimer_Elapsed;
        }
        private void DrawTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Results is null) return;

            Action(() =>
            {
                var collection = Results;
                Parallel.ForEach(collection, (item) =>
                {
                    if (item.FadeInWidth < item.ItemsCounts)
                    {
                        item.FadeInWidth += (int) item.ItemsCounts / 100;
                    }
                });
                Results = null;
                Results = collection;
            });
        }
        public void Build(ObservableCollection<IParsedItemViewModel> collection) => Initialize(Sort(collection));
        private IEnumerable<IParsedItemViewModel> Sort(IEnumerable<IParsedItemViewModel> collection)
        {
            var sortedList = from x in collection
                             orderby x.ItemsCounts descending
                             select x;
            return sortedList;
        }
        private void Initialize(IEnumerable<IParsedItemViewModel> collection)
        {
            Results = new ObservableCollection<IParsedItemViewModel>();
            foreach (var item in collection)
            {
                Action(() => Results.Add(item));
            }
            DrawTimer.Start();
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
        public void Reset() => Results = new ObservableCollection<IParsedItemViewModel>();
        public void StopDrawing() => DrawTimer.Stop();
    }
}
