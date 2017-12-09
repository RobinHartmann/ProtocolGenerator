using System;
using System.Text.RegularExpressions;
using Xceed.Wpf.Toolkit;

namespace TranscriptGenerator.Utilities
{
    class TimePicker : DateTimeUpDown
    {
        private static readonly Regex LONG_TIME_PATTERN = new Regex("^([01]?[0-9]|2[0-3])(:[0-5]?[0-9]){2}$");

        public new DateTimeFormat Format
        {
            get
            {
                return base.Format;
            }
            private set
            {
                base.Format = value;
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Format = DateTimeFormat.LongTime;
        }

        protected override void OnTextChanged(string previousValue, string currentValue)
        {
            if (!string.IsNullOrEmpty(previousValue) && (string.IsNullOrEmpty(currentValue) || !LONG_TIME_PATTERN.IsMatch(currentValue)))
            {
                base.Text = previousValue;
            }
            else
            {
                base.OnTextChanged(previousValue, currentValue);
            }
        }

        protected override void OnValueChanged(DateTime? oldValue, DateTime? newValue)
        {
            if (oldValue != null && newValue == null)
            {
                base.Value = oldValue;
            }
            else
            {
                base.OnValueChanged(oldValue, newValue);
            }
        }
    }
}
