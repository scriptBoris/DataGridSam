using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam.Utils
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    internal sealed class StackCell : Grid
    {
        private List<GridCell> cells = new List<GridCell>();
        private bool isStyleDefault = true;

        // Data grid sam
        public static readonly BindableProperty DataGridProperty =
            BindableProperty.Create(nameof(DataGrid), typeof(DataGrid), typeof(StackCell), null,
                propertyChanged: (b, o, n) =>
                {
                    (b as StackCell).CreateView();
                });
        public DataGrid DataGrid
        {
            get { return (DataGrid)GetValue(DataGridProperty); }
            set { SetValue(DataGridProperty, value); }
        }

        // Row context
        public static readonly BindableProperty RowContextProperty =
            BindableProperty.Create(nameof(RowContext), typeof(object), typeof(StackCell),
                propertyChanged: (b, o, n) =>
                {
                    var self = (StackCell)b;
                    var click = (TapGestureRecognizer)self.GestureRecognizers.FirstOrDefault();
                    // Triggers
                    if (n is INotifyPropertyChanged model)
                        model.PropertyChanged += self.Model_PropertyChanged;

                    click.CommandParameter = n;
                });
        public object RowContext
        {
            get { return GetValue(RowContextProperty); }
            set { SetValue(RowContextProperty, value); }
        }

        private void CreateView()
        {
            RowSpacing = 0;
            ColumnSpacing = DataGrid.LinesWidth;

            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = new GridLength(DataGrid.LinesWidth) },
            };

            int index = 0;
            //bool[] columnWithCustomTemplate = new bool [DataGrid.Columns.Count];

            foreach (var column in DataGrid.Columns)
            {
                ColumnDefinitions.Add(new ColumnDefinition() { Width = column.Width });

                var cell = new GridCell
                {
                    Column = column,
                };

                if (column.CellTemplate != null)
                {
                    cell.Wrapper = new ContentView() { Content = column.CellTemplate.CreateContent() as View };
                    cell.IsCustomTemplate = true;
                    //columnWithCustomTemplate[index] = true;
                }
                else
                {
                    var textColor = column.CellTextColor ?? DataGrid.RowsTextColor;
                    var backgroundColor = DataGrid.RowsColor;

                    var label = new Label
                    {
                        TextColor = textColor,
                        FontSize = DataGrid.RowsFontSize,
                        HorizontalOptions = column.HorizontalContentAlignment,
                        VerticalOptions = column.VerticalContentAlignment,
                        HorizontalTextAlignment = column.HorizontalTextAlignment,
                        VerticalTextAlignment = column.VerticalTextAlignment,
                        LineBreakMode = LineBreakMode.WordWrap,
                    };
                    label.SetBinding(Label.TextProperty, new Binding(column.PropertyName, BindingMode.Default, stringFormat: column.StringFormat));
                    //text.SetBinding(Label.FontSizeProperty, new Binding(DataGrid.FontSizeProperty.PropertyName, BindingMode.Default, source: DataGrid));
                    //text.SetBinding(Label.FontFamilyProperty, new Binding(DataGrid.FontFamilyProperty.PropertyName, BindingMode.Default, source: DataGrid));

                    cell.Wrapper = new ContentView
                    {
                        Padding = DataGrid.CellPadding,
                        Content = label,
                        BackgroundColor = backgroundColor,
                    };
                    cell.Label = label;
                }

                SetColumn(cell.Wrapper, index);
                SetRow(cell.Wrapper, 0);
                Children.Add(cell.Wrapper);
                cells.Add(cell);

                index++;
            }

            // Create horizontal line table
            var line = CreateHorizontalLine();
            SetRow(line, 1);
            SetColumn(line, 0);
            SetColumnSpan(line, DataGrid.Columns.Count);
            Children.Add(line);

            // Add tap event
            // Set only tap command, setting CommandParameter - after changed "RowContext" :)
            var tapControll = new TapGestureRecognizer();
            tapControll.Tapped += TapControll_Tapped;
            GestureRecognizers.Add(tapControll);
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ThrowTriggerStyle(e.PropertyName);
        }

        private void TapControll_Tapped(object sender, EventArgs e)
        {
            // GUI Unselected last row
            var last = DataGrid.SelectedCell;
            if (last != null)
            {
                foreach (var item in last.cells)
                {
                    if (!item.IsCustomTemplate)
                        item.Wrapper.BackgroundColor = DataGrid.RowsColor;
                }
            }

            // GUI Selected row
            DataGrid.SelectedCell = this;
            DataGrid.SelectedItem = this.BindingContext;
            foreach (var item in cells)
            {
                if (!item.IsCustomTemplate)
                    item.Wrapper.BackgroundColor = DataGrid.SelectedRowColor;
            }

            // Run ICommand selected item
            DataGrid.CommandSelectedItem?.Execute(this.BindingContext);
        }

        private void ThrowTriggerStyle(string propName)
        {
            if (DataGrid.RowTriggers.Count == 0)
                return;

            bool doneChanged = false;
            foreach (var trigger in DataGrid.RowTriggers)
            {
                if (propName == trigger.PropertyTrigger)
                {
                    var value = RowContext.GetType().GetProperty(trigger.PropertyTrigger).GetValue(RowContext);
                    var t1 = value.GetType();
                    var t2 = trigger.Value.GetType();

                    if (t1 == t2)
                    {
                        if (value is bool bValue && trigger.Value is bool bTrigger && bValue == bTrigger)
                        {
                            doneChanged = true;
                            SetStyleRowByTrigger(trigger);
                        }
                        else if (value is int iValue && trigger.Value is int iTrigger && iValue == iTrigger)
                        {
                            doneChanged = true;
                            SetStyleRowByTrigger(trigger);
                        }
                        else if (value is string sValue && trigger.Value is string sTrigger && sValue == sTrigger)
                        {
                            doneChanged = true;
                            SetStyleRowByTrigger(trigger);
                        }
                    }
                }
            }

            if (!doneChanged && !isStyleDefault)
            {
                SetStyleDefault();
            }
        }

        private void SetStyleRowByTrigger(RowTrigger trigger)
        {
            foreach (var item in cells)
            {
                if (!item.IsCustomTemplate)
                {
                    if (item.Label != null)
                        item.Label.TextColor = trigger.RowTextColor;

                    if (item.Wrapper != null)
                        item.Wrapper.BackgroundColor = trigger.RowBackgroundColor;
                }
            }
            isStyleDefault = false;
        }

        private void SetStyleDefault()
        {
            foreach (var item in cells)
            {
                if (!item.IsCustomTemplate)
                {
                    if (item.Label != null)
                        item.Label.TextColor = item.Column.CellTextColor ?? DataGrid.RowsTextColor;

                    if (item.Wrapper != null)
                        item.Wrapper.BackgroundColor = DataGrid.RowsColor;
                }
            }
            isStyleDefault = true;
        }

        private BoxView CreateHorizontalLine()
        {
            var line = new BoxView
            {
                BackgroundColor = DataGrid.LinesColor,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            return line;
        }
    }
}
