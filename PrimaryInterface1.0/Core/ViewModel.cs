using PrimaryInterface1._0.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimaryInterface1._0.Core
{
    public class ViewModel
    {
        public static List<List<CellState>> cellsState = new List<List<CellState>>();


        private ObservableCollection<object> _DataCollection = new ObservableCollection<object>();
        public ObservableCollection<object> DataCollection
        {
            get { return _DataCollection; }
            private set { _DataCollection = value; }
        }

        private List<DeviceModel> _InnerDeviceList = new List<DeviceModel>();

        private List<object> _ConstructionHelper = new List<object>();
        public List<object> ConstructionHelper
        {
            get { return _ConstructionHelper; }
        }

        private List<int> _PositionHelper = new List<int>();
        public List<int> PositionHelper
        {
            get { return _PositionHelper; }
        }

        private List<CellState> _ColumnCellState = new List<Model.CellState>();//top
        public List<CellState> ColumnCellState
        {
            get { return _ColumnCellState; }
        }

        private List<CellState> _RowCellState = new List<Model.CellState>();//left
        public List<CellState> RowCellState
        {
            get { return _RowCellState; }
        }

        private int _SelectRow = 0;
        public int SelectRow
        {
            get { return _SelectRow; }
            set
            {
                if (value < 0 && value >= _RowCellState.Count)
                    return;
                if (_SelectRow >= 0 && _SelectRow < _RowCellState.Count)
                    _RowCellState[_SelectRow].IsSelect = false;
                _SelectRow = value;
                _RowCellState[_SelectRow].IsSelect = true;
            }
        }

        private int _SelectColumn = 0;
        public int SelectColumn
        {
            get { return _SelectColumn; }
            set
            {
                if (value < 0 && value >= _ColumnCellState.Count)
                    return;
                if (_SelectColumn >= 0 && _SelectColumn < _ColumnCellState.Count)
                    _ColumnCellState[_SelectColumn].IsSelect = false;
                _SelectColumn = value;
                _ColumnCellState[_SelectColumn].IsSelect = true;
            }
        }

        public int RemoveIndex { get; set; }

        public ViewModel()
        {
            _DataCollection.CollectionChanged += _DataCollection_CollectionChanged;
        }
        private int PositionHelperIndex = 0;

        private void _DataCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems!=null)
            {
                foreach (var item in e.NewItems)
                {
                    if (!(item is DeviceModel))
                        continue;
                    DeviceModel device = item as DeviceModel;

                    _InnerDeviceList.Add(device);
                    _ConstructionHelper.Add(device);
                    foreach (DeviceInterface element in device.InterfaceList)
                    {
                        _ConstructionHelper.Add(element);
                    }
                    for (int i = 0; i < device.InterfaceCount + 1; i++)
                    {
                        _ColumnCellState.Add(new Model.CellState() { RowState = false, ColumnState = false });
                        _RowCellState.Add(new Model.CellState() { RowState = false, ColumnState = false });
                        PositionHelper.Add(PositionHelperIndex++);
                    }
                }
            }
            if(e.OldItems!=null)
            {
                foreach (var item in e.OldItems)
                {
                    if (!(item is DeviceModel))
                        continue;
                    DeviceModel device = item as DeviceModel;
                    int Index = _InnerDeviceList.IndexOf(device);
                    RemoveIndex = _ConstructionHelper.IndexOf(device);
                    for (int i = 0; i < device.InterfaceCount + 1; i++)
                    {
                        _ColumnCellState.RemoveAt(RemoveIndex);
                        _RowCellState.RemoveAt(RemoveIndex);
                        _ConstructionHelper.RemoveAt(RemoveIndex);
                        _PositionHelper.RemoveAt(RemoveIndex);
                    }
                    _InnerDeviceList.RemoveAt(Index);
                }
            }
        }
    }
}
