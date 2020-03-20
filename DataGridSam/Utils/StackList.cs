using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam.Utils
{
    /// <summary>
    /// RepeatableStack
    /// StackLayout corresponding to ItemsSource and ItemTemplate
    /// </summary>
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    internal class StackList : StackLayout
    {
        // ItemsSource
        public static BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(StackList), null, defaultBindingMode: BindingMode.OneWay,
                propertyChanged: ItemsChanged);
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Has items
        public static BindableProperty HasItemsProperty =
            BindableProperty.Create(nameof(HasItems), typeof(bool), typeof(StackList), true);
        public bool HasItems
        {
            get { return (bool)GetValue(HasItemsProperty); }
            set { SetValue(HasItemsProperty, value); }
        }

        // ItemTemplate
        public static BindableProperty ItemTemplateProperty =
            BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(StackList), default(DataTemplate),
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    var control = (StackList)bindable;
                    //when to occur propertychanged earlier ItemsSource than ItemTemplate, raise ItemsChanged manually
                    if (newValue != null && control.ItemsSource != null && !control.doneItemSourceChanged)
                    {
                        ItemsChanged(bindable, null, control.ItemsSource);
                    }
                });
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        // DataGrid
        public static readonly BindableProperty DataGridProperty =
            BindableProperty.Create(nameof(DataGrid), typeof(DataGrid), typeof(StackList), null);
        public DataGrid DataGrid
        {
            get { return (DataGrid)GetValue(DataGridProperty); }
            set { SetValue(DataGridProperty, value); }
        }

        private bool doneItemSourceChanged = false;

        private static void ItemsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (StackList)bindable;
            // when to occur propertychanged earlier ItemsSource than ItemTemplate, do nothing.
            if (self.ItemTemplate == null)
            {
                self.doneItemSourceChanged = false;
                return;
            }

            self.doneItemSourceChanged = true;

            IEnumerable newList;
            try
            {
                newList = newValue as IEnumerable;
            }
            catch (Exception e)
            {
                throw e;
            }

            if (oldValue is INotifyCollectionChanged oldObservableCollection)
            {
                oldObservableCollection.CollectionChanged -= self.OnItemsSourceCollectionChanged;
            }

            if (newValue is INotifyCollectionChanged newObservableCollection)
            {
                newObservableCollection.CollectionChanged += self.OnItemsSourceCollectionChanged;
            }

            self.Children.Clear();
            if (newList != null)
            {
                int paginationCount = self.DataGrid.PaginationItemCount;
                if (paginationCount == 0)
                {
                    // Without pagination
                    int i = 0;
                    View last = null;
                    foreach (var item in newList)
                    {
                        // Say triggers what binding context changed
                        if (i == 0)
                            self.DataGrid.OnChangeItemsBindingContext(item.GetType());

                        last = CreateChildViewFor(self.ItemTemplate, item, self, i);
                        self.Children.Add(last);
                        i++;
                    }

                    if (last != null)
                    {
                        var row = (Row)last;
                        row.line.IsVisible = false;
                    }
                }
                else if (paginationCount > 0)
                {
                    // Pagination
                    self.RedrawForPage(paginationCount, selectPage:1);
                }
                else
                {
                    throw new Exception("DataGridSam: PaginationItemCount cant be under zero");
                }
            }

            self.HasItems = (self.Children.Count > 0);
            self.UpdateChildrenLayout();
            self.InvalidateLayout();
        }

        public void Update()
        {
            UpdateChildrenLayout();
            InvalidateLayout();
        }

        private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                Children.RemoveAt(e.OldStartingIndex);

                var item = e.NewItems[e.NewStartingIndex];
                var view = CreateChildViewFor(this.ItemTemplate, item, this, e.NewStartingIndex);
                Children.Insert(e.NewStartingIndex, view);

                // Hide line if row is last
                if (Children.LastOrDefault() == view)
                    (view as Row).line.IsVisible = false;
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems != null)
                {
                    bool isInsert;

                    if (Children.Count == 0)
                        isInsert = false;
                    else if (e.NewStartingIndex < Children.Count)
                        isInsert = true;
                    else
                        isInsert = false;

                    // Logic for pagination if that enabled
                    if (DataGrid.PaginationItemCount > 0)
                    {
                        int startIndex = e.NewStartingIndex;
                        int endIndex = e.NewStartingIndex + e.NewItems.Count-1;
                    
                        if (startIndex > DataGrid.PaginationCurrentPageEndIndex)
                        {
                            DataGrid.ShowPaginationNextButton(true);
                            return;
                        }

                        if (endIndex < DataGrid.PaginationCurrentPageStartIndex)
                        {
                            return;
                        }
                    }

                    View child;

                    // For add - last line set visible
                    if (!isInsert)
                    {
                        child = Children.LastOrDefault();
                        if (child != null)
                            (child as Row).line.IsVisible = true;
                    }

                    int pause = 0;
                    for (var i = 0; i < e.NewItems.Count; ++i)
                    {
                        int index = i + e.NewStartingIndex;
                        pause = index;
                        var item = e.NewItems[i];

                        child = CreateChildViewFor(this.ItemTemplate, item, this, index);
                        Children.Insert(index, child);

                        // line visibile
                        if (!isInsert && index == Children.Count - 1)
                        {
                            (child as Row).line.IsVisible = false;
                        }
                    }

                    // recalc autonumber
                    if (DataGrid.IsCalcAutoNumber)
                    {
                        for (int i = pause; i < Children.Count; i++)
                            ((Row)Children[i]).UpdateAutoNumeric(i + 1);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems == null)
                    return;

                // Delete item with ENABLED Pagination
                if (DataGrid.PaginationItemCount > 0)
                {
                    int startIndex = e.OldStartingIndex;
                    int endIndex = e.OldStartingIndex + e.OldItems.Count - 1;
                    var source = (IList)ItemsSource;

                    // if next page
                    if (startIndex > DataGrid.PaginationCurrentPageEndIndex)
                    {
                        if (source.Count-1 <= DataGrid.PaginationCurrentPageEndIndex)
                            DataGrid.ShowPaginationNextButton(false);
                    }
                    // Previus page or other
                    else
                    {
                        startIndex -= DataGrid.PaginationCurrentPageStartIndex;
                        if (startIndex < 0)
                            startIndex = 0;
                        endIndex = startIndex + (e.OldItems.Count - 1);


                        for (int i = startIndex; i <= endIndex; i++)
                        {
                            if (i > DataGrid.PaginationItemCount - 1 || i > source.Count)
                                break;

                            RemoveAt(i);
                        }

                        // go to previus page if current page clear
                        if (Children.Count == 0 && source.Count > 0)
                        {
                            RedrawForPage(DataGrid.PaginationItemCount, DataGrid.PaginationCurrentPage-1);
                            return;
                        }

                        startIndex = DataGrid.PaginationCurrentPageEndIndex;
                        int end = startIndex + e.OldItems.Count - 1;
                        for (int i=startIndex; i<=end; i++)
                        {
                            if (i >= source.Count - 1)
                                DataGrid.ShowPaginationNextButton(false);

                            if (i > source.Count - 1)
                                break;

                            var view = CreateChildViewFor(this.ItemTemplate, source[i], this, i);
                            Add(view);
                        }
                    }
                }
                // Default delete item
                else
                {
                    //RemoveAt(e.OldStartingIndex);
                    bool isLast = (e.OldStartingIndex == Children.Count - 1);
                    Children.RemoveAt(e.OldStartingIndex);

                    if (isLast)
                    {
                        var last = Children.LastOrDefault() as Row;
                        if (last != null)
                        {
                            last.line.IsVisible = false;
                        }
                    }
                    else if (DataGrid.IsCalcAutoNumber)
                    {
                        // Auto numeric
                        for (int i = e.OldStartingIndex; i < Children.Count; i++)
                        {
                            var row = (Row)Children[i];
                            row.UpdateAutoNumeric(i + 1);
                        }
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                Children.Clear();
            }
            else
            {
                return;
            }
            HasItems = (Children.Count > 0);
        }

        /// <summary>
        /// Update list for selectedPage (Pagination)
        /// </summary>
        /// <param name="paginationCount"></param>
        /// <param name="selectPage"></param>
        /// <param name="selectedItem"></param>
        internal void RedrawForPage(int paginationCount, int selectPage = 1, object selectedItem = null)
        {
            if (paginationCount <= 0)
                throw new Exception("DataGridSam: PaginationItemCount cant be under or equal zero");

            // Clear old items
            Children.Clear();

            // TODO Check low perfomance
            var itemList = ItemsSource.Cast<object>().ToArray();
            int itemCount = itemList.Length;
            int pages = 1;

            // Calc count pages by itemList count
            if (itemCount > 0)
            {
                float f = (float)itemCount / (float)paginationCount;
                int calc = (int)f;
                if (f > calc)
                    calc++;

                pages = (calc == 0) ? 1 : calc;
            }

            // Fix select page if out of range
            if (selectPage <= 0)
                selectPage = 1;
            else if (selectPage > pages)
                selectPage = pages;

            // SelectedItem hight priopity than selectPage
            if (selectedItem != null && ItemsSource is IList list)
            {
                int selectedItemIndex = list.IndexOf(selectedItem);
                if (selectedItemIndex > 0 && itemCount > 0)
                {
                    selectPage = (selectedItemIndex / itemCount) * pages + 1;
                    if (selectPage > pages)
                        selectPage = pages;
                }
            }

            // Calculate start index
            //double c = ((double)selectPage/pages) * itemCount;
            //int startedIndex = (int)c - paginationCount;
            int startedIndex = (selectPage * paginationCount) - paginationCount;
            if (startedIndex < 0)
                startedIndex = 0;

            // Calculate end index
            int endIndex = (selectPage == pages) ? itemCount-1 : (selectPage * paginationCount)-1;

            // Save system data
            DataGrid.PaginationCurrentPage = selectPage;
            DataGrid.PaginationCurrentPageStartIndex = startedIndex;
            DataGrid.PaginationCurrentPageItemsEndIndex = endIndex;
            DataGrid.PaginationCurrentPageEndIndex = (selectPage * paginationCount)-1;

            // Show buttons
            DataGrid.ShowPaginationBackButton(selectPage > 1);
            DataGrid.ShowPaginationNextButton(selectPage < pages);

            // Create rows
            View last = null;
            for (int i = startedIndex; i <= endIndex; i++)
            {
                last = CreateChildViewFor(ItemTemplate, itemList[i], this, i);
                Children.Add(last);
            }

            if (last != null)
            {
                var row = (Row)last;
                row.line.IsVisible = false;
            }

            HasItems = (Children.Count > 0);
        }

        private void RemoveAt(int oldStartingIndex)
        {
            bool isLast = (oldStartingIndex == Children.Count - 1);
            Children.RemoveAt(oldStartingIndex);

            if (isLast)
            {
                var last = Children.LastOrDefault() as Row;
                if (last != null)
                {
                    last.line.IsVisible = false;
                }
            }
        }

        private void Add(View view)
        {
            var last = Children.LastOrDefault() as Row;
            if (last != null)
            {
                last.line.IsVisible = true;
            }

            (view as Row).line.IsVisible = false;
            Children.Add(view);
        }



        /// <summary>
        /// Создает строку для таблицы
        /// </summary>
        /// <param name="template">Шаблон который будет использован для создания элемента</param>
        /// <param name="item">Модель данных</param>
        /// <param name="container">StackList</param>
        /// <param name="index">Индекс для автонумерации</param>
        private static View CreateChildViewFor(DataTemplate template, object item, 
            BindableObject container, int index = -1)
        {
            if (template is DataTemplateSelector selector)
                template = selector.SelectTemplate(item, container);

            // Здесь происходит неявный вызов метода : DataGridViewCell.CreateView()
            var view = (View)template.CreateContent();
            
            // Auto number
            if (index > -1)
            {
                var row = (Row)view;
                if (row.DataGrid.IsCalcAutoNumber)
                    row.UpdateAutoNumeric(index + 1);
            }

            return view;
        }
    }
}