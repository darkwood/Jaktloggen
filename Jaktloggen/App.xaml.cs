using Xamarin.Forms;

namespace Jaktloggen
{
    public partial class App : Application
    {
        public static bool UseMockDataStore = true;
        public static string BackendUrl = "https://localhost:5000";
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
                DependencyService.Register<Mock.HuntDataStore>();
                DependencyService.Register<Mock.LogDataStore>();
            }
            else
            {
                DependencyService.Register<CloudDataStore<Hunt>>();
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
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
