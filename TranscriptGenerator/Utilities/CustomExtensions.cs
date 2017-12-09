using System.Windows;
using System.Windows.Media;

namespace TranscriptGenerator.Utilities
{
    public static class CustomExtensions
    {
        public static T FindParent<T>(this DependencyObject child)
        where T : DependencyObject
        {
            // Get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            // End of tree reached
            if (parentObject == null)
                return null;

            // Check if the parent matches the specified type
            var parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                return FindParent<T>(parentObject);
            }
        }

        public static T GetVisualChild<T>(this DependencyObject parent)
        where T : Visual
        {
            T child = default(T);

            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }
    }
}
