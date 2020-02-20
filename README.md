# DataGridSam for Xamarin.Forms
Simple and fast GUI element for Xamarin.Forms

![Repo_List](Screenshots/Head.png)

# Screenshot of real use
![Repo_List](Screenshots/Screenshot_20191118-171142.jpg)

## Nuget Installation

[https://www.nuget.org/packages/DataGridSam/](https://www.nuget.org/packages/DataGridSam/)

```bash
Install-Package DataGridSam
```

## Supported Platforms
 - Android
 - iOS
 - UWP


## Install android project
```c#
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(savedInstanceState);

            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            DataGridSam.Droid.Initialize.Init();
            LoadApplication(new App());
        }
    }
```

## Examle
```xml
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
		     xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
		     xmlns:sam="clr-namespace:DataGridSam;assembly=DataGridSam"
		     x:Class="Sample.Views.TestView">
    <StackLayout>
        <sam:DataGrid ItemsSource="{Binding Items}">
            <sam:DataGrid.Columns>
                <sam:DataGridColumn Title="#" 
                                    PropertyName="PositionNumber"
                                    Width="40"/>
                <sam:DataGridColumn Title="Name" 
                                    PropertyName="Name"
                                    Width="2*"/>
                <sam:DataGridColumn Title="Price" 
                                    PropertyName="Price"
                                    Width="1*"
                                    HorizontalTextAlignment="End"/>
                <sam:DataGridColumn Title="Weight" 
                                    PropertyName="Weight"
                                    Width="1*"/>
            </sam:DataGrid.Columns>
        </sam:DataGrid>
    </StackLayout>
</ContentPage>
```


## Referenced source code

* https://github.com/muak/AiForms.Layouts
* https://github.com/akgulebubekir/Xamarin.Forms.DataGrid

## License

MIT Licensed
