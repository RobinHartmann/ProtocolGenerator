using System;
using System.Windows;
using System.Windows.Controls;

namespace TranscriptGenerator.PageSwitcher.Pages
{
    /// <summary>
    /// Interaktionslogik für TextInput.xaml
    /// </summary>
    public partial class TextInput : UserControl, ISwitchable
    {
        private static TextInput instance;

        private TextInput()
        {
            InitializeComponent();
        }

        private void tbInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateBtnContinue();
        }

        public void NavigateBackwards(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void NavigateForwards(object sender, RoutedEventArgs e)
        {
            LineChooser.Text = Text;
            PageSwitcher.Navigate(LineChooser.Instance);
        }

        public void Prepare()
        {
            UpdateBtnContinue();
        }

        public static TextInput Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TextInput();
                }

                return instance;
            }
        }

        public static string Text
        {
            get
            {
                return Instance.tbInput.Text;
            }
            set
            {
                Instance.tbInput.Text = value;
            }
        }

        private void UpdateBtnContinue()
        {
            PageSwitcher.IsContinueEnabled = !string.IsNullOrWhiteSpace(tbInput.Text);
        }
    }
}
