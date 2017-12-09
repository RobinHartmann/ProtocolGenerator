using TranscriptGenerator.PageSwitcher.Pages;
using System;
using System.Windows;
using System.Windows.Controls;

namespace TranscriptGenerator.PageSwitcher
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class PageSwitcher : Window
    {
        public static readonly string DEF_CONTENT_CONTINUE = "Continue";

        public PageSwitcher()
        {
            InitializeComponent();

            Instance = this;
            Instance.Title = "Transcript Generator";

            Navigate(Pages.TextInput.Instance);
        }

        public void btnBack_Click(object sender, RoutedEventArgs e)
        {
            ISwitchable s = Page as ISwitchable;
            s.NavigateBackwards(sender, e);
        }

        public void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            ISwitchable s = Page as ISwitchable;
            s.NavigateForwards(sender, e);
        }

        public static void Navigate(UserControl nextPage)
        {
            ISwitchable s = nextPage as ISwitchable;

            if (s != null)
            {
                IsBackEnabled = false;
                IsContinueEnabled = false;

                ContentContinue = DEF_CONTENT_CONTINUE;

                s.Prepare();
            }
            else
            {
                throw new NotImplementedException("NextPage doesn't implement the ISwitchable interface " + nextPage.Name.ToString());
            }

            Page = nextPage;
        }

        public static PageSwitcher Instance
        {
            get;
            private set;
        }

        private static UserControl Page
        {
            get
            {
                return Instance.pageArea.Children[0] as UserControl;
            }
            set
            {
                Instance.pageArea.Children.Clear();
                Instance.pageArea.Children.Add(value);
            }
        }

        public static bool IsBackEnabled
        {
            get
            {
                return Instance.btnBack.IsEnabled;
            }
            set
            {
                Instance.btnBack.IsEnabled = value;
            }
        }

        public static bool IsContinueEnabled
        {
            get
            {
                return Instance.btnContinue.IsEnabled;
            }
            set
            {
                Instance.btnContinue.IsEnabled = value;
            }
        }

        public static string ContentContinue
        {
            get
            {
                return Instance.btnContinue.Content as string;
            }
            set
            {
                Instance.btnContinue.Content = value;
            }
        }
    }
}
