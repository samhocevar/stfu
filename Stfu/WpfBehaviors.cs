//
//  Stfu — Sam’s Tiny Framework Utilities
//
//  Copyright © 2017–2021 Sam Hocevar <sam@hocevar.net>
//
//  This library is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//

using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Stfu.Wpf
{
    public static class Behaviors
    {
        /// <summary>
        /// Attach this property to a VirtualizingStackPanel as well as its containing ListView
        /// to enable per-pixel scrolling. Uses ScrollUnitProperty (.NET Framework 4.5 and later)
        /// when available, otherwise uses the private VirtualizingPanel.IsPixelBased property
        /// through reflexion. Code is inspired by https://stackoverflow.com/a/17431815/111461
        /// </summary>
        public static readonly DependencyProperty SmoothScrollingProperty =
            DependencyProperty.RegisterAttached("SmoothScrolling", typeof(bool), typeof(Behaviors),
                                                new UIPropertyMetadata(false, SmoothScrollingChanged));

        public static bool GetSmoothScrolling(DependencyObject o)
            => (bool)o.GetValue(SmoothScrollingProperty);

        public static void SetSmoothScrolling(DependencyObject o, bool val)
            => o.SetValue(SmoothScrollingProperty, val);

        private static void SmoothScrollingChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o is VirtualizingPanel || o is ItemsControl)
            {
                var enum_type = typeof(Window).Assembly.GetType("System.Windows.Controls.ScrollUnit");
                var field_flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;
                var field = typeof(VirtualizingPanel).GetField("ScrollUnitProperty", field_flags);
                if (enum_type != null && field?.GetValue(null) is DependencyProperty dp)
                {
                    o.SetValue(dp, Enum.Parse(enum_type, (bool)e.NewValue ? "Pixel" : "Item"));
                }
                else if (o is VirtualizingPanel)
                {
                    var prop_flags = BindingFlags.NonPublic | BindingFlags.Instance;
                    var prop = o.GetType().GetProperty("IsPixelBased", prop_flags);
                    prop?.SetValue(o, (bool)e.NewValue, null);
                }
            }
        }
    }
}
