using System.Reflection;
using Infrastructure.Data;
using Repositories.Interfaces;
using Repositories.Implementations;
using Services.Interfaces;
using Services.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;

// Added: bring Shared and User service namespaces to register their implementations
using Shared.Repositories;
using Shared.Services;
using Services.UserService;

var builder = WebApplication.CreateBuilder(args);

// PostgreSQL configuration
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Controllers
builder.Services.AddControllers();

// Swagger connection
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

// Repositories
builder.Services.AddScoped<ICartRepository, CartRepositoryImpl>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepositoryImpl>();
builder.Services.AddScoped<IProductRepository, ProductRepositoryImpl>();

// Added repositories (wishlist, wishlist items, user)
builder.Services.AddScoped<IWishlistRepository, WishlistRepositoryImpl>();
builder.Services.AddScoped<IWishlistItemRepository, WishlistItemRepositoryImpl>();
builder.Services.AddScoped<IUserRepository, UserRepositoryImpl>();

// Services
builder.Services.AddScoped<ICartService, CartServiceImpl>();
builder.Services.AddScoped<ICategoryService, CategoryServiceImpl>();
builder.Services.AddScoped<IProductService, ProductServiceImpl>();

// Added services (wishlist, wishlist items, user)
builder.Services.AddScoped<IWishlistService, WishlistServiceImpl>();
builder.Services.AddScoped<IWishlistItemService, WishlistItemServiceImpl>();
builder.Services.AddScoped<IUserService, UserServiceImpl>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// ВАЖНО: порядок строго такой
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapControllers();

// ВАЖНО: fallback для SPA
app.MapFallbackToFile("index.html");

app.Run();

