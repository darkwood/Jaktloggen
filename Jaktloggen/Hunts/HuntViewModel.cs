using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Jaktloggen.InputViews;
using Xamarin.Forms;

namespace Jaktloggen
{
    public class HuntViewModel : BaseViewModel
    {
        public Hunt Item { get; set; }
        string infoMessage;
        public string InfoMessage
        {
            get { return infoMessage; }
            set { SetProperty(ref infoMessage, value); }
        }
        public ICommand LocationCommand { protected set; get; }
        public ICommand DateFromCommand { protected set; get; }
        public ICommand DateToCommand { protected set; get; }
        public ICommand NotesCommand { protected set; get; }

        public HuntViewModel(Hunt item, INavigation navigation)
        {
            Navigation = navigation;
            if(string.IsNullOrEmpty(item.ID)){
                item = CreateNewItem();
            }
            Title = item?.Location;
            Item = item.DeepClone();
            CreateCommands(item);
        }

        private Hunt CreateNewItem()
        {

            Hunt newHunt = new Hunt()
            {
                DateFrom = DateTime.Today,
                DateTo = DateTime.Today,

            };
            return newHunt;
        }

        public async Task OnAppearing()
        {
            if (string.IsNullOrEmpty(Item.ID))
            {
                await SetPositionAsync();
            }
        }

        private async Task SetPositionAsync()
        {
            InfoMessage = "Henter din posisjon...";
            await Task.Delay(2000);
            InfoMessage = "Posisjon funnet!";

            Item.Latitude = "new lat";
            Item.Longitude = "new long";
        }

        private void CreateCommands(Hunt item)
        {
            LocationCommand = new Command(async () =>
            {
                await Navigation.PushAsync(new InputEntry("Sted", Item.Location, (InputEntry obj) => {
                    Item.Location = obj.Value;
                }));
            });
            DateFromCommand = new Command(async () =>
            {
                await Navigation.PushAsync(new InputDate("Dato fra", Item.DateFrom, (InputDate obj) => {
                    Item.DateFrom = obj.VM.Value;
                    if (Item.DateTo == null || Item.DateTo < Item.DateFrom){
                        Item.DateTo = Item.DateFrom;
                    }
                }));
            });
            DateToCommand = new Command(async () =>
            {
                await Navigation.PushAsync(new InputDate("Dato til", Item.DateTo, (InputDate obj) => {
                    Item.DateTo = obj.VM.Value;
                    if (Item.DateFrom == null || Item.DateFrom > Item.DateTo)
                    {
                        Item.DateFrom = Item.DateTo;
                    }
                }));
            });
            NotesCommand = new Command(async () =>
            {
                var inputView = new InputEntry("Notater", Item.Notes, (InputEntry obj) =>
                {
                    Item.Notes = obj.Value;
                });
                inputView.Multiline = true;
                await Navigation.PushAsync(inputView);
            });
        }
    }
}
