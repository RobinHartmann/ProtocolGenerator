using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace TranscriptGenerator.PageSwitcher.Pages
{
    /// <summary>
    /// Interaktionslogik für LineChooser.xaml
    /// </summary>
    public partial class LineChooser : UserControl, ISwitchable
    {
        private static LineChooser instance;
        private static List<Line> lines = new List<Line>();

        private LineChooser()
        {
            InitializeComponent();
        }

        private void CheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            UpdateBtnContinue();
        }

        public void NavigateBackwards(object sender, RoutedEventArgs e)
        {
            PageSwitcher.Navigate(Pages.TextInput.Instance);
        }

        public void NavigateForwards(object sender, RoutedEventArgs e)
        {
            PageSwitcher.Navigate(ConfigCoarse.Instance);
        }

        public void Prepare()
        {
            PageSwitcher.IsBackEnabled = true;
            UpdateBtnContinue();
        }

        public static LineChooser Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LineChooser();
                }
                return instance;
            }
        }

        public static string Text
        {
            get
            {
                StringBuilder b = new StringBuilder();

                foreach (Line l in lines)
                {
                    b.Append(l.LineText);
                }

                return b.ToString();
            }
            set
            {
                string[] inputSplit = value.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

                lines.Clear();

                foreach (string s in inputSplit)
                {
                    lines.Add(new Line(!string.IsNullOrWhiteSpace(s), s));
                }

                Instance.dgLines.ItemsSource = lines;
                Instance.dgLines.Items.Refresh();
            }
        }

        public static List<Line> Lines
        {
            get
            {
                return lines;
            }
            set
            {
                lines = value;
                Instance.dgLines.ItemsSource = lines;
                Instance.dgLines.Items.Refresh();
            }
        }

        private void UpdateBtnContinue()
        {
            foreach (Line l in lines)
            {
                if (l.TimestampEnabled)
                {
                    PageSwitcher.IsContinueEnabled = true;
                    return;
                }
            }

            PageSwitcher.IsContinueEnabled = false;
        }

        public class Line : INotifyPropertyChanged
        {
            private bool timestampEnabled;
            private DateTime timestamp;
            private string lineText;

            public bool TimestampEnabled
            {
                get
                {
                    return timestampEnabled;
                }
                set
                {
                    if (value != timestampEnabled)
                    {
                        timestampEnabled = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            public DateTime Timestamp
            {
                get
                {
                    return timestamp;
                }
                set
                {
                    if (value != timestamp)
                    {
                        timestamp = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            public string LineText
            {
                get
                {
                    return lineText;
                }
                set
                {
                    if (value != lineText)
                    {
                        lineText = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            public Line(bool timestampEnabled, string lineText)
            {
                this.TimestampEnabled = timestampEnabled;
                this.LineText = lineText;
            }

            public event PropertyChangedEventHandler PropertyChanged;

            private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }
    }
}
