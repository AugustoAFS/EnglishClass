using Dapper;
using EnglishClass.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using BCrypt.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;

namespace EnglishClass.services.UserService
{
    public class UserService : IUserInterface
    {
        private readonly IConfiguration _configuration;
        private readonly string getConnection;
        private readonly string _jwtSecret;

        public UserService(IConfiguration configuration)
        {
            _configuration = configuration;
            getConnection = _configuration.GetConnectionString("DefaultConnection");
            _jwtSecret = _configuration["JwtSecret:SecretKey"];

            // Verifique se a string de conexão foi carregada corretamente
            if (string.IsNullOrEmpty(getConnection))
            {
                throw new InvalidOperationException("A string de conexão não foi configurada.");
            }

            if (string.IsNullOrEmpty(_jwtSecret))
            {
                throw new InvalidOperationException("A chave secreta do JWT não foi configurada.");
            }
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            using (var con = new SqlConnection(getConnection))
            {
                await con.OpenAsync();
                var query = "SELECT * FROM Users";
                var users = await con.QueryAsync<User>(query);
                return users;
            }
        }

        public async Task<User> GetUserById(int userId)
        {
            using (var con = new SqlConnection(getConnection))
            {
                var query = "SELECT * FROM Users WHERE Id = @id";
                return await con.QueryFirstOrDefaultAsync<User>(query, new { id = userId });
            }
        }

        public async Task<IEnumerable<User>> CreateUser(User user)
        {
            using (var con = new SqlConnection(getConnection))
            {
                await con.OpenAsync();

                // Criptografando a senha antes de salvar
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

                var query = @"INSERT INTO dbo.Users (Username, Email, PasswordHash, CreatedAt)
                              VALUES (@Username, @Email, @PasswordHash, GETDATE());
                              SELECT * FROM dbo.Users WHERE Id = SCOPE_IDENTITY();";

                var createdUser = await con.QueryAsync<User>(query, new
                {
                    user.Username,
                    user.Email,
                    PasswordHash = hashedPassword
                });

                return createdUser;
            }
        }

        public Task<User> UpdateUser(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> DeleteUser(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> Login(string username, string password)
        {
            using (var con = new SqlConnection(getConnection))
            {
                await con.OpenAsync();

                var query = "SELECT * FROM Users WHERE Username = @username";
                var user = await con.QueryFirstOrDefaultAsync<User>(query, new { username });

                // Verificar se o usuário existe
                if (user == null)
                {
                    throw new UnauthorizedAccessException("Usuário ou senha inválidos.");
                }

                // Comparar a senha digitada com o hash armazenado no banco
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

                if (!isPasswordValid)
                {
                    throw new UnauthorizedAccessException("Usuário ou senha inválidos.");
                }

                // Criar os claims para o token JWT
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                };

                // Criar chave secreta e credenciais do JWT
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // Criar token JWT com validade de 1 dia
                var token = new JwtSecurityToken(
                    issuer: "EnglishClass",
                    audience: "EnglishClass.UI",
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(1),
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }



        public async Task<User> GetUserByUsernameAsync(string username)
        {
            using (var con = new SqlConnection(getConnection))
            {
                await con.OpenAsync();
                var query = "SELECT * FROM Users WHERE Username = @Username";
                return await con.QueryFirstOrDefaultAsync<User>(query, new { Username = username });
            }
        }
    }
}
