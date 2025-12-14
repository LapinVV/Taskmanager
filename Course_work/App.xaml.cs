using Course_work.Models;

namespace Course_work;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        
        DataManager.LoadData();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new NavigationPage(new MainPage()));
        
        window.Destroying += (s, e) =>
        {
            AppData.SaveAllData();
        };
        
        return window;
    }
}