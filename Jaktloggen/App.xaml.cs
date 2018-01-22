using Xamarin.Forms;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

using Device = Xamarin.Forms.Device;
using System;
using Jaktloggen.Services;

namespace Jaktloggen
{
    public partial class App : Application
    {
        public static bool UseMockDataStore = false;
        public static string BackendUrl = "http://localhost:8607";
        public static IDataStore<Jakt> HuntDataStore => DependencyService.Get<IDataStore<Jakt>>();
        public static IDataStore<Logg> LogDataStore => DependencyService.Get<IDataStore<Logg>>();
        public static IDataStore<Jeger> HunterDataStore => DependencyService.Get<IDataStore<Jeger>>();
        public static IDataStore<Dog> DogDataStore => DependencyService.Get<IDataStore<Dog>>();
        public static IDataStore<Art> SpecieDataStore => DependencyService.Get<IDataStore<Art>>();
        public static IDataStore<ArtGroup> SpecieGroupDataStore => DependencyService.Get<IDataStore<ArtGroup>>();

        private static string FILE_ART = "arter.xml";
        private static string FILE_LOGGTYPE_GROUP = "loggtypegroup.xml";
        private static string FILE_LOGGTYPER = "loggtyper.xml";
        private static string FILE_ARTGROUP = "artgroup.xml";

        public App()
        {
            InitializeComponent();
            //TODO Kopiere faste data: Arter, Artgrupper osv

            LoadXmlData();

            RegisterDataStores();

            if (Device.RuntimePlatform == Device.iOS)
                MainPage = new MainPage();
            else
                MainPage = new NavigationPage(new MainPage());
        }

        private void LoadXmlData()
        {
            if (!FileService.Exists(FILE_ARTGROUP))
            {
                FileService.CopyToAppFolder(FILE_ARTGROUP);
            }

            if (!FileService.Exists(FILE_ART))
            {
                FileService.CopyToAppFolder(FILE_ART);
            }

            if (!FileService.Exists(FILE_LOGGTYPE_GROUP))
            {
                FileService.CopyToAppFolder(FILE_LOGGTYPE_GROUP);
            }

            if (!FileService.Exists(FILE_LOGGTYPER))
            {
                FileService.CopyToAppFolder(FILE_LOGGTYPER);
            }
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
                DependencyService.Register<DataStores.File.DataStore<Jakt>>();
                DependencyService.Register<DataStores.File.DataStore<Logg>>();
                DependencyService.Register<DataStores.File.DataStore<Jeger>>();
                DependencyService.Register<DataStores.File.DataStore<Dog>>();
                DependencyService.Register<DataStores.File.DataStore<Art>>();
                DependencyService.Register<DataStores.File.DataStore<ArtGroup>>();
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
