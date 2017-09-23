using MS.Internal.PresentationFramework;
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
            CGridItemsSource.ExtendedItemsChanged += CGridItemsSource_ExtendedItemsChanged;
        }
        #endregion


        private List<object> ConstructionHelper = new List<object>();
        private List<CellState> RowCreateHelper = new List<Model.CellState>();
        private void CGridItemsSource_ExtendedItemsChanged(object item, bool AddOrRemove)
        {
            if (!(item is DeviceModel))
                return;
            if (AddOrRemove)
            {
                DeviceModel device = item as DeviceModel;
                InnerDeviceList.Add(device);
                ConstructionHelper.Add(device);
                foreach (DeviceInterface element in device.InterfaceList)
                {
                    ConstructionHelper.Add(element);
                }
                foreach (List<CellState> RowList in cellsState)
                {
                    for (int i = 0; i < device.InterfaceCount + 1; i++)
                    {
                        RowList.Add(new Model.CellState() { RowState = false, ColumnState = false });
                    }
                }
                for (int i = 0; i < device.InterfaceCount + 1; i++)
                {
                    RowCreateHelper.Add(new Model.CellState() { RowState = false, ColumnState = false });
                }
                for (int i = 0; i < device.InterfaceCount + 1; i++)
                {
                    cellsState.Add(new List<Model.CellState>(RowCreateHelper));
                }
                foreach (List<CellState> RowList in cellsState)
                {
                    foreach (var e in RowList)
                    {
                        Debug.Write("1--");
                    }
                    Debug.Write("\n");
                }

                for (int i = 0; i < device.InterfaceCount + 1; i++)
                {
                    this.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0, GridUnitType.Auto) });
                    this.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0, GridUnitType.Auto) });
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
                            Control CellControl = CellFactory.CreateCell(TempDev, device, RowIndex, ColumnIndex);
                            this.Children.Add(CellControl);
                            RowList.Add(CellControl);
                            Grid.SetColumn(CellControl, ColumnIndex);
                            Grid.SetRow(CellControl, RowIndex);
                        }
                        else
                        {
                            int ColumnIndex = ConstructionHelper.IndexOf(device.InterfaceList[i - 1]);
                            Control CellControl = CellFactory.CreateCell(TempDev, device.InterfaceList[i - 1], RowIndex, ColumnIndex);
                            this.Children.Add(CellControl);
                            RowList.Add(CellControl);
                            Grid.SetColumn(CellControl, ColumnIndex);
                            Grid.SetRow(CellControl, RowIndex);
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
                            Control CellControl = CellFactory.CreateCell(device, ConstructionHelper[i], CellsControl.Count, ColumnIndex);
                            tempRow.Add(CellControl);
                            this.Children.Add(CellControl);
                            Grid.SetColumn(CellControl, ColumnIndex);
                            Grid.SetRow(CellControl, CellsControl.Count);
                        }
                    }
                    else
                    {
                        for (int ColumnIndex = 0; ColumnIndex < ConstructionHelper.Count; ColumnIndex++)
                        {
                            Control CellControl = CellFactory.CreateCell(device.InterfaceList[i - 1], ConstructionHelper[i], CellsControl.Count, ColumnIndex);
                            tempRow.Add(CellControl);
                            this.Children.Add(CellControl);
                            Grid.SetColumn(CellControl, ColumnIndex);
                            Grid.SetRow(CellControl, CellsControl.Count);
                        }
                    }
                }
            }
            else
            {
                DeviceModel device = item as DeviceModel;
                int Index = InnerDeviceList.IndexOf(device);
                int RemoveIndex = ConstructionHelper.IndexOf(device);
                for (int i = 0; i < device.InterfaceCount+1; i++)
                {
                    cellsState.RemoveAt(RemoveIndex);
                }
                foreach (List<CellState> RowList in cellsState)
                {
                    for (int i = 0; i < device.InterfaceCount+1; i++)
                    {
                        RowList.RemoveAt(RemoveIndex);
                    }
                }
                for (int i = 0; i < device.InterfaceCount+1; i++)
                {
                    ConstructionHelper.RemoveAt(RemoveIndex);
                    RowCreateHelper.RemoveAt(RemoveIndex);
                }
                InnerDeviceList.Remove((DeviceModel)item);
                foreach (List<CellState> RowList in cellsState)
                {
                    foreach (var e in RowList)
                    {
                        Debug.Write("1--");
                    }
                    Debug.Write("\n");
                }
            }
        }
        private List<List<Control>> CellsControl = new List<List<Control>>();

        private List<List<CellState>> cellsState = new List<List<CellState>>();
        public List<List<CellState>> CellState
        {
            get { return cellsState; }
        }
    }






}
