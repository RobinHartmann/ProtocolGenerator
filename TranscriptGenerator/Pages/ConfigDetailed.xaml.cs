using TranscriptGenerator.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.Core.Input;

namespace TranscriptGenerator.PageSwitcher.Pages
{
    /// <summary>
    /// Interaktionslogik für ConfigDetailed.xaml
    /// </summary>
    public partial class ConfigDetailed : UserControl, ISwitchable
    {
        private static readonly char[] IGNORED_CHARS = { ' ', ',', ';', ':', '.', '?', '!', '\'', '\"', '/', '\\', '\n', '\r' };

        private static ConfigDetailed instance;
        private static List<LineChooser.Line> lines;
        private static DateTime lastTimestamp;
        private static DataGridCellInfo lastSelectedTextCell;

        private static bool cellEditHandled;

        public ConfigDetailed()
        {
            InitializeComponent();
        }

        private void DateTimeUpDown_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            DateTimeUpDown timePicker = sender as DateTimeUpDown;
            DataGridCell senderCell = timePicker.FindParent<DataGridCell>();

            UpdateTimestamps(timePicker);
            lastTimestamp = ExtractTimestamp(timePicker);

            senderCell.IsEditing = true;
        }

        private void DateTimeUpDown_MouseEnter(object sender, MouseEventArgs e)
        {
            lastTimestamp = ExtractTimestamp(sender as DateTimeUpDown);
        }

        private void DateTimeUpDown_MouseLeave(object sender, MouseEventArgs e)
        {
            DateTimeUpDown timePicker = sender as DateTimeUpDown;
            DataGridCell senderCell = timePicker.FindParent<DataGridCell>();
            DataGridCellInfo currentCellInfo = Instance.dgLines.CurrentCell;

            if (senderCell != null && (senderCell.Content as ContentPresenter).Content == currentCellInfo.Item && senderCell.Column == currentCellInfo.Column)
            {
                UpdateTimestamps(timePicker);
            }
        }

        public void Prepare()
        {
            Lines = ConfigCoarse.Lines;

            PageSwitcher.IsBackEnabled = true;
            PageSwitcher.IsContinueEnabled = true;
            PageSwitcher.ContentContinue = "Copy to Clipboard";

            InitializeTimestamps(ConfigCoarse.SettingsCoarse);
        }

        public void NavigateBackwards(object sender, RoutedEventArgs e)
        {
            PageSwitcher.Navigate(ConfigCoarse.Instance);
        }

        public void NavigateForwards(object sender, RoutedEventArgs e)
        {
            StringBuilder clipboardText = new StringBuilder();

            foreach (LineChooser.Line l in Lines)
            {
                if (l.TimestampEnabled)
                {
                    clipboardText.Append(l.Timestamp.ToString("HH:mm:ss") + ": ");
                }
                else if (l.LineText.Replace(Environment.NewLine, string.Empty).Length > 0)
                {
                    clipboardText.Append("          ");
                }

                clipboardText.AppendLine(l.LineText);
            }

            Clipboard.SetText(clipboardText.ToString());
        }

        public static ConfigDetailed Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConfigDetailed();
                }

                return instance;
            }
        }

        public static List<LineChooser.Line> Lines
        {
            get
            {
                return lines;
            }
            set
            {
                lines = value;

                if (Instance.dgLines.ItemsSource == null)
                {
                    Instance.dgLines.ItemsSource = lines;
                }
                else
                {
                    Instance.dgLines.Items.Refresh();
                }
            }
        }

        private void InitializeTimestamps(ConfigCoarse.SettingsConfigCoarse settings)
        {
            DateTime nextTime = settings.StartingTime;
            int timeWithinLines = settings.TimeWithinLines;
            ConfigCoarse.TimeWithinLinesMode timeWithinLinesMode = settings.TimeWithinLinesMode;
            int timeBetweenLines = settings.TimeBetweenLines;
            bool firstTimestampReached = false;

            foreach (LineChooser.Line l in Lines)
            {
                if (l.TimestampEnabled)
                {
                    firstTimestampReached = true;
                    l.Timestamp = nextTime;

                    if (timeBetweenLines != 0)
                    {
                        nextTime = nextTime.AddMilliseconds(timeBetweenLines);
                    }
                }

                if (firstTimestampReached)
                {
                    string lineText = l.LineText;

                    if (timeWithinLines != 0 && !string.IsNullOrWhiteSpace(lineText))
                    {
                        int factor = 0;
                        string[] words = lineText.Split(IGNORED_CHARS, StringSplitOptions.RemoveEmptyEntries);

                        if (timeWithinLinesMode == ConfigCoarse.TimeWithinLinesMode.PerLetter)
                        {
                            factor = string.Join(string.Empty, words).ToCharArray().Length;
                        }
                        else
                        {
                            factor = words.Length;
                        }

                        nextTime = nextTime.AddMilliseconds(timeWithinLines * factor);
                    }
                }
            }
        }

        private void UpdateTimestamps(DateTimeUpDown currentTimePicker)
        {
            TimeSpan timestampValueChange = ExtractTimestamp(currentTimePicker) - lastTimestamp;

            if (timestampValueChange != TimeSpan.Zero)
            {
                for (int i = Lines.IndexOf(dgLines.CurrentCell.Item as LineChooser.Line) + 1; i < Lines.Count; i++)
                {
                    LineChooser.Line l = Lines[i];

                    if (!l.TimestampEnabled)
                    {
                        continue;
                    }

                    l.Timestamp += timestampValueChange;
                }
            }
        }

        private DateTime ExtractTimestamp(DateTimeUpDown timePicker)
        {
            ContentPresenter senderContentPresenter = timePicker.FindParent<ContentPresenter>();
            return (senderContentPresenter.Content as LineChooser.Line).Timestamp;
        }

        private void dgLines_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column as DataGridTextColumn != null && !cellEditHandled)
            {
                lastSelectedTextCell = Instance.dgLines.CurrentCell;

                if (e.EditAction == DataGridEditAction.Commit)
                {
                    cellEditHandled = true;
                    Instance.dgLines.CommitEdit();
                    Instance.dgLines.CommitEdit();
                }
                else
                {
                    cellEditHandled = true;
                    Instance.dgLines.CancelEdit();
                    Instance.dgLines.CancelEdit();
                }

                cellEditHandled = false;
            }
        }
    }
}
