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
    public class DeviceInterfaceGrid : Grid
    {
        static DeviceInterfaceGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DeviceInterfaceGrid), new FrameworkPropertyMetadata(typeof(DeviceInterfaceGrid)));
        }

        private ItemsGrid CGridItemsSource = new ItemsGrid();
        [CommonDependencyProperty]
        public IEnumerable ItemsSource
        {
            get { return CGridItemsSource.ItemsSource; }
            set { CGridItemsSource.ItemsSource = value; }
        }

        #region Construction
        public DeviceInterfaceGrid()
        {
            //this.Children.Add(new Border() { HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch ,BorderBrush= new SolidColorBrush(Color.FromRgb(0,0,0))});
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
                ColumnCellState = _DataSource.ColumnCellState;
                RowCellState = _DataSource.RowCellState;
                ConstructionHelper = _DataSource.ConstructionHelper;
                PositionHelper = _DataSource.PositionHelper;
                ItemsSource = DataSource.DataCollection;
            }
        }
        private List<CellState> ColumnCellState { get; set; }
        private List<CellState> RowCellState { get; set; }
        private List<object> ConstructionHelper { get; set; }
        private List<int> PositionHelper { get; set; }
        public List<DeviceModel> InnerDeviceList = new List<DeviceModel>();
        private void CGridItemsSource_ExtendedItemsChanged(object item, bool AddOrRemove)
        {
            if (!(item is DeviceModel))
                return;
            DeviceModel device = item as DeviceModel;
            if (AddOrRemove)
            {
                InnerDeviceList.Add(device);

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
                            //Debug.WriteLine(((CLabel)CellControl).ToolTip.ToString() + " " + CellControl.Visibility);
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
                            //Debug.WriteLine(((CLabel)CellControl).ToolTip.ToString() + " " + CellControl.Visibility);
                            Grid.SetColumn(CellControl, PositionHelper[ColumnIndex]);
                            Grid.SetRow(CellControl, RowDefinitions.Count - device.InterfaceCount - 1 + i);
                        }
                    }
                    CellsControl.Add(tempRow);
                }
            }
            else
            {
                int Index = InnerDeviceList.IndexOf(device);
                int RemoveIndex = DataSource.RemoveIndex;//ConstructionHelper.IndexOf(device);

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
            }
        }
        private List<List<Control>> CellsControl = new List<List<Control>>();

        internal void CellBtnExpand(bool IsOpen,CToggleBtn Source)
        {
            int RowIndex = ConstructionHelper.IndexOf(Source.ObjectTag1);
            int ColumnIndex = ConstructionHelper.IndexOf(Source.OBjectTag2);
            for (int i = 0; i < ((DeviceModel)Source.ObjectTag1).InterfaceCount+1; i++)
            {
                RowCellState[RowIndex + i].SingleBool = IsOpen;
            }
            for (int i = 0; i < ((DeviceModel)Source.OBjectTag2).InterfaceCount+1; i++)
            {
                ColumnCellState[ColumnIndex + i].SingleBool = IsOpen;
            }
        }

        internal void CellSelcect(bool IsSelect,Control Source)
        {
            int RowIndex = 0;
            int ColumnIndex = 0;
            if (Source is CToggleBtn)
            {
                RowIndex = ConstructionHelper.IndexOf(((CToggleBtn)Source).ObjectTag1);
                ColumnIndex = ConstructionHelper.IndexOf(((CToggleBtn)Source).OBjectTag2);
            }
            else
            {
                RowIndex = ConstructionHelper.IndexOf(((CLabel)Source).ObjectTag1);
                ColumnIndex = ConstructionHelper.IndexOf(((CLabel)Source).OBjectTag2);
            }
            DataSource.SelectColumn = ColumnIndex;
            DataSource.SelectRow = RowIndex;
        }

        public class CellFactory
        {
            public static Control CreateCell(object a, object b, int Row, int Column ,DeviceInterfaceGrid temp)
            {
                Control Good = null;
                if (a is DeviceModel && b is DeviceModel)
                {
                    DeviceModel A = a as DeviceModel;
                    DeviceModel B = b as DeviceModel;
                    Good = new CToggleBtn() { ObjectTag1 = A, OBjectTag2 = B };
                    Good.ToolTip = string.Format(A.DeviceName + "&&" + B.DeviceName);
                    Binding B1 = new Binding("SingleBool") { Source = temp.RowCellState[Row] };
                    Binding B2 = new Binding("SingleBool") { Source = temp.ColumnCellState[Column] };

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
                    Good = new CLabel() { IsCommon = false, ObjectTag1 = A, OBjectTag2 = B };
                    Good.ToolTip = string.Format(A.DeviceName);
                    Good.SetBinding(CLabel.VisibilityProperty, new Binding("SingleBool") { Source = temp.ColumnCellState[Column], Converter = Converter.CellVisibilityConverter2 });
                }
                else if (a is DeviceInterface && b is DeviceModel)
                {
                    DeviceInterface A = a as DeviceInterface;
                    DeviceModel B = b as DeviceModel;
                    Good = new CLabel() { IsCommon = false, ObjectTag1 = A, OBjectTag2 = B };
                    Good.ToolTip = string.Format(B.DeviceName);
                    Good.SetBinding(CLabel.VisibilityProperty, new Binding("SingleBool") { Source = temp.RowCellState[Row], Converter = Converter.CellVisibilityConverter2 });


                }
                else if (a is DeviceInterface && b is DeviceInterface)
                {
                    DeviceInterface A = a as DeviceInterface;
                    DeviceInterface B = b as DeviceInterface;
                    Good = new CLabel() { IsCommon = true, ObjectTag1 = A, OBjectTag2 = B };
                    Good.ToolTip = string.Format(A.InterfaceName + "=>" + B.InterfaceName);

                    Binding B1 = new Binding("SingleBool") { Source = temp.RowCellState[Row] };
                    Binding B2 = new Binding("SingleBool") { Source = temp.ColumnCellState[Column] };
                    MultiBinding MBinding = new MultiBinding() { Mode = BindingMode.OneWay };
                    MBinding.Bindings.Add(B1);
                    MBinding.Bindings.Add(B2);
                    MBinding.Converter = Converter.CellVisibilityConverter;
                    Good.SetBinding(CLabel.VisibilityProperty, MBinding);
                }
                if (Good == null)
                    return Good;
                Good.Width = Good.Height = 20;
                if(Good is CToggleBtn)
                {
                    ((CToggleBtn)Good).ExpandCell += temp.CellBtnExpand;
                    ((CToggleBtn)Good).IsMouseSelect += temp.CellSelcect;
                }
                else if (Good is CLabel)
                {
                    Binding SelectBD1 = new Binding("IsSelect") { Source = temp.RowCellState[Row] };
                    Binding SelectBD2 = new Binding("IsSelect") { Source = temp.ColumnCellState[Column] };
                    MultiBinding SelectMB = new MultiBinding() { Mode = BindingMode.OneWay };
                    SelectMB.Bindings.Add(SelectBD1);
                    SelectMB.Bindings.Add(SelectBD2);
                    SelectMB.Converter = Converter.SelectConverter;
                    Good.SetBinding(CLabel.IsSelectedProperty, SelectMB);

                    ((CLabel)Good).IsMouseSelect += temp.CellSelcect;
                }
                return (Control)Good;
            }
        }

    }
}
