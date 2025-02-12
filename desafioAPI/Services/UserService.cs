using desafioAPI.Models;
using desafioAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace desafioAPI.Services
{
    public class UserService
    {

        private readonly UserRepository _repo;
        private readonly IConfiguration _configuration;
        private readonly WalletService _walletService;
        private readonly ILogger<UserService> _logger;

        public UserService(UserRepository repo, IConfiguration configuration, WalletService walletService, ILogger<UserService> logger)
        {
            _repo = repo;
            _configuration = configuration;
            _walletService = walletService;
            _logger = logger;
        }

        public async Task<object> Login(string username, string password)
        {


            var user = await _repo.GetByEmailAndPassword(username, password);

            if (user != null)
            {
                var token = GenerateJWTToken();

                object result = new
                {
                    user = user.Id,
                    token
                };

                return result;
            }
            else
            {
                throw new InvalidOperationException("Wrong password or email");
            }


        }

        public async Task<User> CreateAccount(User user)
        {


            if (!await _repo.IsEmailAlreadyRegistered(user.Email))
            {

                var strategy = _repo.CreateStrategy();

                await strategy.ExecuteAsync(async () =>
                {

                    var transaction = await _repo.BeginTransaction();
                    try
                    {

                        var createdUser = await _repo.Create(user);

                        await _walletService.CreateWallet(createdUser.Id);

                        await transaction.CommitAsync();

                        return createdUser;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw new ApplicationException(ex.Message);
                    }

                });


            }
            else
            {
                throw new InvalidOperationException("Email is already registered");
            }
            return null;


        }

        private string GenerateJWTToken()
        {

            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer: issuer,
                                             audience: audience,
                                             expires: DateTime.Now.AddHours(1),
                                             signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;

        }
    }
}
