
using System;
using System.Windows.Input;
using Jaktloggen.Cells;
using Xamarin.Forms;

namespace Jaktloggen 
{
    public class LogPageCode : ContentPage
    {
        private readonly LogViewModel log;

        public LogPageCode(LogViewModel logViewModel)
        {
            BindingContext = log = logViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await log.OnAppearing();

            Content = new JTableView
            {
                HasUnevenRows = true,
                Intent = TableIntent.Settings,
                Root = GetTableContent()
            };
        }

        private TableRoot GetTableContent()
        {
            var root = new TableRoot();
            var section = new TableSection();

            section.Add(new ImageHeaderCell
            {
                HeightRequest = "200",
                ImagePath = log.ImageFilename,
                Command = log.ImageCommand
            });
            section.Add(new ItemPickerCell
            {
                Text = "Art",
                Items = log.PickerSpecies,
                SelectedItem = log.SelectedPickerSpecie,
                Command = log.SpeciesCommand
            });
            section.Add(new CountCell
            {
                Text = "Treff",
                Count = log.Hits,
                Command = log.HitsCommand
            });
            section.Add(new CountCell
            {
                Text = "Skudd",
                Count = log.Shots,
                Command = log.ShotsCommand
            });
            section.Add(new CountCell
            {
                Text = "Sett",
                Count = log.Observed,
                Command = log.ObservedCommand
            });

            section.Add(new MapCell
            {
                Text = "Posisjon",
                Latitude = log.Latitude,
                Longitude = log.Longitude,
                Command = log.PositionCommand
            });

            DateCell dateCell = new DateCell
            {
                Text = "Dato",
                Command = log.DateCommand, 
                DateField = "Date"
            };
            dateCell.SetBinding(DateCell.DateProperty, "Date", BindingMode.TwoWay);
            section.Add(dateCell);

            TimeCell timeCell = new TimeCell
            {
                Text = "Tidspunkt",
                Command = log.TimeCommand
            };
            timeCell.SetBinding(TimeCell.TimeProperty, "Time", BindingMode.TwoWay);
            section.Add(timeCell);


            section.Add(new ExtendedTextCell
            {
                Command = log.NotesCommand,
                Text = "Notater",
                Text2 = log.Notes
            });

            var customSection = new TableSection();


            TryAddCustomField(customSection, "Gender", "Kjønn", log.Gender, log.GenderCommand);
            TryAddCustomField(customSection, "Weight", "Vekt", log.Weight + " kg", log.WeightCommand);
            TryAddCustomField(customSection, "ButchWeight", "Slaktevekt", log.ButchWeight + " kg", log.ButchWeightCommand);
            TryAddCustomField(customSection, "WeaponType", "Våpentype", log.WeaponType, log.WeaponTypeCommand);
            TryAddCustomField(customSection, "Tags", "Tagger", log.Tags.ToString(), log.TagsCommand);
            TryAddCustomField(customSection, "Gender", "Kjønn", log.Gender, log.GenderCommand);
            TryAddCustomField(customSection, "Weather", "Vær", log.Weather, log.WeatherCommand);

            root.Add(section);
            root.Add(customSection);
            return root;
        }

        private void TryAddCustomField(TableSection section, string key, string label, string value, ICommand command)
        {
            if (log.CustomFields.Contains(key))
            {
                section.Add(new ExtendedTextCell
                {
                    Text = label,
                    Text2 = value,
                    Command = command
                });
            }
        }
    }
}

