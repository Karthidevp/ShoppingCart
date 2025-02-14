using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Options;
using ShoppingCart;
using ShoppingCart.Interface;
using ShoppingCart.Services;
using Rotativa.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Add services to the container.
builder.Services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<AccountServices>();
builder.Services.AddScoped<ProductServices>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<OrederService>();
// Register the IUserservice with the implementation
builder.Services.AddScoped<IUserservice, UserServices>();
builder.Services.AddScoped<IAccountServices, AccountServices>();
builder.Services.AddScoped<IProductServices, ProductServices>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<Iorderservices, OrederService>();



builder.Services.AddSession(options =>
{
   
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true; // Make the session cookie accessible only by the server
    options.Cookie.IsEssential = true; // Ensure the session cookie is essential
});
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.LoginPath = "/Account/Login";
//        options.LogoutPath = "/Account/Logout";
//        options.AccessDeniedPath = "/Account/AccessDenied";
//    });

var app = builder.Build();
//app.RotativaConfiguration.Setup(app.Environment.WebRootPath, "Rotativa");
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
