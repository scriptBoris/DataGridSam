using DataGridSam.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam.Elements
{
    /// <summary>
    /// RepeatableStack
    /// </summary>
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    internal class StackList : Layout<GridRow>
    {
        private bool? lastHasItems = null;
        
        internal double StackHeight = -1;
        internal int ItemsCount;
        internal DataGrid DataGrid;

        public StackList(DataGrid host)
        {
            VerticalOptions = LayoutOptions.FillAndExpand;
            DataGrid = host;
        }

        internal void UpdateSource(object oldValue, object newValue)
        {
            ItemsCount = 0;
            Children.Clear();

            IEnumerable newList;
            try
            {
                newList = newValue as IEnumerable;
            }
            catch (Exception e)
            {
                TryOnEmptyItems();
                throw e;
            }

            if (oldValue is INotifyCollectionChanged oldObservableCollection)
                oldObservableCollection.CollectionChanged -= OnItemsSourceCollectionChanged;

            if (newValue is INotifyCollectionChanged newObservableCollection)
                newObservableCollection.CollectionChanged += OnItemsSourceCollectionChanged;

            if (newList != null)
            {
                // Say triggers what binding context changed
                var targetType = newList.GetType().GetGenericArguments()[0];
                DataGrid.OnChangeItemsBindingContext(targetType);

                // Instantly add items
                int i = 0;
                object item = null;
                var enumerator = newList.GetEnumerator();
                if (enumerator.MoveNext())
                {
                    while (true)
                    {
                        item = enumerator.Current;

                        if (enumerator.MoveNext())
                        {
                            AddRow(item, i, true, false);
                            i++;
                        }
                        else
                        {
                            AddRow(item, i, false, false);
                            break;
                        }
                    }
                }

                // Update auto number
                if (DataGrid.AutoNumberStrategy != AutoNumberStrategyType.None)
                    foreach (var row in Children)
                        row.UpdateAutoNumeric(row.Index);
            }

            TryOnEmptyItems();
        }

        internal void Redraw(bool isNeedMeasure = true, bool isNeedLayout = true)
        {
            foreach (var item in Children)
                item.CallInvalidateMeasure();

            if (isNeedMeasure)
                InvalidateMeasure();

            if (isNeedLayout)
                InvalidateLayout();
        }

        private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Replace
            if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                int oldId = e.OldStartingIndex;
                int newId = e.NewStartingIndex;
                bool isLast = (ItemsCount == oldId + 1);

                Children.RemoveAt(oldId);
                InsertRow(e.NewItems[newId], newId, isLast);
            }
            // Add or Insert
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems == null)
                    return;

                bool isInsert = (Children.Count > 0 && e.NewStartingIndex < Children.Count);

                // For add - previous line set visible
                if (!isInsert)
                {
                    var lastRow = Children.LastOrDefault() as GridRow;
                    if (lastRow != null)
                        lastRow.UpdateLineVisibility(true);
                }

                int index;
                // For each elements except the last
                for (var i = 0; i < e.NewItems.Count - 1; ++i)
                {
                    index = i + e.NewStartingIndex;
                    var item = e.NewItems[i];

                    if (isInsert)
                    {
                        // Insert row
                        InsertRow(item, index, true);
                    }
                    else
                    {
                        // Add row
                        AddRow(item, index, true);
                    }
                }

                index = e.NewStartingIndex + e.NewItems.Count - 1;
                
                // Last item from NewItems
                var lastItem = e.NewItems[e.NewItems.Count - 1];
                if (isInsert)
                    InsertRow(lastItem, index, !(index == ItemsCount) );
                else
                    AddRow(lastItem, index, !(index == ItemsCount) );

                // recalc autonumber
                if (DataGrid.IsAutoNumberCalc)
                {
                    if (DataGrid.AutoNumberStrategy == Enums.AutoNumberStrategyType.Both)
                    {
                        for (int i = 0; i < Children.Count; i++)
                            Children[i].UpdateAutoNumeric(i);
                    }
                    else if (DataGrid.AutoNumberStrategy == Enums.AutoNumberStrategyType.Down)
                    {
                        for (int i = index; i < Children.Count; i++)
                            Children[i].UpdateAutoNumeric(i);
                    }
                    else if (DataGrid.AutoNumberStrategy == Enums.AutoNumberStrategyType.Up)
                    {
                        for (int i = index; i >= 0; i--)
                            Children[i].UpdateAutoNumeric(i);
                    }
                }

                TryOnEmptyItems();
            }
            // Remove
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems == null)
                    return;

                // Common calc
                ItemsCount -= e.OldItems.Count;

                int delIndex = e.OldStartingIndex;
                bool isLast = (delIndex == ItemsCount);
                Children.RemoveAt(delIndex);

                if (isLast)
                {
                    var last = Children.LastOrDefault() as GridRow;
                    if (last != null)
                        last.UpdateLineVisibility(false);
                }
                    
                if (DataGrid.IsAutoNumberCalc)
                {
                    // recalc autonumber
                    if (DataGrid.AutoNumberStrategy == Enums.AutoNumberStrategyType.Both)
                    {
                        for (int i = 0; i < Children.Count; i++)
                            (Children[i] as GridRow).UpdateAutoNumeric(i);
                    }
                    else if (DataGrid.AutoNumberStrategy == Enums.AutoNumberStrategyType.Down)
                    {
                        for (int i = delIndex; i < Children.Count; i++)
                            (Children[i] as GridRow).UpdateAutoNumeric(i);
                    }
                    else if (DataGrid.AutoNumberStrategy == Enums.AutoNumberStrategyType.Up)
                    {
                        if (ItemsCount > 0)
                        {
                            delIndex--;
                            if (delIndex < 0)
                                delIndex = 0;

                            for (int i = delIndex; i >= 0; i--)
                                (Children[i] as GridRow).UpdateAutoNumeric(i);
                        }
                    }
                }

                TryOnEmptyItems();
            }
            // Reset
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                Children.Clear();
                ItemsCount = 0;
                TryOnEmptyItems();
            }
            else
            {
                return;
            }
        }

        private GridRow AddRow(object bindItem, int index, bool isLineVisible, bool isAutoNumber = true)
        {
            ItemsCount++;
            var row = new GridRow(bindItem, this, index, isLineVisible, isAutoNumber);
            Children.Add(row);
            return row;
        }

        private GridRow InsertRow(object bindItem, int index, bool isLineVisible)
        {
            ItemsCount++;
            var row = new GridRow(bindItem, this, index, isLineVisible, true);
            Children.Insert(index, row);
            return row;
        }

        private void TryOnEmptyItems()
        {
            bool res = (Children.Count == 0);
            if (lastHasItems != null && res == lastHasItems.Value)
                return;

            // Actions:
            DataGrid.UpdateEmptyViewVisible();
            InvalidateLayout();

            lastHasItems = res;
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            if (Children.Count == 0)
            {
                StackHeight = -1;
                return new SizeRequest(new Size(widthConstraint, 0));
            }
            else
            {
                StackHeight = 0;
                foreach (var item in Children)
                {
                    var res = item.Measure(widthConstraint, double.PositiveInfinity, MeasureFlags.IncludeMargins);
                    StackHeight += res.Request.Height;
                }

                return new SizeRequest(new Size(widthConstraint, StackHeight));
            }
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            if (Children.Count == 0)
                return;

            double offsetY = 0;
            for (int i = 0; i < Children.Count; i++)
            {
                var row = Children[i];
                double rowHeight = row.RowHeight;
                var rect = new Rectangle(0, offsetY, width, rowHeight);

                // Draw row
                row.RenderRow(0, 0, width, rowHeight);
                // And draw row? O_O (joke) This thing is needed to fix 
                // bug with empty rows after Foreach=>Itemsource.Add(item)
                LayoutChildIntoBoundingRegion(row, rect);

                offsetY += rowHeight;
            }
        }
    }
}