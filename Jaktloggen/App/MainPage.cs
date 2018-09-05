using Xamarin.Forms;

namespace Jaktloggen
{
    public class MainPage : TabbedPage
    {
        public MainPage()
        {
            Page itemsPage, aboutPage, huntersPage, dogsPage, speciesPage, statsPage, fieldsPage = null;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    itemsPage = new NavigationPage(new HuntsPage())
                                {
                                    Title = "Jaktturer"
                                };

                    huntersPage = new NavigationPage(new HuntersPage())
                                {
                                    Title = "Jegere"
                                };
                    dogsPage = new NavigationPage(new DogsPage())
                                {
                                    Title = "Hunder"
                                };
                    speciesPage = new NavigationPage(new SpeciesPage())
                                {
                                    Title = "Arter"
                                };
                    aboutPage = new NavigationPage(new AboutPage())
                                {
                                    Title = "Info"
                                };
                    statsPage = new NavigationPage(new StatsPage())
                    {
                        Title = "Statistikk"
                    };
                    fieldsPage = new NavigationPage(new CustomFieldsPage())
                    {
                        Title = "Ekstra felter"
                    };
                    itemsPage.Icon = "Tabbar/gevir.png";
                    huntersPage.Icon = "Tabbar/hunters.png";
                    dogsPage.Icon = "Tabbar/dog.png";
                    speciesPage.Icon = "Arter.png";
                    aboutPage.Icon = "Tabbar/info.png";
                    statsPage.Icon = "Tabbar/stats.png";
                    fieldsPage.Icon = "Felter.png";
                    break;
                default:
                    itemsPage = new HuntsPage()
                                {
                                    Title = "Jakt"
                                };

                    huntersPage = new HuntersPage()
                                {
                                    Title = "Jegere"
                                };

                    dogsPage = new DogsPage()
                    {
                        Title = "Hunder"
                    };

                    speciesPage = new SpeciesPage()
                    {
                        Title = "Arter"
                    };
                    aboutPage = new AboutPage()
                    {
                        Title = "Info"
                    };
                    statsPage = new StatsPage() { Title = "Statistikk" };
                    fieldsPage = new CustomFieldsPage() { Title = "Ekstra felter" };
                    break;

            }

            Children.Add(itemsPage);
            Children.Add(huntersPage);
            Children.Add(speciesPage);
            Children.Add(fieldsPage);
            Children.Add(dogsPage);
            Children.Add(statsPage);
            Children.Add(aboutPage);
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            Title = CurrentPage?.Title ?? string.Empty;
        }
    }
}
