using Xamarin.Forms;

namespace Jaktloggen
{
    public class MainPage : TabbedPage
    {
        public MainPage()
        {
            Page itemsPage, aboutPage, huntersPage = null;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    itemsPage = new NavigationPage(new HuntsPage())
                                {
                                    Title = "Browse"
                                };

                    huntersPage = new NavigationPage(new HuntersPage())
                                {
                                    Title = "Jegere"
                                };

                    aboutPage = new NavigationPage(new AboutPage())
                                {
                                    Title = "About"
                                };
                    itemsPage.Icon = "tab_feed.png";
                    aboutPage.Icon = "tab_about.png";
                    break;
                default:
                    itemsPage = new HuntsPage()
                                {
                                    Title = "Browse"
                                };

                    huntersPage = new HuntersPage()
                                {
                                    Title = "Jegere"
                                };

                    aboutPage = new AboutPage()
                                {
                                    Title = "About"
                                };
                    break;
            }

            Children.Add(itemsPage);
            Children.Add(huntersPage);
            Children.Add(aboutPage);

            Title = Children[0].Title;
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            Title = CurrentPage?.Title ?? string.Empty;
        }
    }
}
