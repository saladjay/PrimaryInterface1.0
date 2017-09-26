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
    class DeviceTreeView
    {
    }

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

        public static readonly DependencyProperty OpenProperty = DependencyProperty.Register("Open", typeof(bool), typeof(CTreeViewItem));
        public bool Open
        {
            get { return (bool)GetValue(OpenProperty); }
            set { SetValue(OpenProperty, value); }
        }
        #endregion

        #region expand and collapse


        protected override void OnCollapsed(RoutedEventArgs e)
        {
            foreach (var Element in LogicalTreeHelper.GetChildren(this))
            {
                if (Element is TreeViewItem && ((CTreeViewItem)Element).IsExpanded)
                    ((TreeViewItem)Element).IsExpanded = false;
            }
            //Debug.WriteLine("CalRow's result is " + tempNum);
            if (ExpandedRowIndex == null)
                Debug.WriteLine("ExpandedRowIndex event is not subscribed");
            else
                ExpandedRowIndex(SelfIndex, TotalIndex, -CTreeViewItemCount, IsFirstLevel, IsMouseOver);
            //Debug.WriteLine("SelfIndex + tempNum :" + (SelfIndex + tempNum) + "CTreeViewItem2Count" + CTreeViewItem2Count);
            base.OnCollapsed(e);

            if (RowNumChanged == null)
                Debug.WriteLine("RowNumChanged event is not subsribed");
            else
                RowNumChanged?.Invoke(-CTreeViewItemCount);
        }

        protected override void OnExpanded(RoutedEventArgs e)
        {
            if (SelfIndex == 0 && Parent != null)
            {
                foreach (var Element in LogicalTreeHelper.GetChildren(Parent))
                {
                    if (Element is CTreeViewItem)
                        SelfIndex++;
                    if (Element.Equals(this))
                        break;
                }
                TotalIndex = SelfIndex;
            }
            foreach (var item in LogicalTreeHelper.GetChildren(this))
            {
                if (item is CTreeViewItem)
                {
                    CTreeViewItem TempElement = item as CTreeViewItem;
                    TempElement.TotalIndex = TotalIndex + TempElement.SelfIndex;
                }
            }
            if (ExpandedRowIndex == null)
                Debug.WriteLine("ExpandedRowIndex event is not subscribed");
            else
                ExpandedRowIndex?.Invoke(SelfIndex, TotalIndex, CTreeViewItemCount, IsFirstLevel, IsMouseOver);
            //Debug.WriteLine("SelfIndex + tempNum :" + (SelfIndex + tempNum) + "CTreeViewItem2Count" + CTreeViewItem2Count);
            base.OnExpanded(e);

            if (RowNumChanged == null)
                Debug.WriteLine("RowNumChanged event is not subsribed");
            else
                RowNumChanged?.Invoke(CTreeViewItemCount);
        }

        private void CTreeViewItem_ExpandedRowIndex(int selfIndex, int Index, int num, bool Level, bool origin)
        {
            if (SelectedRowIndex == null)
                Debug.WriteLine("ExpandedRowIndex event is not subscribed");
            else
                ExpandedRowIndex?.Invoke(selfIndex, Index, num, Level, origin);
            //Debug.WriteLine("SelfIndex + tempNum :" + (Index + SelfIndex) + "CTreeViewItem2Count" + num);
        }

        #endregion

        #region select and unselect
        public static readonly DependencyProperty MoveDistanceProperty = DependencyProperty.Register("MoveDistance", typeof(int), typeof(CTreeViewItem),
            new PropertyMetadata(0, OnMoveDistanceChanged));

        private static void OnMoveDistanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((int)e.NewValue == 0)
                return;
            CTreeViewItem TempItem = d as CTreeViewItem;
            //Debug.WriteLine((string)TempItem.Header + (int)e.NewValue);
            TempItem.TotalIndex += (int)e.NewValue;
            if (TempItem.IsExpanded)
                foreach (var item in LogicalTreeHelper.GetChildren(TempItem))
                {
                    if (item is CTreeViewItem)
                        ((CTreeViewItem)item).MoveDistance = (int)e.NewValue;
                }
            TempItem.MoveDistance = 0;
        }

        public int MoveDistance
        {
            get { return (int)GetValue(MoveDistanceProperty); }
            set { SetValue(MoveDistanceProperty, value); }
        }

        public static readonly DependencyProperty MouseSelectedProperty = DependencyProperty.Register("MouseSelected", typeof(bool), typeof(CTreeViewItem),
            new PropertyMetadata(false));
        public bool MouseSelected
        {
            get { return (bool)GetValue(MouseSelectedProperty); }
            set { SetValue(MouseSelectedProperty, value); }
        }

        public bool IsMouseOverChild { get; set; }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            Debug.WriteLine("Mouse enter its header is " + this.Header);
            if (IsMouseOverChild)
                return;
            base.OnMouseEnter(e);
            if (Parent is CTreeViewItem)
            {
                ((CTreeViewItem)Parent).MouseSelected = false;
                ((CTreeViewItem)Parent).IsMouseOverChild = true;
            }
            if (SelectedRowIndex == null)
                Debug.WriteLine("SelectedRowIndex original event is not subscribed");
            else
                SelectedRowIndex?.Invoke(TotalIndex, this, true);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            //MouseSelected = false;
            Debug.WriteLine("Mouse leave its header is " + this.Header);

            if (Parent is CTreeViewItem)
            {
                CTreeViewItem TempParent = Parent as CTreeViewItem;
                TempParent.IsMouseOverChild = false;
                if (TempParent.IsMouseOver)
                    SelectedRowIndex?.Invoke(TempParent.TotalIndex, TempParent, true);
            }
            base.OnMouseLeave(e);
            if (SelectedRowIndex == null)
                Debug.WriteLine("SelectedRowIndex original event is not subscribed");
            else
                SelectedRowIndex?.Invoke(TotalIndex, this, false);
        }

        private void CTreeViewItem_SelectedRowIndex(int Index, object item, bool IsMouseSelect)
        {
            if (SelectedRowIndex == null)
                Debug.WriteLine("SelectedRowIndex event is not subscribed");
            else
                SelectedRowIndex?.Invoke(Index, item, IsMouseSelect);
        }

        protected override void OnSelected(RoutedEventArgs e)
        {
            return;
            if (SelfIndex == 0 && Parent != null)
            {
                foreach (var Element in LogicalTreeHelper.GetChildren(Parent))
                {
                    if (Element is CTreeViewItem)
                        SelfIndex++;
                    if (Element.Equals(this))
                        break;
                }
                TotalIndex = SelfIndex;
            }
            if (SelectedRowIndex == null)
                Debug.WriteLine("SelectedRowIndex original event is not subscribed");
            else
                SelectedRowIndex?.Invoke(TotalIndex, this, IsMouseOver);
            base.OnSelected(e);
        }

        protected override void OnUnselected(RoutedEventArgs e)
        {
            base.OnUnselected(e);
        }

        public CTreeViewItem FindSelectRow(int Index)
        {
            foreach (var item in LogicalTreeHelper.GetChildren(this))
            {
                if (item is CTreeViewItem)
                {
                    CTreeViewItem TempItem = item as CTreeViewItem;
                    if (TempItem.TotalIndex < Index)
                        continue;
                    if (TempItem.TotalIndex == Index)
                        return TempItem;
                    if (TempItem.TotalIndex > Index)
                        return null;
                }
            }
            return null;
        }
        #endregion

        private bool OriginalSource { get; set; }
        private int CTreeViewItemCount { get; set; }
        public bool IsFirstLevel { get; set; }
        public int SelfIndex { get; set; }
        public int TotalIndex { get; set; }

        public delegate void NumChangedHandler1(int num);
        public event NumChangedHandler1 RowNumChanged;
        public delegate void NumChangeHandler3(int Index, object itself, bool IsMouseSelect);
        public event NumChangeHandler3 SelectedRowIndex;
        public delegate void NumChangedHandler2(int SelfIndex, int Index, int Num, bool IsFirstLevel, bool origin);
        public event NumChangedHandler2 ExpandedRowIndex;

        protected override void OnInitialized(EventArgs e)
        {
            IsFirstLevel = false;
            Style = (Style)FindResource("CTreeViewItemStyle1");
            if (Parent != null)
            {
                foreach (var Element in LogicalTreeHelper.GetChildren(Parent))
                {
                    if (Element is CTreeViewItem)
                        SelfIndex++;
                }
                TotalIndex = SelfIndex;
            }
            foreach (var Element in LogicalTreeHelper.GetChildren(this))
            {
                if (Element is CTreeViewItem)
                {
                    CTreeViewItemCount++;
                    ((CTreeViewItem)Element).SelectedRowIndex += CTreeViewItem_SelectedRowIndex;
                    ((CTreeViewItem)Element).ExpandedRowIndex += CTreeViewItem_ExpandedRowIndex;
                }
            }
            base.OnInitialized(e);
        }

        protected override void AddChild(object value)
        {
            base.AddChild(value);
            if (value is CTreeViewItem)
            {
                CTreeViewItemCount++;
                CTreeViewItem temp = value as CTreeViewItem;
                temp.SelectedRowIndex += CTreeViewItem_SelectedRowIndex;
                temp.ExpandedRowIndex += CTreeViewItem_ExpandedRowIndex;
                if (temp.SelfIndex == 0)
                {
                    int Index = 0;
                    foreach (var e in LogicalTreeHelper.GetChildren(this))
                    {
                        if (e is CTreeViewItem)
                            Index++;
                        if (temp.Equals(e))
                            temp.SelfIndex = Index;
                    }
                    temp.TotalIndex = temp.SelfIndex;
                    Debug.WriteLine("initial selfindex" + temp.SelfIndex);
                }
            }
        }

        public void AddCTreeViewItem(object value)
        {
            AddChild(value);
        }
    }

    public class CTreeView : TreeView
    {
        static CTreeView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CTreeView), new FrameworkPropertyMetadata(typeof(CTreeView)));
        }

        private int CurrentSelectedRow = 0;
        private List<CTreeViewItem> FirstLevelItem = new List<CTreeViewItem>();

        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register("Direction", typeof(_Direction), typeof(CTreeView), new PropertyMetadata(_Direction.Left));
        public _Direction Direction
        {
            get { return (_Direction)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        public static readonly DependencyProperty TotalItemsCountProperty = DependencyProperty.Register("TotalItemsCount", typeof(int), typeof(CTreeView));
        public int TotalItemsCount
        {
            get { return (int)GetValue(TotalItemsCountProperty); }
            set { SetValue(TotalItemsCountProperty, value); }
        }

        public static readonly DependencyProperty SelectRowIndexProperty = DependencyProperty.Register("SelectRowIndex", typeof(int), typeof(CTreeView), new PropertyMetadata(0, OnSelectRowIndexChanged));
        public int SelectRowIndex
        {
            get { return (int)GetValue(SelectRowIndexProperty); }
            set { SetValue(SelectRowIndexProperty, value); }
        }


        private static void OnSelectRowIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((int)e.NewValue <= 0)
            {
                CTreeView TempTree = d as CTreeView;
                if (TempTree == null)
                    return;
                CTreeViewItem TempItem = TempTree.FindSelectRow((int)e.OldValue);
                if (TempItem != null)
                    TempItem.MouseSelected = false;
                return;
            }
            else
            {
                CTreeView TempTree = d as CTreeView;
                if (TempTree == null)
                    return;
                Debug.WriteLine("selected index is " + (int)e.NewValue + " and its header is " + TempTree.FindSelectRow((int)e.NewValue)?.Header);
                CTreeViewItem TempItem = TempTree.FindSelectRow((int)e.NewValue);
                if (TempItem != null)
                    TempItem.MouseSelected = true;
                TempItem = TempTree.FindSelectRow((int)e.OldValue);
                if (TempItem != null)
                    TempItem.MouseSelected = false;
            }
        }

        private CTreeViewItem FindSelectRow(int Index)
        {
            if (Index == 0)
                return null;
            CTreeViewItem LastItem = new CTreeViewItem();
            foreach (var item in LogicalTreeHelper.GetChildren(this))
            {
                if (item is CTreeViewItem)
                {
                    CTreeViewItem TempItem = item as CTreeViewItem;
                    if (TempItem.TotalIndex < Index)
                    {
                        LastItem = TempItem;
                        continue;
                    }
                    if (TempItem.TotalIndex == Index)
                        return TempItem;
                    if (TempItem.TotalIndex > Index)
                        return LastItem.FindSelectRow(Index);
                }
            }
            return LastItem.FindSelectRow(Index);
        }

        public static readonly DependencyProperty AddNewRanksProperty = DependencyProperty.Register("AddNewRanks", typeof(NewAddRanks), typeof(CTreeView));
        private NewAddRanks AddNewRanks
        {
            get { return (NewAddRanks)GetValue(AddNewRanksProperty); }
            set { SetValue(AddNewRanksProperty, value); }
        }

        public static readonly DependencyProperty ExpandedIndexProperty = DependencyProperty.Register("ExpandedIndex", typeof(ExpandedIndex), typeof(CTreeView), new PropertyMetadata(new ExpandedIndex() { Expanded = false, Index = -1 }, OnExpandedIndexChanged));

        private static void OnExpandedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CTreeView tempTreeView = d as CTreeView;
            ExpandedIndex NewExpandedIndex = (ExpandedIndex)e.NewValue;
            Debug.WriteLine("index is " + NewExpandedIndex.Index + " expanded is " + NewExpandedIndex.Expanded);
            tempTreeView.FirstLevelItem[NewExpandedIndex.Index].IsExpanded = NewExpandedIndex.Expanded;
        }

        public int ExpandedIndex
        {
            get { return (int)GetValue(ExpandedIndexProperty); }
            set { SetValue(ExpandedIndexProperty, value); }
        }

        private List<bool> FirstLevelItemState = null;

        public static readonly DependencyProperty ItemsStateProperty = DependencyProperty.Register("ItemsState", typeof(ListState), typeof(CTreeView), new PropertyMetadata(new ListState(), OnItemsStateChanged));

        private static void OnItemsStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CTreeView tempTree = d as CTreeView;
            if (tempTree.FirstLevelItemState == null)
                return;
            for (int i = 0; i < tempTree.FirstLevelItemState.Count; i++)
            {
                Debug.Write(tempTree.FirstLevelItemState[i].ToString() + "-");
            }
            Debug.Write("\n");
        }

        public ListState ItemsState
        {
            get { return (ListState)GetValue(ItemsStateProperty); }
            set { SetValue(ItemsStateProperty, value); }
        }

        private void InitialAddEvent(DependencyObject a)
        {
            foreach (var Element in LogicalTreeHelper.GetChildren(a))
            {
                if (Element is CTreeViewItem)
                {
                    CTreeViewItem temp = Element as CTreeViewItem;
                    if (a is TreeView)
                    {

                        temp.IsFirstLevel = true;
                        FirstLevelItem.Add(temp);
                        temp.SelectedRowIndex += Temp_SelectedRowIndex;
                        temp.ExpandedRowIndex += Temp_ExpandedRowIndex;
                    }
                    if (temp.HasItems)
                    {
                        temp.RowNumChanged += Temp_RowNumChanged;
                        InitialAddEvent(temp);
                    }
                    else
                        continue;
                }
            }
        }

        public delegate void ItemsStateChangedHandler(ItemInfo Source);
        public event ItemsStateChangedHandler ItemsStateChanged;
        private void Temp_ExpandedRowIndex(int selfIndex, int Index, int num, bool Level, bool origin)
        {
            //new notify mechanism 
            if (ItemsStateChanged == null)
                Debug.WriteLine("Nobody subscribes ItemsStateChanged");
            ItemsStateChanged?.Invoke(new ItemInfo() { Position = selfIndex, IsExpanded = num > 0 });
            //old notify mechanism
            AddNewRanks = new NewAddRanks() { SelfIndex = selfIndex, RanksCount = num, RanksIndex = Index, IsFirstLevel = Level, IsOrigin = origin };

            if (Index < CurrentSelectedRow)
            {
                CurrentSelectedRow += num;
                SelectRowIndex = Index;
                Debug.WriteLine("in top index is " + Index + "num is " + num);
            }
            Debug.WriteLine("New ranks index is " + Index + "count is " + num + Level);

            if (Level)
            {
                FirstLevelItemState[selfIndex - 1] = num > 0;
                ItemsState = new ListState() { index = selfIndex - 1, state = num > 0 };
                Debug.WriteLine("state is " + FirstLevelItemState[selfIndex - 1]);
            }
            foreach (var item in LogicalTreeHelper.GetChildren(this))
            {
                if (!(item is CTreeViewItem))
                    continue;
                CTreeViewItem e = item as CTreeViewItem;
                if (e.TotalIndex > Index)
                {
                    e.MoveDistance = num;
                }
            }
        }

        public delegate void ItemsSelectedChangedHandler(int Index, object item);
        public event ItemsSelectedChangedHandler ItemsSelectedChanged;
        private void Temp_SelectedRowIndex(int Index, object item, bool IsMouseSelect)
        {
            if (IsMouseSelect)
            {
                if (ItemsSelectedChanged == null)
                    Debug.WriteLine("Nobody subscribed the event ItemsSelectedChanged");
                ItemsSelectedChanged?.Invoke(Index, item);
                SelectRowIndex = Index;
                CurrentSelectedRow = Index;
            }
        }

        private void Temp_RowNumChanged(int Delta)
        {
            TotalItemsCount += Delta;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            InitialAddEvent(this);
            Style = (Style)FindResource("CTreeViewStyle");

            if (FirstLevelItem != null && FirstLevelItem.Count != 0)
            {
                //ItemsState = new List<bool>(FirstLevelItem.Count);
                FirstLevelItemState = new List<bool>(FirstLevelItem.Count);
            }
            else
            {
                //ItemsState = new List<bool>();
                FirstLevelItemState = new List<bool>();
            }
        }

        public int RemoveCTreeViewItem(DeviceModel RemoveDeviceModel)
        {
            int Result = -1;
            CTreeViewItem temp = null;
            foreach (var item in Items)
            {
                if (item is CTreeViewItem && (DeviceModel)((CTreeViewItem)item).Tag == RemoveDeviceModel)
                {
                    temp = (CTreeViewItem)item;
                    Result = Items.IndexOf(temp);
                    break;
                }
            }
            if (Result == -1)
                return Result;
            foreach (CTreeViewItem item in FirstLevelItem)
            {
                if (item.TotalIndex > temp.TotalIndex)
                {
                    item.TotalIndex -= 1;
                    item.SelfIndex -= 1;
                }
            }
            this.FirstLevelItem.Remove(temp);
            FirstLevelItemState.RemoveAt(Result);
            Items.Remove(temp);
            return Result;
        }

        protected override void AddChild(object value)
        {
            base.AddChild(value);
            Debug.WriteLine("Add Child TreeView");
            if (value is CTreeViewItem)
            {
                InitialChildEventAndInfo((CTreeViewItem)value);
            }
        }

        public void AddCTreeViewItem(object value)
        {
            AddChild(value);
        }

        private void InitialChildEventAndInfo(object value)
        {
            if (!(value is CTreeViewItem))
                return;
            CTreeViewItem temp = value as CTreeViewItem;
            temp.IsFirstLevel = true;
            FirstLevelItem.Add(temp);
            //ItemsState.Add(false);
            FirstLevelItemState.Add(false);
            foreach (var item in LogicalTreeHelper.GetChildren(this))
            {
                if (!(item is CTreeViewItem))
                    continue;
                CTreeViewItem e = item as CTreeViewItem;
                temp.SelfIndex++;
                if (e.Equals(temp))
                    break;
            }
            temp.TotalIndex = temp.SelfIndex;
            //Debug.WriteLine("initial CTreeViewItem2 " + (string)temp.Header + temp.SelfIndex);
            temp.SelectedRowIndex += Temp_SelectedRowIndex;
            temp.ExpandedRowIndex += Temp_ExpandedRowIndex;
            SubscribeRowNumChangeEvent(temp);
        }

        private void SubscribeRowNumChangeEvent(CTreeViewItem e)
        {
            if (e.HasItems)
            {
                e.RowNumChanged += Temp_RowNumChanged;
                InitialAddEvent(e);
            }
        }
    }


    public struct ItemInfo
    {
        public bool IsExpanded;
        public int Position;
    }
    public enum _Direction
    {
        Left,
        Top
    }
}
