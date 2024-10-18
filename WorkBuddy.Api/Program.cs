using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WorkBuddy.Api.Data;
using WorkBuddy.Api.Interfaces;
using WorkBuddy.Api.Services;
//Installed nu-get packages: Microsoft.entityframework.core.sqlite  and .design
// System.IdentityModel.Tokens.JWT , Microsoft.AspNetCore.Authentication.JwtBearer

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddDbContext<DataContext>(opt=>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));

});
//Adding Cors so i can make requests from the Angular Server (Because its running in another port from the api)
builder.Services.AddCors();
//Add a token service that i will use to provide JWTokens for user authorization
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options=> {
        var tokenKey = builder.Configuration["TokenKey"];
        if (tokenKey == null) { throw new Exception("Token Key Not FOund"); }
        //Check for The signature of the key (we only accept signed key from our server)
        //To check later
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };


    
    });



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Cors used
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200","https://localhost:4200"));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();//First we authenticate 
app.UseAuthorization();// Then Autorization

app.MapControllers();

app.Run();
