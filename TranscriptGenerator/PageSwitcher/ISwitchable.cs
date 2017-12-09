using System.Windows;

namespace TranscriptGenerator.PageSwitcher
{
    public interface ISwitchable
    {
        void Prepare();
        void NavigateBackwards(object sender, RoutedEventArgs e);
        void NavigateForwards(object sender, RoutedEventArgs e);
    }
}
