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
        public static readonly DependencyProperty ChangedIconProperty = DependencyProperty.Register("ChangedIcon", typeof(bool), typeof(CToggleBtn), new PropertyMetadata(false));

        public bool ChangedIcon
        {
            get { return (bool)GetValue(ChangedIconProperty); }
            set { SetValue(ChangedIconProperty, value); }
        }

        public object ObjectTag1 { get; set; }
        public object OBjectTag2 { get; set; }


        public delegate void ExpandCellHandler(bool Expand, CToggleBtn Source);
        public event ExpandCellHandler ExpandCell;
        private bool IsOpen = false;
        protected override void OnClick()
        {
            IsOpen = !IsOpen;
            ExpandCell?.Invoke(IsOpen, this);
            base.OnClick();
        }

        public delegate void IsMouseOverHandler(bool IsMouseSelect, CToggleBtn Source);
        public event IsMouseOverHandler IsMouseSelect;
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            IsMouseSelect?.Invoke(true, this);
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            IsMouseSelect?.Invoke(false, this);
            base.OnMouseLeave(e);
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

        public delegate void IsMouseOverHandler(bool IsMouseSelect, CLabel Source);
        public event IsMouseOverHandler IsMouseSelect;

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            //Debug.WriteLine("visibility is " + this.Visibility);
            //Debug.WriteLine("source" + PositionRow + " " + PositionColumn);
            //Debug.WriteLine("source2" + Grid.GetRow(this) + " " + Grid.GetColumn(this));
            IsMouseSelect?.Invoke(true, this);
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            IsMouseSelect?.Invoke(false, this);
            base.OnMouseLeave(e);
        }

        public object ObjectTag1 { get; set; }
        public object OBjectTag2 { get; set; }


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
