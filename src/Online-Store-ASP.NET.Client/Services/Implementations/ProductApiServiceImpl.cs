using System.Net.Http;
using System.Net.Http.Json;
using System.Linq;
using Shared.DTO.Product;
using Online_Store_ASP_NET.Client.Services.Interfaces;

namespace Online_Store_ASP_NET.Client.Services.Implementations
{
    public class ProductApiServiceImpl : IProductApiService
    {
        private readonly HttpClient _http;

        public ProductApiServiceImpl(HttpClient http) => _http = http;

        public async Task<IEnumerable<ProductReadDto>> GetAllAsync() =>
            await _http.GetFromJsonAsync<IEnumerable<ProductReadDto>>("product")
            ?? Enumerable.Empty<ProductReadDto>();

        public async Task<ProductReadDto?> GetByIdAsync(int id) =>
            await _http.GetFromJsonAsync<ProductReadDto?>($"product/{id}");
    }
}
