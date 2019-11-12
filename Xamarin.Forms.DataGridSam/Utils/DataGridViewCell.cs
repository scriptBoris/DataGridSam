﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam.Utils
{
    internal sealed class DataGridViewCell : Grid
    {
        private Color backgroundColor = Color.White;
        private Color textColor = Color.Black;
        private Color lineColor = Color.Blue;

        public DataGridViewCell()
        {
        }

        #region properties
        public DataGrid DataGrid
        {
            get { return (DataGrid)GetValue(DataGridProperty); }
            set { SetValue(DataGridProperty, value); }
        }

        public int Index
        {
            get { return (int)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }

        public object RowContext
        {
            get { return GetValue(RowContextProperty); }
            set { SetValue(RowContextProperty, value); }
        }
        #endregion

        #region Bindable Properties
        public static readonly BindableProperty DataGridProperty =
            BindableProperty.Create(nameof(DataGrid), typeof(DataGrid), typeof(DataGridViewCell), null,
                propertyChanged: (b, o, n) => (b as DataGridViewCell).CreateView());

        public static readonly BindableProperty IndexProperty =
            BindableProperty.Create(nameof(Index), typeof(int), typeof(DataGridViewCell), 0);

        public static readonly BindableProperty RowContextProperty =
            BindableProperty.Create(nameof(RowContext), typeof(object), typeof(DataGridViewCell));
        #endregion

        #region Methods
        private void CreateView()
        {
            RowSpacing = 0;
            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = new GridLength(DataGrid.LinesWidth) },
            };

            int index = 0;
            foreach (var column in DataGrid.Columns)
            {
                ColumnDefinitions.Add(new ColumnDefinition() { Width = column.Width });
                var label = new Label
                {
                    TextColor = textColor,
                    HorizontalOptions = column.HorizontalContentAlignment,
                    VerticalOptions = column.VerticalContentAlignment,
                    HorizontalTextAlignment = column.HorizontalTextAlignment,
                    VerticalTextAlignment = column.VerticalTextAlignment,
                    LineBreakMode = LineBreakMode.WordWrap,
                };
                label.SetBinding(Label.TextProperty, new Binding(column.PropertyName, BindingMode.Default, stringFormat: column.StringFormat));
                //text.SetBinding(Label.FontSizeProperty, new Binding(DataGrid.FontSizeProperty.PropertyName, BindingMode.Default, source: DataGrid));
                //text.SetBinding(Label.FontFamilyProperty, new Binding(DataGrid.FontFamilyProperty.PropertyName, BindingMode.Default, source: DataGrid));

                var cell = new ContentView
                {
                    Padding = 0,
                    Content = label,
                };
                SetColumn(cell, index);
                SetRow(cell, 0);
                Children.Add(cell);
                index++;
            }

            var line = CreateLine();
            SetRow(line, 1);
            SetColumn(line, 0);
            SetColumnSpan(line, DataGrid.Columns.Count);
            Children.Add(line);
        }

        private View CreateLine()
        {
            var line = new BoxView
            {
                BackgroundColor = lineColor,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            return line;
        }


        //private void ChangeColor(Color color)
        //{
        //    foreach (var v in mainLayout.Children)
        //    {
        //        v.BackgroundColor = color;
        //        var contentView = v as ContentView;
        //        //if (contentView?.Content is Label)
        //            //((Label)contentView.Content).TextColor = _textColor;
        //    }
        //}
        #endregion
    }
}