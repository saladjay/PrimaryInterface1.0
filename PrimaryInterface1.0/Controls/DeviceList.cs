using MS.Internal.PresentationFramework;
using PrimaryInterface1._0.Core;
using PrimaryInterface1._0.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace PrimaryInterface1._0.Controls
{
    class DeviceItemList:Grid
    {
        private ItemsGrid CGridItemsSource = new ItemsGrid();
        [CommonDependencyProperty]
        public IEnumerable ItemsSource
        {
            get { return CGridItemsSource.ItemsSource; }
            set { CGridItemsSource.ItemsSource = value; }
        }

        public static readonly DependencyProperty DockDirectionProperty = DependencyProperty.Register("DockDirection", typeof(_Direction), typeof(DeviceItemList), new PropertyMetadata(_Direction.Left));
        public _Direction DockDirection
        {
            get { return (_Direction)GetValue(DockDirectionProperty); }
            set { SetValue(DockDirectionProperty, value); }
        }

        public static readonly DependencyProperty DeviceHeaderProperty = DependencyProperty.Register("DeviceHeader", typeof(string), typeof(DeviceItemList));
        public string DeviceHeader
        {
            get { return (string)GetValue(DeviceHeaderProperty); }
            set { SetValue(DeviceHeaderProperty, value); }
        }

        #region Construction
        public DeviceItemList()
        {
            CGridItemsSource.ExtendedItemsChanged += CGridItemsSource_ExtendedItemsChanged;
        }
        #endregion

        private ViewModel _DataSource = null;
        public ViewModel DataSource
        {
            get { return _DataSource; }
            set
            {
                _DataSource = value;
                if (DockDirection == _Direction.Left)
                    SideCellState = _DataSource.RowCellState;
                else
                    SideCellState = _DataSource.ColumnCellState;
                ConstructionHelper = _DataSource.ConstructionHelper;
                PositionHelper = _DataSource.PositionHelper;
                ItemsSource = _DataSource.DataCollection;
            }
        }
        private List<CellState> SideCellState { get; set; }
        private List<object> ConstructionHelper { get; set; }
        private List<int> PositionHelper { get; set; }
        public List<DeviceModel> InnerDeviceList = new List<DeviceModel>();
        private void CGridItemsSource_ExtendedItemsChanged(object item, bool AddOrRemove)
        {
            if (!(item is DeviceModel))
                return;
            DeviceModel device = item as DeviceModel;
            if(AddOrRemove)
            {
                InnerDeviceList.Add(device);
                for (int i = 0; i < device.InterfaceCount+1; i++)
                {
                    if (DockDirection == _Direction.Left)
                        this.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(20, GridUnitType.Auto) });
                    else
                        this.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(20, GridUnitType.Auto) });
                }

                int Index = ConstructionHelper.IndexOf(device);
                for (int i = 0; i < device.InterfaceCount+1; i++)
                {
                    CTreeViewItem tempItem = DeviceItemFactory.CreateItem(ConstructionHelper[Index + i], Index + i, this) as CTreeViewItem;
                    this.Children.Add(tempItem);
                    if (DockDirection == _Direction.Left)
                        Grid.SetRow(tempItem, PositionHelper[Index + i]);
                    else
                        Grid.SetColumn(tempItem, PositionHelper[Index + i]);
                    ItemsList.Add(tempItem);
                }
            }
            else
            {
                int Index = InnerDeviceList.IndexOf(device);
                int RemoveIndex = DataSource.RemoveIndex;//ConstructionHelper.IndexOf(device);
                InnerDeviceList.RemoveAt(Index);
                for (int i = 0; i < device.InterfaceCount+1; i++)
                {
                    this.Children.RemoveAt(RemoveIndex);
                    ItemsList.RemoveAt(RemoveIndex);
                }
            }
        }
        private List<Control> ItemsList = new List<Control>();

        internal void ItemExpandHandler(bool Open,CTreeViewItem source)
        {
            DeviceModel TempDevice = source.Tag as DeviceModel;
            int Index = ItemsList.IndexOf(source);
            for (int i = 0; i < TempDevice.InterfaceCount+1; i++)
            {
                SideCellState[Index + i].SingleBool = Open;
            }
        }

        internal void ItemSelectHandler(bool Selected,CTreeViewItem source)
        {
            int Index = ItemsList.IndexOf(source);
            if (DockDirection == _Direction.Left)
                DataSource.SelectRow = Index;
            else
                DataSource.SelectColumn = Index;
        }

        public class DeviceItemFactory
        {
            public static Control CreateItem(object a,int Index, DeviceItemList DeviceList)
            {
                Control Good = null;
                if (a is DeviceModel)
                {
                    DeviceModel A = a as DeviceModel;
                    Good = new CTreeViewItem() { Header = A.DeviceName, Direction = DeviceList.DockDirection,Tag=A };
                    Good.Style = (Style)Good.FindResource("CTreeViewItemStyle2");
                    Good.SetBinding(CTreeViewItem.OpenProperty, new Binding("SingleBool") { Source = DeviceList.SideCellState[Index] });
                    Good.SetBinding(CTreeViewItem.MouseSelectedProperty, new Binding("IsSelect") { Source = DeviceList.SideCellState[Index] });
                    ((CTreeViewItem)Good).ExpandItems += DeviceList.ItemExpandHandler;
                    ((CTreeViewItem)Good).SelectItem += DeviceList.ItemSelectHandler;
                }
                else if (a is DeviceInterface)
                {
                    DeviceInterface A = a as DeviceInterface;
                    if(DeviceList.DockDirection==_Direction.Left)
                    {
                        Good = new CTreeViewItem() { Header = "Input" + A.InterfaceName, Direction = _Direction.Left, HideBtn = true };
                    }
                    else
                    {
                        Good = new CTreeViewItem() { Header = "Output" + A.InterfaceName, Direction = _Direction.Top, HideBtn = true };
                    }
                    Good.Style = (Style)Good.FindResource("CTreeViewItemStyle2");
                    Good.SetBinding(CTreeViewItem.VisibilityProperty, new Binding("SingleBool") { Source = DeviceList.SideCellState[Index], Converter=Converter.CellVisibilityConverter2 });
                    Good.SetBinding(CTreeViewItem.MouseSelectedProperty, new Binding("IsSelect") { Source = DeviceList.SideCellState[Index] });
                    ((CTreeViewItem)Good).SelectItem += DeviceList.ItemSelectHandler;
                }
                return Good;
            }
        }
    }
}
