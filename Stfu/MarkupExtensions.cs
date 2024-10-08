//
//  Stfu — Sam’s Tiny Framework Utilities
//
//  Copyright © 2017–2024 Sam Hocevar <sam@hocevar.net>
//
//  This library is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Stfu.Wpf
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class BoolInverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider) => this;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is bool b ? !b : value;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value is bool b ? !b : value;
    }

    /// <summary>
    /// Convert a boolean to a visibility value suitable for XAML objects.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public sealed class BoolToVisibilityConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider) => this;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is bool ? (bool)value ? IfTrue : IfFalse : null;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Equals(value, IfTrue) ? true : Equals(value, IfFalse) ? false : null;

        public Visibility IfTrue { get; set; } = Visibility.Visible;
        public Visibility IfFalse { get; set; } = Visibility.Collapsed;
    }
}
