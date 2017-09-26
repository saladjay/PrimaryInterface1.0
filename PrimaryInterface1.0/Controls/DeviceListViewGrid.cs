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
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:PrimaryInterface1._0.Controls"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:PrimaryInterface1._0.Controls;assembly=PrimaryInterface1._0.Controls"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误: 
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:DeviceListViewGrid/>
    ///
    /// </summary>
    public class DeviceListViewGrid : Grid
    {
        static DeviceListViewGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DeviceListViewGrid), new FrameworkPropertyMetadata(typeof(DeviceListViewGrid)));
        }

        private List<DeviceModel> InnerDeviceList = new List<DeviceModel>();
        private ItemsGrid DeviceListItemsSource = new ItemsGrid();
        [CommonDependencyProperty]
        public IEnumerable ItemsSource
        {
            get { return DeviceListItemsSource.ItemsSource; }
            set { DeviceListItemsSource.ItemsSource = value; }
        }
        #region Construction
        public DeviceListViewGrid()
        {
            DeviceListItemsSource.ExtendedItemsChanged += DeviceListItemsSource_ExtendedItemsChanged;
        }
        #endregion

        private List<object> ConstructionHelper = new List<object>();
        private List<CellState> RowCreateHelper = new List<Model.CellState>();
        private List<int> PositionHelper = new List<int>();
        private int PositionHelperIndex = 0;
        private void DeviceListItemsSource_ExtendedItemsChanged(object item, bool AddOrRemove)
        {
            if (!(item is DeviceModel))
            {
                return;
            }
            DeviceModel device = item as DeviceModel;
            if(AddOrRemove)
            {
                InnerDeviceList.Add(device);
                ConstructionHelper.Add(device);
                foreach (DeviceInterface element in device.InterfaceList)
                {
                    ConstructionHelper.Add(element);
                }
            }
            else
            {

            }
        }
    }
}
