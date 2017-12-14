using Xamarin.Forms;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

using Device = Xamarin.Forms.Device;

namespace Jaktloggen
{
    public partial class App : Application
    {
        public static bool UseMockDataStore = false;
        public static string BackendUrl = "http://localhost:8607";
        public static IDataStore<Hunt> HuntDataStore => DependencyService.Get<IDataStore<Hunt>>();
        public static IDataStore<Log> LogDataStore => DependencyService.Get<IDataStore<Log>>();
        public App()
        {
            InitializeComponent();

            RegisterDataStores();

            if (Device.RuntimePlatform == Device.iOS)
                MainPage = new MainPage();
            else
                MainPage = new NavigationPage(new MainPage());
        }

        private void RegisterDataStores()
        {
            if (UseMockDataStore)
            {
                DependencyService.Register<DataStores.Mock.HuntDataStore>();
                DependencyService.Register<DataStores.Mock.LogDataStore>();
            }
            else
            {
                DependencyService.Register<DataStores.File.HuntDataStore>();
                DependencyService.Register<DataStores.Cloud.LogDataStore>();
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            AppCenter.Start("android=8ce55db8-e0f4-42aa-8839-db304d8bfe52;" + "uwp={Your UWP App secret here};" +
                            "ios=8ec886eb-d541-4ae6-ab45-e263af15f58a;",
                typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
