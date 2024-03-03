using EFCore.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Configuration.AddJsonFile("appsettings.json");
var configuration = builder.Configuration;
var jwtIssuer = configuration["JwtIssuer"];
var jwtAudience = configuration["JwtAudience"];
var jwtSecurityKey = configuration["JwtSecurityKey"]!;
var BlockedIPs = configuration["BlockedIPs"];




builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwtOptions =>
{
    jwtOptions.SaveToken = true;
    jwtOptions.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,
        ValidateAudience = false,
        ValidAudience = jwtAudience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecurityKey)),
        ValidateLifetime = true
    };
});
builder.Services.AddHttpClient("JsClient", client => {
    client.BaseAddress = new Uri("https://localhost:7161/");
});


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
    option =>
    { 
        option.LoginPath = "/Home/Login";
        option.LogoutPath = "/Home/Logout";
        option.Cookie.Name = "mahqp.ir";
    }


    );



var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();


app.UseMiddleware<IpBlockMiddleware>(BlockedIPs); // Blocked IP addresses separated by semicolon

//164.90.192.144


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();
