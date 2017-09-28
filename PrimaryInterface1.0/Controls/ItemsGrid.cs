using MS.Internal.PresentationFramework;
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
    ///     <MyNamespace:InterfaceGrid/>
    ///
    /// </summary>


    public class ItemsGrid : ItemsControl
    {
        public ItemsGrid()
        {

        }

        public delegate void ItemsChangedHandler(object item, bool AddOrRemove);
        public event ItemsChangedHandler ExtendedItemsChanged;

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    ExtendedItemsChanged?.Invoke(item, true);
                }
            }
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    ExtendedItemsChanged?.Invoke(item, false);
                }
            }
            base.OnItemsChanged(e);
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
        }
    }

    public class ExtendedItemsSourceComponent : DependencyObject
    {
        [CommonDependencyProperty]
        public static readonly DependencyProperty ExtendedItemsSourceProperty = DependencyProperty.Register("ExtendedItemsSource", 
            typeof(IEnumerable), typeof(ExtendedItemsSourceComponent), 
            new PropertyMetadata(((IEnumerable)null), new PropertyChangedCallback(OnExtendedItemsSourceChanged)));

        private static void OnExtendedItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExtendedItemsSourceComponent tempComponent = d as ExtendedItemsSourceComponent;
            IEnumerable oldValue = (IEnumerable)e.OldValue;
            IEnumerable newValue = (IEnumerable)e.NewValue;
            tempComponent.OnExtendedItemsSourceChanged(oldValue, newValue);
            tempComponent.ExtendedItemsSourceEvent?.Invoke(oldValue, newValue);
        }

        public delegate void ExtendedItemsSourceEventHandler(IEnumerable oldValue, IEnumerable newValue);
        public event ExtendedItemsSourceEventHandler ExtendedItemsSourceEvent;

        protected virtual void OnExtendedItemsSourceChanged(IEnumerable oldValue,IEnumerable newValue)
        {

        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ExtendedItemsSourceProperty); }
            set
            {
                if (value == null)
                {
                    ClearValue(ExtendedItemsSourceProperty);
                }
                else
                {
                    SetValue(ExtendedItemsSourceProperty, value);
                }
            }
        }

        public ExtendedItemsSourceComponent()
        {
            
        }
    }

    public class AttachedItemsSourceComponent
    {
        public delegate void AttachedItemsSourceChangedEventHandler(IEnumerable oldValue,IEnumerable newValue);
        public event AttachedItemsSourceChangedEventHandler AttachedItemsSourceChanged;
        public delegate void AttachedItemsChangedHandler(object item, bool IsAdd);
        public event AttachedItemsChangedHandler AttachedItemsChanged;

        private ObservableCollection<object> _attachedItemsSource;
        public ObservableCollection<object> AttachedItemsSource
        {
            get { return _attachedItemsSource; }
            set
            {
                AttachedItemsSourceChanged?.Invoke(_attachedItemsSource,value);
                _attachedItemsSource = value;
            }
        }

        public AttachedItemsSourceComponent()
        {
            _attachedItemsSource.CollectionChanged += _attachedItemsSource_CollectionChanged;
        }

        private void _attachedItemsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var item in e.NewItems)
            {
                AttachedItemsChanged?.Invoke(item, true);
            }
            foreach (var item in e.OldItems)
            {
                AttachedItemsChanged?.Invoke(item, false);
            }
        }
    }
}
