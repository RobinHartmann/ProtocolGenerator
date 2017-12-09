using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace TranscriptGenerator.PageSwitcher.Pages
{
    /// <summary>
    /// Interaktionslogik für TimestampConfigCoarse.xaml
    /// </summary>
    public partial class ConfigCoarse : UserControl, ISwitchable
    {
        private static ConfigCoarse instance;

        public ConfigCoarse()
        {
            InitializeComponent();
        }

        public enum TimeWithinLinesMode
        {
            PerLetter,
            PerWord
        }

        public void NavigateBackwards(object sender, RoutedEventArgs e)
        {
            PageSwitcher.Navigate(LineChooser.Instance);
        }

        public void NavigateForwards(object sender, RoutedEventArgs e)
        {
            TimeWithinLinesMode mode = rbtPerLetter.IsChecked.GetValueOrDefault() ? TimeWithinLinesMode.PerLetter : TimeWithinLinesMode.PerWord;
            SettingsCoarse = new SettingsConfigCoarse(timStartingTime.Value, intTimeWithinLines.Value, mode, intTimeBetweenLines.Value);

            PageSwitcher.Navigate(ConfigDetailed.Instance);
        }

        public void Prepare()
        {
            Lines = LineChooser.Lines;

            PageSwitcher.IsBackEnabled = true;
            PageSwitcher.IsContinueEnabled = true;
        }

        public static ConfigCoarse Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConfigCoarse();
                }

                return instance;
            }
        }

        public class SettingsConfigCoarse
        {
            public DateTime StartingTime
            {
                get;
                private set;
            }

            public int TimeWithinLines
            {
                get;
                private set;
            }

            public TimeWithinLinesMode TimeWithinLinesMode
            {
                get;
                private set;
            }

            public int TimeBetweenLines
            {
                get;
                private set;
            }

            public SettingsConfigCoarse(DateTime? startingTime, int? timeWithinLines, TimeWithinLinesMode timeWithinLinesMode, int? timeBetweenLines)
            {
                this.StartingTime = startingTime.GetValueOrDefault();
                this.TimeWithinLines = timeWithinLines.GetValueOrDefault();
                this.TimeWithinLinesMode = timeWithinLinesMode;
                this.TimeBetweenLines = timeBetweenLines.GetValueOrDefault();
            }
        }

        public static List<LineChooser.Line> Lines
        {
            get;
            set;
        }

        public static SettingsConfigCoarse SettingsCoarse
        {
            get;
            protected set;
        }
    }
}
