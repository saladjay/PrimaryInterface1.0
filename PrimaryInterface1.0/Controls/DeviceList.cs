using MS.Internal.PresentationFramework;
using PrimaryInterface1._0.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PrimaryInterface1._0.Controls
{
    class DeviceListItem:Control
    {
        private List<DeviceModel> InnerDeviceList = new List<DeviceModel>();
        private ItemsGrid CGridItemsSource = new ItemsGrid();
        [CommonDependencyProperty]
        public IEnumerable ItemsSource
        {
            get { return CGridItemsSource.ItemsSource; }
            set { CGridItemsSource.ItemsSource = value; }
        }

        public static readonly DependencyProperty DockDirectionProperty = DependencyProperty.Register("DockDirection", typeof(_Direction), typeof(DeviceListItem), new PropertyMetadata(_Direction.Left));
        public _Direction DockDirection
        {
            get { return (_Direction)GetValue(DockDirectionProperty); }
            set { SetValue(DockDirectionProperty, value); }
        }

        public static readonly DependencyProperty DeviceHeaderProperty = DependencyProperty.Register("DeviceHeader", typeof(string), typeof(DeviceListItem));
        public string DeviceHeader
        {
            get { return (string)GetValue(DeviceHeaderProperty); }
            set { SetValue(DeviceHeaderProperty, value); }
        }

        #region Construction
        public DeviceListItem()
        {
            CGridItemsSource.ExtendedItemsChanged += CGridItemsSource_ExtendedItemsChanged;
        }
        #endregion

        private List<CellState> _ColumnCellState = new List<Model.CellState>();//top
        public List<CellState> RowCellState
        {
            get { return _ColumnCellState; }
        }

        private void CGridItemsSource_ExtendedItemsChanged(object item, bool AddOrRemove)
        {
               
        }
    }
}
