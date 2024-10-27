using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Domains;
using Repository.Repositories.Classes;
using Repository.Repositories.Interfaces;
using Services.Services.Classes;
using Services.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Register the in-memory cache service
builder.Services.AddMemoryCache();

// Register logging service
builder.Services.AddLogging();

// Register database service
builder.Services.AddDbContext<DataContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("ConString"), action =>
    {
        action.MigrationsAssembly("Repository");
        action.CommandTimeout(30);
    });
});

// Register repositories
builder.Services.AddScoped<IGenericRepository<Products>, ProductsRepository>();
builder.Services.AddScoped<ICalculation, ProductsRepository>();

// Register services
builder.Services.AddScoped<IProductsService, ProductsService>();

var app = builder.Build();

// Apply any pending migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    dbContext.Database.Migrate();
}

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
