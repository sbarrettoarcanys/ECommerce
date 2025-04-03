using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ECommerce.DataAccess.Data;
using ECommerce.DataAccess.DBInitializer;
using Microsoft.AspNetCore.Identity.UI.Services;
using ECommerce.Utility;
using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.DataAccess.Repository;
using ECommerce.BusinessLogic.IManagers;
using ECommerce.BusinessLogic.Managers;
using ECommerce.Models.Models;
using ECommerce.Models.ViewModels;
using Mapster;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString);
    options.EnableSensitiveDataLogging();


});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<IDBInitializer, DBInitializer>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

#region Managers

builder.Services.AddScoped<ICategoryManager, CategoryManager>();
builder.Services.AddScoped<IProductManager, ProductManager>();
builder.Services.AddScoped<IProductImageManager, ProductImageManager>();
builder.Services.AddScoped<IProductCategoryManager, ProductCategoryManager>();
builder.Services.AddScoped<IShoppingCartManager, ShoppingCartManager>();

#endregion

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

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
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

ConfigureMapster();
SeedDatabase();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();



void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDBInitializer>();
        dbInitializer.Initialize();
    }
}

void ConfigureMapster()
{

    #region model to viewmodel

    TypeAdapterConfig<ProductModel, ProductViewModel>.NewConfig()
        .Map(dest => dest.ProductCategories, src => src.ProductCategories.Adapt<List<ProductCategoryViewModel>>())
        .Map(dest => dest.ProductImages, src => src.ProductImages.Adapt<List<ProductImageViewModel>>())
        .Map(dest => dest.CategoryIds, src => src.ProductCategories.Select(x => x.CategoryId).Adapt<List<int>>());

    TypeAdapterConfig<ProductCategoryModel, ProductCategoryViewModel>.NewConfig()
        .Map(dest => dest.CategoryViewModel, src => src.Category.Adapt<CategoryViewModel>());

    TypeAdapterConfig<ShoppingCartModel, ShoppingCartViewModel>.NewConfig()
        .Map(dest => dest.Price, src => src.Product.DiscountedPrice ?? src.Product.Price);
    #endregion

    #region viewmodel to model

    TypeAdapterConfig<ProductViewModel, ProductModel>.NewConfig()
        .Map(dest => dest.ProductCategories, src => src.ProductCategories.Adapt<List<ProductCategoryModel>>())
        .Map(dest => dest.ProductImages, src => src.ProductImages.Adapt<List<ProductImageModel>>());

    TypeAdapterConfig<ProductCategoryViewModel, ProductCategoryModel>.NewConfig()
        .Map(dest => dest.Category, src => src.CategoryViewModel.Adapt<CategoryModel>());

    #endregion
}