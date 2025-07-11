using Domain.Entities;
using Application.Features.AccountContext.Dto;

namespace Application.Interfaces
{
    public interface IAuthorizeService
    {
        Task<string> GenerateJwtTokenAsync(User user);
        Task<User> ValidateUserAsync(string username, string password);
        Task<LoginDto> LoginAsync(string username, string password);
      

    }
}
