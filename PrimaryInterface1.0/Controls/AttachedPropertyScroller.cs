using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PrimaryInterface1._0.Controls
{

    class AttachedPropertyScroller : DependencyObject
    {
        public static readonly DependencyProperty ScrollValueProperty = DependencyProperty.RegisterAttached("ScrollValue", typeof(double), typeof(AttachedPropertyScroller), new PropertyMetadata(0.0, OnScrollValueChanged));

        private static void OnScrollValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is ScrollViewer))
                return;
            ScrollViewer TempScrollViewer = d as ScrollViewer;
            if (TempScrollViewer == null)
                return;
            if (TempScrollViewer.HorizontalScrollBarVisibility == ScrollBarVisibility.Hidden)
                TempScrollViewer.ScrollToHorizontalOffset((double)e.NewValue);

            if (TempScrollViewer.VerticalScrollBarVisibility == ScrollBarVisibility.Hidden)
                TempScrollViewer.ScrollToVerticalOffset((double)e.NewValue);
        }

        public double ScrollValue
        {
            get { return (double)GetValue(ScrollValueProperty); }
            set { SetValue(ScrollValueProperty, value); }
        }

        public static void SetScrollValue(DependencyObject obj, double value)
        {
            obj.SetValue(ScrollValueProperty, value);
        }

        public static double GetScrollValue(DependencyObject obj)
        {
            return (double)obj.GetValue(ScrollValueProperty);
        }
    }

}
