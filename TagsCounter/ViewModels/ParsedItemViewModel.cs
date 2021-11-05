using System.Windows;
using TagsCounter.Infrastructure.Additional;
using TagsCounter.Models.Interfaces;

namespace TagsCounter.Models
{
    public class ParsedItemViewModel : IParsedItemViewModel
    {
        private int _FadeInWidth;
        private OPERATION_STATUS _Status;
        public string URL { get; set; }
        public int ItemsCounts { get; set; }
        public OPERATION_STATUS Status { get => _Status; set { _Status = value; ValidateStatus(); } }
        public int FadeInWidth { get => _FadeInWidth; set { _FadeInWidth = value; ValidateStatus(); } }
        public string StatusColor { get; set; }
        public string StatusMessage { get; set; }
        private void ValidateStatus()
        {
            switch (Status)
            {
                case OPERATION_STATUS.Processing:
                    StatusColor = "#20B2AA";
                    return;
                case OPERATION_STATUS.Done:
                    StatusColor = "DarkCyan";
                    return;
                case OPERATION_STATUS.Awaiting:
                    StatusColor = "#E0FFFF";
                    return;
                case OPERATION_STATUS.Error:
                    StatusColor = "#DC143C";
                    return;
                default:
                    break;
            }
        }
    }
}
