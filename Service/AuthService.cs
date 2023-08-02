using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PaymentGatewayAPI.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PaymentGatewayAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;

        public AuthService(DataContext dataContext,IConfiguration configuration)
        {
            _dataContext = dataContext;
            _configuration = configuration;
        }
        public async  Task<ServiceResponse<string>> Login(string MerchantName, string Password)
        {
            var response = new ServiceResponse<string>();
            var Merchant = await _dataContext.Merchants.FirstOrDefaultAsync
                (m => m.Name.ToLower().Equals(MerchantName.ToLower()));
            if (Merchant == null)
            {
                response.IsSuccess = false;
                response.Message = "User not found";
            }
            else if(!verifyPassword(Password, Merchant.PasswordHash, Merchant.PasswordSalt))
            {
                response.IsSuccess = false;
                response.Message = "Invalid Login";
            }
            else
            {
                response.IsSuccess = true;
                response.Data = GenerateToken(Merchant);
            }
            return response;
        }

        public async Task<ServiceResponse<int>> Register(Merchant Merchant, string Password)
        {
            var serviceResponse = new ServiceResponse<int>();
            if (await MerchantExits(Merchant.Name))
            {
                serviceResponse.IsSuccess = false;
                serviceResponse.Message = "Merchant Name already exists";
                return serviceResponse;
            }

            CreatePasswordHash(Password, out byte[] passwordHash, out byte[] passwordSalt);

            Merchant.PasswordHash = passwordHash;
            Merchant.PasswordSalt = passwordSalt;

            _dataContext.Merchants.Add(Merchant);
            await _dataContext.SaveChangesAsync();
           
            serviceResponse.Data = Merchant.Id;
            return serviceResponse;

        }

        private async Task<bool> MerchantExits(string MerchantName)
        {
            if(await _dataContext.Merchants.AnyAsync(m => m.Name.ToLower() == MerchantName.ToLower()))
            {
                return true;
            }
            return false;
        }

        private void CreatePasswordHash(string password , out byte[] passwordHash,out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool verifyPassword(string password,  byte[] passwordHash,  byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var HashValue = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return HashValue.SequenceEqual(passwordHash);
            }
        }

        private string GenerateToken(Merchant Merchant)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, Merchant.Id.ToString()),
                new Claim(ClaimTypes.Name, Merchant.Name)
            };

            var SecretKey = _configuration.GetSection("AppSettings:Token").Value!;

            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(SecretKey));

            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials,
            };

            JwtSecurityTokenHandler tokenHandler =  new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDesc);
            return tokenHandler.WriteToken(token);           
        }
    }
}
