using devC_Jwt.Helpers;
using devC_Jwt.Models;
using devC_Jwt.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Runtime.Intrinsics.X86;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Humanizer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//--------kk=>map a connection or section of  appsettings.json
builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT")); //mapping JWT section 
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDBContext>();
builder.Services.AddDbContext<ApplicationDBContext>(opts => //Mapping String Connection
opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

//*--------kk=>add CORS // CustomizedServices //Mapper //othersss
builder.Services.AddScoped<IAuthService,AuthService>();
//AddTransient: A new instance per service request. Use for stateless or lightweight services.
//AddScoped: A new instance per request or scope. Use for services that need to maintain state per request.
//AddSingleton: A single instance shared across the entire application lifetime. Use for services that need to be globally available and can handle concurrent access.
builder.Services.AddAutoMapper(typeof(Program));

//----kk=> 
builder.Services.AddAuthentication(opts => {
    opts.DefaultAuthenticateScheme= JwtBearerDefaults.AuthenticationScheme;// to avoid adding (AuthenticationSchemes="Bearer") in [Authorize(AuthenticationSchemes="Bearer")] every controller 
    opts.DefaultChallengeScheme= JwtBearerDefaults.AuthenticationScheme; //specifies what happens when an unauthorized request is made
}).AddJwtBearer(op =>
{
    op.RequireHttpsMetadata=false; //to ensure secure communication over HTTPS => in production recommedned to be true 
    op.SaveToken = false; //token should be saved after authentication => false is recommended for stateless authentication (like JWT), where the token is sent with every request.
    op.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true, 
        ValidateIssuer = true, //The issuer is the entity that created and signed the token (e.g., your authentication server).
        ValidateAudience = true, // The audience usually represents the target service the token is intended for.
        ValidateLifetime = true,

        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])) //SymmetricSecurityKey, which takes the JWT:Key value from the configuration file (like appsettings.json)
    };
});


builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); //authentication before authoriztion => authen : u are allowed to use app or not 

app.UseAuthorization(); //author => what u are allowed to do through app 

app.MapControllers();

app.Run();
