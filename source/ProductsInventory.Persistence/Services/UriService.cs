using ProductsInventory.Persistence.Interfaces.Services;

namespace ProductsInventory.Persistence.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;

        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }
    }
}
