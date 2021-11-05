using System;
using System.Timers;
using System.Windows;
using TagsCounter.ViewModels.Basic;

namespace TagsCounter.Models.Services
{
    public class PerformanceMaster : ViewModel
    {
        private Visibility _Statement;
        private double _AmountOfTime;
        private readonly Timer SystemTimer;
        public double AmountOfTime { get => _AmountOfTime; set { _AmountOfTime = value; OnPropertyChanged(); } }
        public Visibility Statement { get => _Statement; set { _Statement = value; OnPropertyChanged(); } }
        public PerformanceMaster()
        {
            SystemTimer = new Timer(100);
            SystemTimer.Elapsed += SystemTimer_Elapsed;
            Statement = Visibility.Hidden;
        }
        public void Start()
        {
            Statement = Visibility.Visible;
            AmountOfTime = 0.0;
            SystemTimer.Start();
        }
        public void Stop() => SystemTimer.Stop();
        private void SystemTimer_Elapsed(object sender, ElapsedEventArgs e) => AmountOfTime = Math.Round(AmountOfTime += 0.1, 3);
    }
}
