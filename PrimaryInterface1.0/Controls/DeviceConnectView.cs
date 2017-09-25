using PrimaryInterface1._0.Controls;
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

        private CTreeView LeftTreeView { get; set; }
        private CTreeView TopTreeView { get; set; }
        private InterfaceGrid_01 InterfaceState { get; set; }
        public DeviceConnectView()
        {
            Style = (Style)FindResource("DeviceConnectViewStyle");
            LeftTreeView = new CTreeView() { Direction = _Direction.Left };
            TopTreeView = new CTreeView() { Direction = _Direction.Top };
            InterfaceState = new InterfaceGrid_01();
            //InterfaceState = new CGrid() { };
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            this.SetValue(LeftItemPropertyKey, LeftTreeView);
            this.SetValue(TopItemPropertyKey, TopTreeView);
            this.SetValue(ButtomItemPropertyKey, InterfaceState);
            //LeftTreeView.ItemsSource = LeftModelList;
            //TopTreeView.ItemsSource = TopModelList;
            InterfaceState.ItemsSource = InterfaceList;
            //InterfaceState.SetBinding(CGrid.PrimaryColumnProperty, new Binding("FirstLevelItemsCount") { Source = TopTreeView });
            //InterfaceState.SetBinding(CGrid.PrimaryRowProperty, new Binding("FirstLevelItemsCount") { Source = LeftTreeView });
            //InterfaceState.SetBinding(CGrid.NewColumnProperty, new Binding("AddNewRanks") { Source = TopTreeView });
            //InterfaceState.SetBinding(CGrid.NewRowProperty, new Binding("AddNewRanks") { Source = LeftTreeView });
            //InterfaceState.SetBinding(CGrid.ColumnItemsStateProperty, new Binding("ItemsState") { Source = TopTreeView });
            //InterfaceState.SetBinding(CGrid.RowItemsStateProperty, new Binding("ItemsState") { Source = LeftTreeView });

            LeftTreeView.SetBinding(CTreeView.ExpandedIndexProperty, new Binding("ExpandedRow") { Source = InterfaceState });
            TopTreeView.SetBinding(CTreeView.ExpandedIndexProperty, new Binding("ExpandedColumn") { Source = InterfaceState });

            //InterfaceState.SetBinding(CGrid.SelectedColumnProperty, new Binding("SelectRowIndex") { Source = TopTreeView, Mode = BindingMode.TwoWay });
            //InterfaceState.SetBinding(CGrid.SelectedRowProperty, new Binding("SelectRowIndex") { Source = LeftTreeView, Mode = BindingMode.TwoWay });
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {

        }


        private ObservableCollection<DeviceModel> LeftModelList = new ObservableCollection<DeviceModel>();
        private ObservableCollection<DeviceModel> TopModelList = new ObservableCollection<DeviceModel>();
        private ObservableCollection<DeviceModel> InterfaceList = new ObservableCollection<DeviceModel>();
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (DeviceModel item in e.NewItems)
                {
                    InterfaceList.Add((DeviceModel)item);
                    DeviceModel TempDeviceModel = item as DeviceModel;
                    Debug.WriteLine(TempDeviceModel.DeviceName);
                    CTreeViewItem LItemRoot = new CTreeViewItem() { Header = TempDeviceModel.DeviceName };
                    CTreeViewItem TItemRoot = new CTreeViewItem() { Header = TempDeviceModel.DeviceName };
                    foreach (DeviceInterface i in TempDeviceModel.InterfaceList)
                    {
                        //if (Direction == _Direction.Left)
                        LItemRoot.AddCTreeViewItem(new CTreeViewItem() { Header = "Input" + i.InterfaceName });
                        //else
                        TItemRoot.AddCTreeViewItem(new CTreeViewItem() { Header = "Output" + i.InterfaceName });
                    }
                    LeftTreeView.AddCTreeViewItem(LItemRoot);
                    TopTreeView.AddCTreeViewItem(TItemRoot);
                }
            }
            if (e.OldItems != null)
            {
                foreach (DeviceModel item in e.OldItems)
                {
                    InterfaceList.Remove(item);
                    int Index = LeftTreeView.RemoveCTreeViewItem(item.DeviceName);
                    TopTreeView.RemoveCTreeViewItem(item.DeviceName);
                }
            }
            base.OnItemsChanged(e);
        }
    }
}
