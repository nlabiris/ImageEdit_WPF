using System;
using System.Globalization;
using System.Windows.Data;

namespace ImageEdit_WPF.HelperClasses {
    [ValueConversion(typeof (double), typeof (string))]
    public class SliderValueConverter : IValueConverter {
        /// <summary>
        /// Double to String.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            double dValue = (double)value;
            return dValue.ToString("0");
        }

        /// <summary>
        /// String to Double.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            double dValue;
            double.TryParse((string)value, out dValue);
            return dValue;
        }
    }

    [ValueConversion(typeof (double), typeof (string))]
    public class SliderValueConverter_GradientBased : IValueConverter {
        /// <summary>
        /// Double to String.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            double dValue = (double)value;
            return dValue.ToString("F1");
        }

        /// <summary>
        /// String to Double.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            double dValue;
            double.TryParse((string)value, out dValue);
            return dValue;
        }
    }
}
