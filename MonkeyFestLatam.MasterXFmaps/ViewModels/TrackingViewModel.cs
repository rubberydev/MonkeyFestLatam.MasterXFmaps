namespace MonkeyFestLatam.MasterXFmaps.ViewModels
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using MonkeyFestLatam.MasterXFmaps.Helpers;
    using MonkeyFestLatam.MasterXFmaps.Interfaces;
    using MonkeyFestLatam.MasterXFmaps.Services;
    using Xamarin.Forms;

    public class TrackingViewModel
    {
        public ICommand CalculateRouteCommand { get; set; }
        public ICommand UpdatePositionCommand { get; set; }

        public ICommand LoadRouteCommand { get; set; }
        IGoogleMapsApiService googleMapsApi = new GoogleMapsApiService();

        public TrackingViewModel()
        {
            LoadRouteCommand = new Command(async () => await LoadRoute());
            LoadRouteCommand.Execute(null);
        }

        public async Task LoadRoute()
        {
            var positionIndex = 1;

            var googleDirection = await googleMapsApi.GetDirections(
                                  GlobalConfiguration.OriginLatitude, GlobalConfiguration.OriginLongitude,
                                  GlobalConfiguration.DestinationLatitude, GlobalConfiguration.DestinationLongitude);

            if (googleDirection.Routes != null && googleDirection.Routes.Count > 0)
            {
                var positions = (Enumerable.ToList(PolylineHelper.Decode(googleDirection.Routes.First().OverviewPolyline.Points)));
                CalculateRouteCommand.Execute(positions);

                //Location tracking simulation
                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    if (positions.Count > positionIndex)
                    {
                        UpdatePositionCommand.Execute(positions[positionIndex]);
                        positionIndex++;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });
            }

        }
    }
}