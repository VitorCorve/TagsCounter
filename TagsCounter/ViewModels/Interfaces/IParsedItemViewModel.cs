
using System.Windows;
using TagsCounter.Infrastructure.Additional;

namespace TagsCounter.Models.Interfaces
{
    public interface IParsedItemViewModel
    {
        string URL { get; }
        int ItemsCounts { get; set; }
        OPERATION_STATUS Status { get; }
        string StatusColor { get; set; }
        string StatusMessage { get; set; }
        int FadeInWidth { get; set; }
    }
}
