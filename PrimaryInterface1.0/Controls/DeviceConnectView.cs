using PrimaryInterface1._0.Controls;
using PrimaryInterface1._0.Core;
using PrimaryInterface1._0.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace PrimaryInterface1._0.Controls
{
    public class DeviceConnectView : ItemsControl
    {
        static DeviceConnectView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DeviceConnectView), new FrameworkPropertyMetadata(typeof(DeviceConnectView)));
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(DeviceConnectView));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyPropertyKey TopItemPropertyKey = DependencyProperty.RegisterReadOnly("TopItem", typeof(object), typeof(DeviceConnectView), new PropertyMetadata(default(object)));
        public static readonly DependencyProperty TopItemProperty = TopItemPropertyKey.DependencyProperty;
        public object TopItem
        {
            get { return (object)GetValue(TopItemPropertyKey.DependencyProperty); }
        }

        public static readonly DependencyPropertyKey LeftItemPropertyKey = DependencyProperty.RegisterReadOnly("LeftItem", typeof(object), typeof(DeviceConnectView), new PropertyMetadata(default(object)));
        public static readonly DependencyProperty LeftItemProperty = LeftItemPropertyKey.DependencyProperty;
        public object LeftItem
        {
            get { return (object)GetValue(LeftItemPropertyKey.DependencyProperty); }
        }

        public static readonly DependencyPropertyKey ButtomItemPropertyKey = DependencyProperty.RegisterReadOnly("ButtomItem", typeof(object), typeof(DeviceConnectView), new PropertyMetadata(default(object)));
        public static readonly DependencyProperty ButtomItemProperty = ButtomItemPropertyKey.DependencyProperty;
        public object ButtomItem
        {
            get { return (object)GetValue(ButtomItemPropertyKey.DependencyProperty); }
        }

        private DeviceItemList LeftTreeView { get; set; }
        private DeviceItemList TopTreeView { get; set; }
        private DeviceInterfaceGrid InterfaceState { get; set; }
        private ViewModel _DataSource = null;
        public ViewModel DataSource
        {
            get { return _DataSource; }
            set
            {
                _DataSource = value;
                this.ItemsSource = _DataSource.DataCollection;
            }
        }
        public DeviceConnectView()
        {
            Style = (Style)FindResource("DeviceConnectViewStyle");
            LeftTreeView = new DeviceItemList() { DockDirection = _Direction.Left };
            TopTreeView = new DeviceItemList() { DockDirection = _Direction.Top };
            InterfaceState = new DeviceInterfaceGrid();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            this.SetValue(LeftItemPropertyKey, LeftTreeView);
            this.SetValue(TopItemPropertyKey, TopTreeView);
            this.SetValue(ButtomItemPropertyKey, InterfaceState);
        }


        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            InterfaceState.DataSource = LeftTreeView.DataSource = TopTreeView.DataSource = DataSource;
        }
    }
}
