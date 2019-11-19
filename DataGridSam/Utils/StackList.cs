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
                }
            );
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

        // Column lines
        //public static readonly BindableProperty ColumnLinesProperty =
        //    BindableProperty.Create(nameof(ColumnLines), typeof(Grid), typeof(StackList), null, 
        //        propertyChanged: (b, o, n) => 
        //        {
        //            var self = (StackList)b;
        //            var gridLines = (Grid)n;
        //            self.SizeChanged += (obj, e) =>
        //            {
        //                DataGrid.CalcHeightColumnLines(self, gridLines);
        //            };
        //        });
        //public Grid ColumnLines
        //{
        //    get { return (Grid)GetValue(ColumnLinesProperty); }
        //    set { SetValue(ColumnLinesProperty, value); }
        //}

        // Pagination item count
        //public static readonly BindableProperty PaginationItemCountProperty =
        //    BindableProperty.Create(nameof(PaginationItemCount), typeof(int), typeof(StackList), 0);
        //public int PaginationItemCount
        //{
        //    get { return (int)GetValue(PaginationItemCountProperty); }
        //    set { SetValue(PaginationItemCountProperty, value); }
        //}

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
                    foreach (var item in newList)
                    {
                        var view = CreateChildViewFor(self.ItemTemplate, item, self);
                        self.Children.Add(view);
                    }
                }
                else if (paginationCount > 0)
                {
                    self.RedrawForPage(paginationCount, selectPage:1);
                    //// TODO Check low perfomance
                    //var transformList = ((IEnumerable)newValue).Cast<object>().ToArray();
                    //int selectedIndex = 0;
                    //int startedIndex = 0;
                    //int startPage = 1;
                    //int count = transformList.Length;
                    //int pages = 1;
                    //if (self.DataGrid.SelectedItem != null && newList is IList list)
                    //{
                    //    selectedIndex = list.IndexOf(self.DataGrid.SelectedItem);
                    //}

                    //// Calc pages count
                    //if (count > 0)
                    //{
                    //    int calc = (count / paginationCount);
                    //    pages = (calc == 0) ? 1 : calc;
                    //}

                    //// Calc start page and start index
                    //if (selectedIndex > 0 && count > 0)
                    //{
                    //    startPage = (selectedIndex / count) * pages + 1;
                    //    if (startPage > pages)
                    //        startPage = pages;

                    //    if (startPage > 1)
                    //    {
                    //        startedIndex = (startPage / pages) * count - paginationCount;
                    //    }
                    //}

                    //self.DataGrid.PaginationCurrentPage = startPage;
                    //self.DataGrid.PaginationCurrentPageStartIndex = startedIndex;

                    //// Show buttons
                    //if (startPage > 1)
                    //    self.DataGrid.ShowPaginationBackButton(true);
                    //if (startPage < pages)
                    //    self.DataGrid.ShowPaginationNextButton(true);


                    //int endIndex = (startPage == pages) ? count : startPage * paginationCount;
                    //for (int i = startedIndex; i < endIndex; i++)
                    //{
                    //    var view = CreateChildViewFor(self.ItemTemplate, transformList[i], self);
                    //    self.Children.Add(view);
                    //}
                }
                else
                {
                    throw new Exception("DataGridSam: PaginationItemCount cant be under zero");
                }
            }

            self.UpdateChildrenLayout();
            self.InvalidateLayout();
        }

        private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Replace)
            {

                this.Children.RemoveAt(e.OldStartingIndex);

                var item = e.NewItems[e.NewStartingIndex];
                var view = CreateChildViewFor(this.ItemTemplate, item, this);

                this.Children.Insert(e.NewStartingIndex, view);
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems != null)
                {
                    for (var i = 0; i < e.NewItems.Count; ++i)
                    {
                        var item = e.NewItems[i];
                        var view = CreateChildViewFor(this.ItemTemplate, item, this);

                        int index = i + e.NewStartingIndex;
                        this.Children.Insert(index, view);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems != null)
                {
                    this.Children.RemoveAt(e.OldStartingIndex);
                    if (this.DataGrid.SelectedRow != null)
                    {
                        this.DataGrid.SelectedItem = null;
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                this.Children.Clear();
            }
            else
            {
                return;
            }
        }

        internal void RedrawForPage(int paginationCount, int selectPage = 1, object selectedItem = null)
        {
            if (selectPage <= 0)
                throw new Exception("DataGridSam: SelectedPage dont be under or equal zero");
            if (paginationCount <= 0)
                throw new Exception("DataGridSam: PaginationItemCount cant be under zero");

            // Clear old items
            Children.Clear();

            // TODO Check low perfomance
            var transformList = ItemsSource.Cast<object>().ToArray();
            int count = transformList.Length;
            int pages = 1;

            // Calc count pages by itemList count
            if (count > 0)
            {
                int calc = (count / paginationCount);
                pages = (calc == 0) ? 1 : calc;
            }

            // SelectedItem hight priopity than selectPage
            if (selectedItem != null && ItemsSource is IList list)
            {
                int selectedItemIndex = list.IndexOf(selectedItem);
                if (selectedItemIndex > 0 && count > 0)
                {
                    selectPage = (selectedItemIndex / count) * pages + 1;
                    if (selectPage > pages)
                        selectPage = pages;
                }
            }

            // Calculate start index by selected page
            //decimal d = selectPage / pages;
            double c = ((double)selectPage/pages) * count;
            int startedIndex = (int)c - paginationCount;

            DataGrid.PaginationCurrentPage = selectPage;
            DataGrid.PaginationCurrentPageStartIndex = startedIndex;

            // Show buttons
            DataGrid.ShowPaginationBackButton(selectPage > 1);
            DataGrid.ShowPaginationNextButton(selectPage < pages);


            int endIndex = (selectPage == pages) ? count : selectPage * paginationCount;
            for (int i = startedIndex; i < endIndex; i++)
            {
                var view = CreateChildViewFor(ItemTemplate, transformList[i], this);
                Children.Add(view);
            }
        }

        /// <summary>
        /// Создает строку для таблицы
        /// </summary>
        /// <param name="template">Шаблон который будет использован для создания элемента</param>
        /// <param name="item">модель данных</param>
        /// <param name="container">StackList</param>
        private static View CreateChildViewFor(DataTemplate template, object item, BindableObject container)
        {
            if (template is DataTemplateSelector selector)
                template = selector.SelectTemplate(item, container);

            // Здесь происходит неявный вызов метода : DataGridViewCell.CreateView()
            var view = (View)template.CreateContent();

            return view;
        }

    }
}