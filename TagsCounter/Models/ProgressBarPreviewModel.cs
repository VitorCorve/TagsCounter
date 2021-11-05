using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows;
using TagsCounter.Infrastructure.Additional;

namespace TagsCounter.Models
{
    public class ProgressBarPreviewModel : INotifyPropertyChanged
    {
        private OPERATION_STATUS _Status;
        private double _Value;
        private Visibility _Statement;
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public OPERATION_STATUS Status { get => _Status; set { _Status = value; OnPropertyChanged(); } }
        public double Value { get => _Value; set { _Value = value; ValidateMax(value); OnPropertyChanged(); } }
        public Visibility Statement { get => _Statement; set { _Statement = value; OnPropertyChanged(); } }
        private Timer ProgressTimer;
        private Timer FinalizationTimer;
        public ProgressBarPreviewModel()
        {
            Statement = Visibility.Hidden;
            MinValue = 0;
            MaxValue = 100;
        }
        public void Show() => Statement = Visibility.Visible;
        public void Hide() => Statement = Visibility.Hidden;
        public event PropertyChangedEventHandler PropertyChanged;
        private double ValidateMax(double value)
        {
            if (value > 100.0)
                return 100.0;
            return value;
        }
        public void FillByStep()
        {
            Clean();
            Show();
            Status = OPERATION_STATUS.Processing;
            ProgressTimer = new Timer(1);
            ProgressTimer.Elapsed += ProgressTimer_Elapsed;
            ProgressTimer.Start();
        }

        private void ProgressTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Value < 70.0)
            {
                Value += 0.2;
            }
            else
            {
                ProgressTimer.Stop();
            }
        }
        public void FillFinaly()
        {
            ProgressTimer.Stop();
            FinalizationTimer = new Timer(10);
            FinalizationTimer.Start();
            FinalizationTimer.Elapsed += FinalizationTimer_Elapsed;
        }

        private void FinalizationTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Value < 100.0)
            {
                Value += 5.0;
            }
            else
            {
                Status = OPERATION_STATUS.Done;
                FinalizationTimer.Stop();
            }
        }
        private void Clean()
        {
            Value = 0;
            Statement = Visibility.Hidden;
            Status = OPERATION_STATUS.Awaiting;
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
