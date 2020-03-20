# Screenshots
![Repo_List](Screenshots/Screenshot.png)
![Repo_List](Screenshots/Screenshot2.png)


## Nuget Installation

[https://www.nuget.org/packages/DataGridSam/](https://www.nuget.org/packages/DataGridSam/)

```bash
Install-Package DataGridSam
```

## Supported Platforms
 - Android
 - iOS (сurrently not available)
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
## Install UWP project
In file **App.xaml.cs**, enter `DataGridSam.UWP.Initialize.Init();` as below
```c#
    protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;
                Xamarin.Forms.Forms.Init(e);
                DataGridSam.UWP.Initialize.Init();

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                }

                Window.Current.Content = rootFrame;
            }
	    ...
        }
```

## Example
```xml
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
	     xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
	     xmlns:sam="clr-namespace:DataGridSam;assembly=DataGridSam"
	     x:Class="Sample.Views.TestView">
    <StackLayout>
        <sam:DataGrid ItemsSource="{Binding Items}">
            
            <sam:DataGridColumn Title="#" 
                                AutoNumber="Down"
                                Width="40"/>
		    
	    <sam:DataGridColumn Title="Photo" 
                                Width="90">
                <sam:DataGridColumn.CellTemplate>
                    <DataTemplate>
                        <Image Source="{Binding PhotoUrl}"/>
                    </DataTemplate>
                </sam:DataGridColumn.CellTemplate>
            </sam:DataGridColumn>
		    
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
        </sam:DataGrid>
    </StackLayout>
</ContentPage>
```


## Referenced source code

* https://github.com/muak/AiForms.Layouts
* https://github.com/akgulebubekir/Xamarin.Forms.DataGrid

## License

MIT Licensed
