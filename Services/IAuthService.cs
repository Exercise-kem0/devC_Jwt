using devC_Jwt.Models;

namespace devC_Jwt.Services
{
    public interface IAuthService
    {
        Task<AuthenModel> RegisterAsync(RegisterModel model);
    }
}
