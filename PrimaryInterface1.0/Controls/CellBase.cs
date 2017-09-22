using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace PrimaryInterface1._0.Controls
{
    class CellBase
    {

    }

    public class CToggleBtn : ToggleButton
    {
        public delegate void CheckedAndExpandedHandler(int Row, int Column, bool IsRowExpanded, bool IsColumnExpanded);
        public event CheckedAndExpandedHandler CheckedAndExpanded;

        public static readonly DependencyProperty ChangedIconProperty = DependencyProperty.Register("ChangedIcon", typeof(bool), typeof(CToggleBtn), new PropertyMetadata(false, OnIconChanged));

        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CToggleBtn temp = d as CToggleBtn;
        }

        public bool ChangedIcon
        {
            get { return (bool)GetValue(ChangedIconProperty); }
            set { SetValue(ChangedIconProperty, value); }
        }

        protected override void OnClick()
        {
            ChangedIcon = !ChangedIcon;
            RowExpanded = ColumnExpanded = ChangedIcon;
            Debug.WriteLine("++++++++++++++++RowIndex is " + RowIndex + " ColumnIndex " + ColumnIndex + " IsExpanded " + ChangedIcon);
            if (CheckedAndExpanded == null)
                Debug.WriteLine("CheckedAndExpanded event is not subscribed");
            else
                CheckedAndExpanded?.Invoke(RowIndex, ColumnIndex, RowExpanded, ColumnExpanded);
            base.OnClick();
        }

        public int PositionRow { get; set; }
        public int PositionColumn { get; set; }

        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }

        private bool rowexpanded;
        public bool RowExpanded
        {
            get { return rowexpanded; }
            set
            {
                rowexpanded = value;
                if (rowexpanded && columnexpanded)
                    ChangedIcon = true;
                else
                    ChangedIcon = false;
            }
        }
        private bool columnexpanded;
        public bool ColumnExpanded
        {
            get { return columnexpanded; }
            set
            {
                columnexpanded = value;
                if (rowexpanded && columnexpanded)
                    ChangedIcon = true;
                else
                    ChangedIcon = false;
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            this.Style = (Style)FindResource("CBtnStyle1");
            //IsEnabled = !(RowIndex == ColumnIndex);
            base.OnInitialized(e);
        }
    }

    public class CLabel : Control
    {
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.RegisterAttached("IsSelected", typeof(bool), typeof(CLabel));
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsCommonProperty = DependencyProperty.RegisterAttached("IsCommon", typeof(bool), typeof(CLabel));
        public bool IsCommon
        {
            get { return (bool)GetValue(IsCommonProperty); }
            set { SetValue(IsCommonProperty, value); }
        }

        public delegate void IsMouseOverHandler(bool IsMouseSelect, object Itself);
        public event IsMouseOverHandler IsMouseSelect;

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            Debug.WriteLine("source" + PositionRow + " " + PositionColumn);
            Debug.WriteLine("source2" + Grid.GetRow(this) + " " + Grid.GetColumn(this));
            IsMouseSelect?.Invoke(true, this);
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            IsMouseSelect?.Invoke(false, this);
            base.OnMouseLeave(e);
        }

        public int PositionRow { get; set; }
        public int PositionColumn { get; set; }

        protected override void OnInitialized(EventArgs e)
        {
            Style = (Style)FindResource("ExpandedLabelStyle");
            IsHitTestVisible = true;
            base.OnInitialized(e);
        }
    }

    public struct NewAddRanks
    {
        public int SelfIndex;
        public int RanksCount;
        public int RanksIndex;
        public bool IsFirstLevel;
        public bool IsOrigin;
    }

    public struct ExpandedIndex
    {
        public bool Expanded;
        public int Index;
    }

    public struct ListState
    {
        public int index;
        public bool state;
    }

}
