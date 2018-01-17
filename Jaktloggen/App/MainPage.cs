using Xamarin.Forms;

namespace Jaktloggen
{
    public class MainPage : TabbedPage
    {
        public MainPage()
        {
            Page itemsPage, aboutPage, huntersPage, dogsPage = null;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    itemsPage = new NavigationPage(new HuntsPage())
                                {
                                    Title = "Jakt"
                                };

                    huntersPage = new NavigationPage(new HuntersPage())
                                {
                                    Title = "Jegere"
                                };
                    dogsPage = new NavigationPage(new DogsPage())
                                {
                                    Title = "Hunder"
                                };
                    aboutPage = new NavigationPage(new AboutPage())
                                {
                                    Title = "Info"
                                };
                    itemsPage.Icon = "Gevir.png";
                    huntersPage.Icon = "Jegere.png";
                    dogsPage.Icon = "dog-paw.png";
                    aboutPage.Icon = "Feedback.png";
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

                    aboutPage = new AboutPage()
                    {
                        Title = "Info"
                    };
                    break;
            }

            Children.Add(itemsPage);
            Children.Add(huntersPage);
            Children.Add(dogsPage);
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
