using MS.Internal.PresentationFramework;
using PrimaryInterface1._0.Core;
using PrimaryInterface1._0.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PrimaryInterface1._0.Controls
{
    /// <summary>
    /// 依照步驟 1a 或 1b 執行，然後執行步驟 2，以便在 XAML 檔中使用此自訂控制項。
    ///
    /// 步驟 1a) 於存在目前專案的 XAML 檔中使用此自訂控制項。
    /// 加入此 XmlNamespace 屬性至標記檔案的根項目為 
    /// 要使用的: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:PrimaryInterface1._0.Controls"
    ///
    ///
    /// 步驟 1b) 於存在其他專案的 XAML 檔中使用此自訂控制項。
    /// 加入此 XmlNamespace 屬性至標記檔案的根項目為 
    /// 要使用的: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:PrimaryInterface1._0.Controls;assembly=PrimaryInterface1._0.Controls"
    ///
    /// 您還必須將 XAML 檔所在專案的專案參考加入
    /// 此專案並重建，以免發生編譯錯誤: 
    ///
    ///     在 [方案總管] 中以滑鼠右鍵按一下目標專案，並按一下
    ///     [加入參考]->[專案]->[瀏覽並選取此專案]
    ///
    ///
    /// 步驟 2)
    /// 開始使用 XAML 檔案中的控制項。
    ///
    ///     <MyNamespace:InterfaceGrid_01/>
    ///
    /// </summary>
    public class InterfaceGrid_01 : Grid
    {
        static InterfaceGrid_01()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(InterfaceGrid_01), new FrameworkPropertyMetadata(typeof(InterfaceGrid_01)));
        }

        private List<DeviceModel> InnerDeviceList = new List<DeviceModel>();
        private ItemsGrid CGridItemsSource = new ItemsGrid();
        [CommonDependencyProperty]
        public IEnumerable ItemsSource
        {
            get { return CGridItemsSource.ItemsSource; }
            set { CGridItemsSource.ItemsSource = value; }
        }

        #region Construction
        public InterfaceGrid_01()
        {
            //this.Children.Add(new Border() { HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch ,BorderBrush= new SolidColorBrush(Color.FromRgb(0,0,0))});
            CGridItemsSource.ExtendedItemsChanged += CGridItemsSource_ExtendedItemsChanged;
        }
        #endregion


        private List<CellState> _ColumnCellState = new List<Model.CellState>();//top
        private List<CellState> _RowCellState = new List<Model.CellState>();//left
        public List<CellState> ColumnCellState
        {
            get { return _ColumnCellState; }
        }

        public List<CellState> RowCellState
        {
            get { return _RowCellState; }
        }


        private List<object> ConstructionHelper = new List<object>();
        private List<CellState> RowCreateHelper = new List<Model.CellState>();
        private List<int> PositionHelper = new List<int>();
        private int PositionHelperIndex = 0;
        //private int CellsColumnIndex = 0;
        private void CGridItemsSource_ExtendedItemsChanged(object item, bool AddOrRemove)
        {
            if (!(item is DeviceModel))
                return;
            DeviceModel device = item as DeviceModel;
            if (AddOrRemove)
            {
                InnerDeviceList.Add(device);
                ConstructionHelper.Add(device);
                foreach (DeviceInterface element in device.InterfaceList)
                {
                    ConstructionHelper.Add(element);
                }

                for (int i = 0; i < device.InterfaceCount + 1; i++)
                {
                    _ColumnCellState.Add(new Model.CellState() { RowState = false, ColumnState = false });
                    _RowCellState.Add(new Model.CellState() { RowState = false, ColumnState = false });
                }

                for (int i = 0; i < device.InterfaceCount + 1; i++)
                {
                    RowCreateHelper.Add(new Model.CellState() { RowState = false, ColumnState = false });
                    PositionHelper.Add(PositionHelperIndex++);
                }

                for (int i = 0; i < device.InterfaceCount + 1; i++)
                {
                    this.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(20, GridUnitType.Auto) });
                    this.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(20, GridUnitType.Auto) });
                }
                foreach (List<Control> RowList in CellsControl)
                {
                    int RowIndex = CellsControl.IndexOf(RowList);
                    for (int i = 0; i < device.InterfaceCount + 1; i++)
                    {
                        object TempDev = ConstructionHelper[RowIndex] as object;
                        if (i == 0)
                        {
                            int ColumnIndex = ConstructionHelper.IndexOf(device);
                            Control CellControl = CellFactory.CreateCell(TempDev, device, RowIndex, ColumnIndex,this);
                            this.Children.Add(CellControl);
                            RowList.Add(CellControl);
                            Grid.SetColumn(CellControl, this.ColumnDefinitions.Count + i - device.InterfaceCount - 1);
                            Grid.SetRow(CellControl, PositionHelper[RowIndex]);
                        }
                        else
                        {
                            int ColumnIndex = ConstructionHelper.IndexOf(device.InterfaceList[i - 1]);
                            Control CellControl = CellFactory.CreateCell(TempDev, device.InterfaceList[i - 1], RowIndex, ColumnIndex,this);
                            Debug.WriteLine(((CLabel)CellControl).ToolTip.ToString() + " " + CellControl.Visibility);
                            this.Children.Add(CellControl);
                            RowList.Add(CellControl);
                            Grid.SetColumn(CellControl, this.ColumnDefinitions.Count + i - device.InterfaceCount - 1);
                            Grid.SetRow(CellControl, PositionHelper[RowIndex]);
                        }
                    }
                }            
                for (int i = 0; i < device.InterfaceCount + 1; i++)
                {
                    List<Control> tempRow = new List<Control>();
                    if(i==0)
                    {
                        for (int ColumnIndex = 0; ColumnIndex < ConstructionHelper.Count; ColumnIndex++)
                        {
                            Control CellControl = CellFactory.CreateCell(device, ConstructionHelper[ColumnIndex], CellsControl.Count, ColumnIndex,this);
                            tempRow.Add(CellControl);
                            this.Children.Add(CellControl);
                            Grid.SetColumn(CellControl, PositionHelper[ColumnIndex]);
                            Grid.SetRow(CellControl, RowDefinitions.Count - device.InterfaceCount - 1 + i);
                        }
                    }
                    else
                    {
                        for (int ColumnIndex = 0; ColumnIndex < ConstructionHelper.Count; ColumnIndex++)
                        {
                            Control CellControl = CellFactory.CreateCell(device.InterfaceList[i - 1], ConstructionHelper[ColumnIndex], CellsControl.Count, ColumnIndex,this);
                            tempRow.Add(CellControl);
                            this.Children.Add(CellControl);
                            Debug.WriteLine(((CLabel)CellControl).ToolTip.ToString() + " " + CellControl.Visibility);
                            Grid.SetColumn(CellControl, PositionHelper[ColumnIndex]);
                            Grid.SetRow(CellControl, RowDefinitions.Count - device.InterfaceCount - 1 + i);
                        }
                    }
                    CellsControl.Add(tempRow);
                }
                Debug.WriteLine("CellsState measure " + _RowCellState.Count + " " + _ColumnCellState.Count);
                Debug.WriteLine("CellsControl measure " + CellsControl.Count + " " + CellsControl.First().Count);
                Debug.WriteLine("grid children count " + this.Children.Count);
            }
            else
            {
                int Index = InnerDeviceList.IndexOf(device);
                int RemoveIndex = ConstructionHelper.IndexOf(device);
                for (int i = 0; i < device.InterfaceCount+1; i++)
                {
                    _ColumnCellState.RemoveAt(RemoveIndex);
                    _RowCellState.RemoveAt(RemoveIndex);
                }

                for (int i = 0; i < device.InterfaceCount+1; i++)
                {
                    ConstructionHelper.RemoveAt(RemoveIndex);
                    RowCreateHelper.RemoveAt(RemoveIndex);
                    PositionHelper.RemoveAt(RemoveIndex);
                }
                InnerDeviceList.Remove((DeviceModel)item);
                for (int i = 0; i < device.InterfaceCount+1; i++)
                {
                    foreach (Control Cell in CellsControl[RemoveIndex])
                    {
                        this.Children.Remove(Cell);
                    }
                    CellsControl.RemoveAt(RemoveIndex);
                }
                foreach (List<Control> rowlist in CellsControl)
                {
                    for (int i = 0; i < device.InterfaceCount+1; i++)
                    {
                        this.Children.Remove(rowlist[RemoveIndex]);
                        rowlist.RemoveAt(RemoveIndex);
                    }
                }
                if (_ColumnCellState.Count == 0)
                    return;
                Debug.WriteLine("CellsState measure " + _RowCellState.Count + " " + _ColumnCellState.Count);
                Debug.WriteLine("CellsControl measure " + CellsControl.Count + " " + CellsControl.First().Count);
                Debug.WriteLine("grid children count " + this.Children.Count);
            }
        }
        private List<List<Control>> CellsControl = new List<List<Control>>();



        public class CellFactory
        {
            public static Control CreateCell(object a, object b, int Row, int Column ,InterfaceGrid_01 temp)
            {
                Control Good = null;
                if (a is DeviceModel && b is DeviceModel)
                {
                    DeviceModel A = a as DeviceModel;
                    DeviceModel B = b as DeviceModel;
                    Good = new CToggleBtn() { };
                    Good.ToolTip = string.Format(A.DeviceName + "&&" + B.DeviceName);
                    Binding B1 = new Binding("SingleBool") { Source = temp._RowCellState[Row] };
                    Binding B2 = new Binding("SingleBool") { Source = temp._ColumnCellState[Column] };

                    MultiBinding MBinding = new MultiBinding() { Mode = BindingMode.OneWay };
                    MBinding.Bindings.Add(B1);
                    MBinding.Bindings.Add(B2);
                    MBinding.Converter = Converter.CellStateConverter;
                    Good.SetBinding(CToggleBtn.ChangedIconProperty, MBinding);
                }
                else if (a is DeviceModel && b is DeviceInterface)
                {
                    DeviceModel A = a as DeviceModel;
                    DeviceInterface B = b as DeviceInterface;
                    Good = new CLabel() { IsCommon = false };
                    Good.ToolTip = string.Format(A.DeviceName);
                    Good.SetBinding(CLabel.VisibilityProperty, new Binding("SingleBool") { Source = temp._ColumnCellState[Column], Converter = Converter.CellVisibilityConverter2 });

                    //Binding SelectBD1 = new Binding("IsSelect") { Source = temp._RowCellState[Row] };
                    //Binding SelectBD2 = new Binding("IsSelect") { Source = temp._ColumnCellState[Column] };
                    //MultiBinding SelectMB = new MultiBinding() { Mode = BindingMode.OneWay };
                    //SelectMB.Bindings.Add(SelectBD1);
                    //SelectMB.Bindings.Add(SelectBD2);
                    //SelectMB.Converter = Converter.SelectConverter;
                    //Good.SetBinding(CLabel.IsSelectedProperty, SelectMB);
                }
                else if (a is DeviceInterface && b is DeviceModel)
                {
                    DeviceInterface A = a as DeviceInterface;
                    DeviceModel B = b as DeviceModel;
                    Good = new CLabel() { IsCommon = false };
                    Good.ToolTip = string.Format(B.DeviceName);
                    Good.SetBinding(CLabel.VisibilityProperty, new Binding("SingleBool") { Source = temp._RowCellState[Row], Converter = Converter.CellVisibilityConverter2 });


                }
                else if (a is DeviceInterface && b is DeviceInterface)
                {
                    DeviceInterface A = a as DeviceInterface;
                    DeviceInterface B = b as DeviceInterface;
                    Good = new CLabel() { IsCommon = true };
                    Good.ToolTip = string.Format(A.InterfaceName + "=>" + B.InterfaceName);

                    Binding B1 = new Binding("SingleBool") { Source = temp._RowCellState[Row] };
                    Binding B2 = new Binding("SingleBool") { Source = temp._ColumnCellState[Column] };
                    MultiBinding MBinding = new MultiBinding() { Mode = BindingMode.OneWay };
                    MBinding.Bindings.Add(B1);
                    MBinding.Bindings.Add(B2);
                    MBinding.Converter = Converter.CellVisibilityConverter;
                    Good.SetBinding(CLabel.VisibilityProperty, MBinding);
                }
                if (Good == null)
                    return Good;

                    Good.Width = Good.Height = 20;
                //{
                //    Binding B1 = new Binding("ColumnState") { Source = temp._RowCellState[Row] };
                //    Binding B2 = new Binding("ColumnState") { Source = temp._ColumnCellState[Column] };

                //    MultiBinding MBinding = new MultiBinding() { Mode = BindingMode.OneWay };
                //    MBinding.Bindings.Add(B1);
                //    MBinding.Bindings.Add(B2);
                //    if (Good is CToggleBtn)
                //    {
                //        MBinding.Converter = Converter.CellStateConverter;
                //        Good.SetBinding(CToggleBtn.ChangedIconProperty, MBinding);
                //    }
                //    else
                //    {
                //        MBinding.Converter = Converter.CellVisibilityConverter;
                //        Good.SetBinding(CLabel.VisibilityProperty, MBinding);
                //    }
                //}
                if(Good is CLabel)
                {
                    Binding SelectBD1 = new Binding("IsSelect") { Source = temp._RowCellState[Row] };
                    Binding SelectBD2 = new Binding("IsSelect") { Source = temp._ColumnCellState[Column] };
                    MultiBinding SelectMB = new MultiBinding() { Mode = BindingMode.OneWay };
                    SelectMB.Bindings.Add(SelectBD1);
                    SelectMB.Bindings.Add(SelectBD2);
                    SelectMB.Converter = Converter.SelectConverter;
                    Good.SetBinding(CLabel.IsSelectedProperty, SelectMB);
                }
                return (Control)Good;
            }
        }

    }
}
