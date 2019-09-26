namespace MonkeyFestLatam.MasterXFmaps.Interfaces
{
    using System.Threading.Tasks;
    using Models.Response;

    public interface IGoogleMapsApiService
    {
        Task<GoogleDirectionsApiResponse> GetDirections(string originLatitude, string originLongitude, string destinationLatitude, string destinationLongitude);
    }
}
