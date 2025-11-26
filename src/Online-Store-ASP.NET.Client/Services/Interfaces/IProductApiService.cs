using Shared.DTO.Product;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Online_Store_ASP_NET.Client.Services.Interfaces
{
    public interface IProductApiService
    {
        Task<IEnumerable<ProductReadDto>> GetAllAsync();
        Task<ProductReadDto?> GetByIdAsync(int id);
    }
}