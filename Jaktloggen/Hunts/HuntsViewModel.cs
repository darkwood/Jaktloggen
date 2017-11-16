using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Jaktloggen
{
    public class HuntsViewModel : BaseViewModel
    {
        public ObservableCollection<Hunt> HuntItems { get; set; }
        public Command LoadHuntItemsCommand { get; set; }

        public HuntsViewModel()
        {
            Title = "Jaktloggen";
            HuntItems = new ObservableCollection<Hunt>();
            LoadHuntItemsCommand = new Command(async () => await ExecuteLoadHuntItemsCommand());

            MessagingCenter.Subscribe<NewHuntPage, Hunt>(this, "AddHunt", async (obj, item) =>
            {
                var _item = item as Hunt;
                HuntItems.Add(_item);
                await DataStore.AddItemAsync(_item);
            });
        }

        async Task ExecuteLoadHuntItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                HuntItems.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    HuntItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
