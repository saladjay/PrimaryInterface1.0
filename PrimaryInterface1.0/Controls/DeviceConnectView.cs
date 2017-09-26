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

            InterfaceState.ItemsSource = InterfaceList;
            LeftTreeView.ItemsSelectedChanged += LeftTreeView_ItemsSelectedChanged;
            TopTreeView.ItemsSelectedChanged += TopTreeView_ItemsSelectedChanged;
            LeftTreeView.ItemsStateChanged += LeftTreeView_ItemsStateChanged;
            TopTreeView.ItemsStateChanged += TopTreeView_ItemsStateChanged;
        }

        private void TopTreeView_ItemsStateChanged(ItemInfo Source)
        {
            int DeviceModelIndex = Source.Position - 1;
            DeviceModel TempDeviceModel = Items[DeviceModelIndex] as DeviceModel;
            if(TempDeviceModel==null)
            {
                Debug.WriteLine("wrong device type");
                return;
            }
            if(Source.IsExpanded)
            {
                int index = PositionHelper.IndexOf(TempDeviceModel);
                for (int i = 0; i < TempDeviceModel.InterfaceCount+1; i++)
                {
                    InterfaceState.ColumnCellState[index + i].SingleBool = true;
                }
            }
            else
            {
                int index = PositionHelper.IndexOf(TempDeviceModel);
                for (int i = 0; i < TempDeviceModel.InterfaceCount + 1; i++)
                {
                    InterfaceState.ColumnCellState[index + i].SingleBool = false;
                }
            }
        }

        private void LeftTreeView_ItemsStateChanged(ItemInfo Source)
        {
            int DeviceModelIndex = Source.Position - 1;
            DeviceModel TempDeviceModel = Items[DeviceModelIndex] as DeviceModel;
            if (TempDeviceModel == null)
            {
                Debug.WriteLine("wrong device type");
                return;
            }
            if (Source.IsExpanded)
            {
                int index = PositionHelper.IndexOf(TempDeviceModel);
                for (int i = 0; i < TempDeviceModel.InterfaceCount + 1; i++)
                {
                    InterfaceState.RowCellState[index + i].SingleBool = true;
                }
            }
            else
            {
                int index = PositionHelper.IndexOf(TempDeviceModel);
                for (int i = 0; i < TempDeviceModel.InterfaceCount + 1; i++)
                {
                    InterfaceState.RowCellState[index + i].SingleBool = false;
                }
            }
        }

        private int? oldRowValue = null;
        private void TopTreeView_ItemsSelectedChanged(int Index, object item)
        {
            CTreeViewItem TempItem = item as CTreeViewItem;
            if (TempItem == null)
                return;
            int PositionIndex = 0;
            if (TempItem.Parent is CTreeView)
            {
                DeviceModel TempDevice = TempItem.Tag as DeviceModel;
                PositionIndex = PositionHelper.IndexOf(TempDevice);
            }
            else
            {
                DeviceModel TempDevice = ((CTreeViewItem)TempItem.Parent).Tag as DeviceModel;
                PositionIndex = PositionHelper.IndexOf(TempDevice) + ((CTreeViewItem)TempItem.Parent).Items.IndexOf(TempItem)+1;
            }
            InterfaceState.RowCellState[PositionIndex].IsSelect = true;
            if (oldRowValue!=null && oldColumnValue != PositionIndex)
            {
                InterfaceState.RowCellState[(int)oldRowValue].IsSelect = false;
            }
            oldRowValue = PositionIndex;
        }

        private int? oldColumnValue = null;
        private void LeftTreeView_ItemsSelectedChanged(int Index, object item)
        {
            CTreeViewItem TempItem = item as CTreeViewItem;
            if (TempItem == null)
                return;
            int PositionIndex = 0;
            if (TempItem.Parent is CTreeView)
            {
                DeviceModel TempDevice = TempItem.Tag as DeviceModel;
                PositionIndex = PositionHelper.IndexOf(TempDevice);
            }
            else
            {
                DeviceModel TempDevice = ((CTreeViewItem)TempItem.Parent).Tag as DeviceModel;
                PositionIndex += PositionHelper.IndexOf(TempDevice) + ((CTreeViewItem)TempItem.Parent).Items.IndexOf(TempItem)+1;
            }
            InterfaceState.ColumnCellState[PositionIndex].IsSelect = true;
            if (oldColumnValue!=null&&oldColumnValue!=PositionIndex)
            {
                InterfaceState.ColumnCellState[(int)oldColumnValue].IsSelect = false;
            }
            oldColumnValue = PositionIndex;
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {

        }


        private ObservableCollection<DeviceModel> LeftModelList = new ObservableCollection<DeviceModel>();
        private ObservableCollection<DeviceModel> TopModelList = new ObservableCollection<DeviceModel>();
        private ObservableCollection<DeviceModel> InterfaceList = new ObservableCollection<DeviceModel>();
        private List<object> PositionHelper = new List<object>();
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (DeviceModel item in e.NewItems)
                {
                    PositionHelper.Add(item);
                    foreach (DeviceInterface TempInterface in item.InterfaceList)
                    {
                        PositionHelper.Add(TempInterface);
                    }
                    InterfaceList.Add((DeviceModel)item);
                    DeviceModel TempDeviceModel = item as DeviceModel;
                    Debug.WriteLine(TempDeviceModel.DeviceName);
                    CTreeViewItem LItemRoot = new CTreeViewItem() { Header = TempDeviceModel.DeviceName, Tag = TempDeviceModel };
                    CTreeViewItem TItemRoot = new CTreeViewItem() { Header = TempDeviceModel.DeviceName, Tag = TempDeviceModel };
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
                    int index = PositionHelper.IndexOf(item);
                    for (int i = 0; i < item.InterfaceCount+1; i++)
                    {
                        PositionHelper.RemoveAt(index);
                    }
                    InterfaceList.Remove(item);
                    int Index = LeftTreeView.RemoveCTreeViewItem(item);
                    TopTreeView.RemoveCTreeViewItem(item);
                }
            }
            base.OnItemsChanged(e);
        }


    }
}
