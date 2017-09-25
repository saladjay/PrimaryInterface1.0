using PrimaryInterface1._0.Core;
using PrimaryInterface1._0.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace PrimaryInterface1._0.Controls
{
    public class CellFactory
    {
        public static Control CreateCell(object a,object b,int Row,int Column)
        {
            Control Good = null;
            if(a is DeviceModel&&b is DeviceModel)
            {
                DeviceModel A = a as DeviceModel;
                DeviceModel B = b as DeviceModel;
                Good = new CToggleBtn() { };
                Good.ToolTip = string.Format(A.DeviceName + "&&" + B.DeviceName);
            }
            else if(a is DeviceModel&&b is DeviceInterface)
            {
                DeviceModel A = a as DeviceModel;
                DeviceInterface B = b as DeviceInterface;
                Good = new CLabel() { IsCommon = false };
                Good.ToolTip = string.Format(A.DeviceName);
            }
            else if(a is DeviceInterface&&b is DeviceModel)
            {
                DeviceInterface A = a as DeviceInterface;
                DeviceModel B = b as DeviceModel;
                Good = new CLabel() { IsCommon = false };
                Good.ToolTip = string.Format(B.DeviceName);
            }
            else if(a is DeviceInterface&&b is DeviceInterface)
            {
                DeviceInterface A = a as DeviceInterface;
                DeviceInterface B = b as DeviceInterface;
                Good = new CLabel() { IsCommon = true };
                Good.ToolTip = string.Format(A.InterfaceName + "=>" + B.InterfaceName);
            }
            if (Good == null)
                return Good;
            Binding B1 = new Binding("RowState") { Source = ViewModel.cellsState[Row][Column] };
            Binding B2 = new Binding("ColumnState") { Source = ViewModel.cellsState[Row][Column] };

            MultiBinding MBinding = new MultiBinding() { Mode = BindingMode.OneWay };
            MBinding.Bindings.Add(B1);
            MBinding.Bindings.Add(B2);
            if (Good is CToggleBtn)
            {
                MBinding.Converter = Converter.CellStateConverter;
                Good.SetBinding(CToggleBtn.ChangedIconProperty, MBinding);
            }
            else
            {
                MBinding.Converter = Converter.CellVisibilityConverter;
                Good.SetBinding(CLabel.VisibilityProperty, MBinding);
            }
            return (Control)Good;
        }
    }
}
