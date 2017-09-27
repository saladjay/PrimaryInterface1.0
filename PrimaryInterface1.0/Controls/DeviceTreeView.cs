using PrimaryInterface1._0.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PrimaryInterface1._0.Controls
{
    public class CTreeViewItem : TreeViewItem
    {
        static CTreeViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CTreeViewItem), new FrameworkPropertyMetadata(typeof(CTreeViewItem)));
        }
        #region style switch
        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register("Direction", typeof(_Direction), typeof(CTreeViewItem));

        public _Direction Direction
        {
            get { return (_Direction)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }     

        public static readonly DependencyProperty HideBtnProperty = DependencyProperty.Register("HideBtn", typeof(bool), typeof(CTreeViewItem),new PropertyMetadata(false));
        public bool HideBtn
        {
            get { return (bool)GetValue(HideBtnProperty); }
            set { SetValue(HideBtnProperty, value); }
        }
        #endregion

        #region expand and collapse
        public static readonly DependencyProperty OpenProperty = DependencyProperty.Register("Open", typeof(bool), typeof(CTreeViewItem), new PropertyMetadata(false, OnOpenChanged));

        private static void OnOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CTreeViewItem tempItem = d as CTreeViewItem;
            if (tempItem == null)
            {
                return;
            }
            tempItem.ExpandItems?.Invoke((bool)e.NewValue, tempItem);
        }

        public bool Open
        {
            get { return (bool)GetValue(OpenProperty); }
            set { SetValue(OpenProperty, value); }
        }
        #endregion

        #region select and unselect
        public static readonly DependencyProperty MouseSelectedProperty = DependencyProperty.Register("MouseSelected", typeof(bool), typeof(CTreeViewItem),
            new PropertyMetadata(false));
        public bool MouseSelected
        {
            get { return (bool)GetValue(MouseSelectedProperty); }
            set { SetValue(MouseSelectedProperty, value); }
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            SelectItem?.Invoke(true, this);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
        }
        #endregion

        public delegate void ExpandItemsHandler(bool IsOpen,CTreeViewItem Source);
        public event ExpandItemsHandler ExpandItems;
        public delegate void SelectItemHandler(bool IsSelect, CTreeViewItem Source);
        public event SelectItemHandler SelectItem;
    }

    public enum _Direction
    {
        Left,
        Top
    }


}
