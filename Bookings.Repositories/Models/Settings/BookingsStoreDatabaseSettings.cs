namespace Bookings.Repositories.Models.Settings
{
    public class BookingsStoreDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string BookingsCollectionName { get; set; } = null!;

        public string HotelsCollectionName { get; set; } = null!;

        public string ClientsCollectionName { get; set; } = null!;
    }
}
