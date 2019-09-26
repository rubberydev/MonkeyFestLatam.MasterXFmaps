namespace MonkeyFestLatam.MasterXFmaps.Views
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Xamarin.Forms.GoogleMaps;

    public partial class TrackingPage : ContentPage
    {
        public static readonly BindableProperty CalculateCommandProperty =
            BindableProperty.Create(nameof(CalculateCommand), typeof(ICommand), typeof(MainPage), null, BindingMode.TwoWay);

        public ICommand CalculateCommand
        {
            get { return (ICommand)GetValue(CalculateCommandProperty); }
            set { SetValue(CalculateCommandProperty, value); }
        }

        public static readonly BindableProperty UpdateCommandProperty =
          BindableProperty.Create(nameof(UpdateCommand), typeof(ICommand), typeof(MainPage), null, BindingMode.TwoWay);

        public ICommand UpdateCommand
        {
            get { return (ICommand)GetValue(UpdateCommandProperty); }
            set { SetValue(UpdateCommandProperty, value); }
        }

        public TrackingPage()
        {
            InitializeComponent();
            CalculateCommand = new Command<List<Xamarin.Forms.GoogleMaps.Position>>(CalculateDistance);
            UpdateCommand = new Command<Xamarin.Forms.GoogleMaps.Position>(UpdatePosition);
        }

        async void UpdatePosition(Xamarin.Forms.GoogleMaps.Position position)
        {
            if (map.Pins.Count == 1)
                return;

            var cPin = map.Pins.FirstOrDefault();

            if (cPin != null)
            {
                cPin.Position = new Position(position.Latitude, position.Longitude);

                cPin.Icon = (Device.RuntimePlatform == Device.iOS) ? BitmapDescriptorFactory.FromView(new Image()
                {
                    Source = "car.png",
                    WidthRequest = 25,
                    HeightRequest = 25
                }) : BitmapDescriptorFactory.DefaultMarker(Color.Purple);

                await map.MoveCamera(CameraUpdateFactory.NewPosition(new Position(position.Latitude, position.Longitude)));
                var previousPosition = map.Polylines.FirstOrDefault().Positions.FirstOrDefault();
                map.Polylines?.FirstOrDefault()?.Positions?.Remove(previousPosition);
            }
            else
            {
                //END TRIP
                map.Polylines?.FirstOrDefault()?.Positions?.Clear();
            }


        }

        void CalculateDistance(List<Xamarin.Forms.GoogleMaps.Position> list)
        {
            map.Polylines.Clear();
            var polyline = new Xamarin.Forms.GoogleMaps.Polyline();
            foreach (var p in list)
            {
                polyline.Positions.Add(p);

            }
            map.Polylines.Add(polyline);
            var position = new Position(polyline.Positions[0].Latitude, polyline.Positions[0].Longitude);
            map.MoveToRegion(MapSpan.FromCenterAndRadius(position, Xamarin.Forms.GoogleMaps.Distance.FromMiles(0.50f)));

            var pin = new Xamarin.Forms.GoogleMaps.Pin
            {
                Type = PinType.Place,
                Position = new Position(polyline.Positions.First().Latitude, polyline.Positions.First().Longitude),
                Label = "First",
                Address = "First",
                Tag = string.Empty,
                Icon = (Device.RuntimePlatform == Device.iOS) ? BitmapDescriptorFactory.FromView(new Image()
                {
                    Source = "car.png",
                    WidthRequest = 25,
                    HeightRequest = 25
                }) : BitmapDescriptorFactory.DefaultMarker(Color.Purple)

            };
            map.Pins.Add(pin);
            var pin1 = new Xamarin.Forms.GoogleMaps.Pin
            {
                Type = PinType.Place,
                Position = new Position(polyline.Positions.Last().Latitude, polyline.Positions.Last().Longitude),
                Label = "Last",
                Address = "Last",
                Tag = string.Empty
            };
            map.Pins.Add(pin1);
        }

    }
}
