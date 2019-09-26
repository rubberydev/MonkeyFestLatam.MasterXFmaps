using System;
using System.Collections.Generic;
using System.Reflection;
using MonkeyFestLatam.MasterXFmaps.Enum;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace MonkeyFestLatam.MasterXFmaps.Views
{
    public partial class MapsStylingPage : ContentPage
    {
        private List<string> StylesList { get; set; }

        public MapsStylingPage()
        {
            InitializeComponent();
            this.StylesList = new List<string>()
            {
                StylesEnum.DARK.ToString(),
                StylesEnum.RETRO.ToString(),
                StylesEnum.NIGHT.ToString(),
                StylesEnum.AUBERGINE.ToString()
            };
            pickerStyles.ItemsSource = this.StylesList;
            this.AddMapStyle(StylesEnum.DARK);
        }

        void AddMapStyle(StylesEnum mapType)
        {
            var assembly = typeof(MapsStylingPage).GetTypeInfo().Assembly;

            System.IO.Stream stream;
            
            switch (mapType)
            {
                case StylesEnum.DARK:
                    stream = assembly.GetManifestResourceStream($"MonkeyFestLatam.MasterXFmaps.MapStyles.MapStyleDark.json");
                    break;
                case StylesEnum.RETRO:
                    stream = assembly.GetManifestResourceStream($"MonkeyFestLatam.MasterXFmaps.MapStyles.MapStyleRetro.json");
                    break;
                case StylesEnum.AUBERGINE:
                    stream = assembly.GetManifestResourceStream($"MonkeyFestLatam.MasterXFmaps.MapStyles.MapStyleAubergine.json");
                    break;
                default:
                    stream = assembly.GetManifestResourceStream($"MonkeyFestLatam.MasterXFmaps.MapStyles.MapStyleNight.json");
                    break;
            }
            
            string styleFile;
            using (var reader = new System.IO.StreamReader(stream))
            {
                styleFile = reader.ReadToEnd();
            }

            map.MapStyle = MapStyle.FromJson(styleFile);
        }

        public void ChangeStyle(object sender, EventArgs e)
        {
            Picker picker = (Picker)sender;
            this.AddMapStyle(((StylesEnum)picker.SelectedIndex));
        }
    }
}
