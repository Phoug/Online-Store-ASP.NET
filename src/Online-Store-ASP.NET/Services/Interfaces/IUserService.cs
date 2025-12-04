using Online_Store_ASP_NET.Shared.DTO.User;

namespace Services.UserService
{
    public interface IUserService
    {
        Task<IEnumerable<UserReadDto>> GetAllAsync();

        Task<UserReadDto?> LoginAsync(string email, string password);

        Task<UserReadDto?> GetByIdAsync(int id);

        Task<UserReadDto> CreateAsync(UserCreateDto dto);

        Task<UserReadDto?> UpdateAsync(int id, UserUpdateDto dto);

        Task<bool> DeleteAsync(int id);

        Task<UserReadDto?> GetByUsernameAsync(string username);
    }
}
