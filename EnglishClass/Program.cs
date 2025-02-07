using EnglishClass.services.UserService;
using EnglishClass.services.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuração do JWT
var JwtSecret = builder.Configuration.GetSection("JwtSecret");
var secretKey = Encoding.UTF8.GetBytes(JwtSecret["SecretKey"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = JwtSecret["Issuer"],  // Emissor do token
        ValidAudience = JwtSecret["Audience"],  // Público do token
        IssuerSigningKey = new SymmetricSecurityKey(secretKey)  // Chave secreta para validação
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// Registrar os serviços
builder.Services.AddScoped<IUserInterface, UserService>();
builder.Services.AddScoped<AuthUserService>();

// Adiciona o Swagger para documentar a API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adiciona os controladores (API)
builder.Services.AddControllers();

var app = builder.Build();

// Configura o pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();

// Ativar CORS
app.UseCors("AllowAll"); 

// Ativar autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

// Mapear os controladores
app.MapControllers();

app.Run();
