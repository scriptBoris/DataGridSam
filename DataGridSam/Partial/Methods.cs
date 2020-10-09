using DataGridSam.Elements;
using DataGridSam.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace DataGridSam
{
    public partial class DataGrid
    {
        private void Init()
        {
            InitHeaderView();
            UpdateRowTriggers();
        }

        private void InitHeaderView()
        {
            SetColumnsBindingContext();
            headGrid.UpdateColumns();
            bodyGrid.UpdateColumns();
        }

        internal void UpdateHeadHeight(int height)
        {
            var row = RowDefinitions.First();

            if (height < 0)
                row.Height = GridLength.Auto;
            else
                row.Height = new GridLength(height);
        }

        internal void UpdateColumnVisibile()
        {
            headGrid.Redraw();
            bodyGrid.Redraw();
            stackList.Redraw();
        }

        private void UpdateRowTriggers()
        {
            int i = 0;
            foreach (var item in RowTriggers)
            {
                item.Priority = i++;
            }
        }

        internal void UpdateEmptyView()
        {
            if (bodyGrid.EmptyView != null)
                bodyGrid.Children.Remove(bodyGrid.EmptyView);

            bodyGrid.EmptyView = ViewForEmpty;
            bodyGrid.Children.Add(ViewForEmpty);

            UpdateEmptyViewVisible();
        }

        internal void UpdateEmptyViewVisible()
        {
            if (ViewForEmpty == null)
                return;

            if (stackList.Children.Count == 0)
            {
                if (!bodyGrid.EmptyView.IsVisible)
                    bodyGrid.EmptyView.IsVisible = true;
            }
            else 
            {
                if (bodyGrid.EmptyView.IsVisible)
                    bodyGrid.EmptyView.IsVisible = false;
            }
        }

        private void UpdateBorderColor()
        {
            headGrid.UpdateBorderColor();
            bodyGrid.UpdateBorderColor();
        }

        private void UpdateBorderWidth()
        {
            headGrid.Redraw();
            bodyGrid.Redraw();
            stackList.Redraw();
            absoluteBottom.TranslationY = BorderWidth * 0.2;
        }

        private void SetColumnsBindingContext()
        {
            if (Columns != null)
                foreach (var c in Columns)
                    c.BindingContext = BindingContext;
        }

        private void UpdateHeaderStyle(Style style)
        {
            if (headGrid == null)
                return;

            foreach (var col in headGrid.Children)
            {
                if (col is StackLayout stackLayout && stackLayout.Children.First() is Label label)
                {
                    label.Style = style ?? HeaderDefaultStyle;
                }
            }
        }

        internal void UpdateIsWrapped()
        {
            if (!IsWrapped)
                absoluteBottom.IsVisible = false;
        }

        internal void CheckWrapperBottomVisible(object obj, EventArgs e)
        {
            if (!IsWrapped)
                return;

            if (mainScroll.Height > stackList.StackHeight)
            {
                absoluteBottom.IsVisible = false;
            }
            else
            {
                absoluteBottom.IsVisible = true;
            }
        }

        /// <summary>
        /// Called every time an ItemsSource changes
        /// Вызывается каждый раз, когда меняется ItemsSource;
        /// </summary>
        internal void OnChangeItemsBindingContext(Type newTypeItems)
        {
            foreach (var trigger in RowTriggers)
            {
                trigger.OnSourceTypeChanged(newTypeItems);
            }
        }

        private void UpdateSelectedItem(object newItem)
        {
            int selectedId = -1;

            if (ItemsSource is IList list)
                selectedId = list.IndexOf(newItem);

            if (selectedId >=0)
                stackList.Children[selectedId].SelectRow();
        }
    }
}
