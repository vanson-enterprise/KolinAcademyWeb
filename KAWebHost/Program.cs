using AutoMapper;
using KA.DataProvider;
using KA.DataProvider.Entities;
using KA.Infrastructure.Authen;
using KA.Repository.Base;
using KA.Service.Address;
using KA.Service.Base;
using KA.Service.Courses;
using KA.Service.Mapper;
using KAWebHost.Areas.Identity;
using KAWebHost.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// =========================== SERVICE ===============================
// Blazor service
builder.Services.AddRazorPages().AddViewLocalization();
builder.Services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });
builder.Services.AddMvc(options => options.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

// DbContext
var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
}, ServiceLifetime.Scoped);

// Mapper
builder.Services.AddSingleton(provider => new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new MappingProfile(provider.GetService<IConfiguration>()));
}).CreateMapper());

// Identity
builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;
    options.User.RequireUniqueEmail = true;
    // Cấu hình về User.
    options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    //options.SignIn.RequireConfirmedAccount = true;
})
.AddErrorDescriber<MultilanguageIdentityErrorDescriber>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();
builder.Services.AddScoped<AuthenticationStateProvider,
              RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/dang-nhap"; // Chuyển hướng nếu truy cập chức năng cần đăng nhập
    options.LogoutPath = $"/dang-xuat";
    options.AccessDeniedPath = $"/"; // Chuyển hướng khi truy cập chức năng ko cho phép
});


// Localize
//builder.Services.AddSingleton(new ResourceManager("DSA.Infrastructure.SharedResource", typeof(SharedResource).GetTypeInfo().Assembly));
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.SetDefaultCulture("vi-VN");
    options.AddSupportedUICultures("en-US", "vi-VN");
    options.AddSupportedCultures("en-US", "vi-VN");

});
// DI
builder.Services.AddTransient<IRepository<AppUser>, BaseRepository<AppUser>>();
builder.Services.AddTransient<IRepository<Province>, BaseRepository<Province>>();
builder.Services.AddTransient<IRepository<District>, BaseRepository<District>>();
builder.Services.AddTransient<IRepository<Ward>, BaseRepository<Ward>>();
builder.Services.AddTransient<IRepository<Course>, BaseRepository<Course>>();

builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<ICourseService, CourseService>();


// ======================= MIDDLEWARE =============================
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//    endpoints.MapBlazorHub();
//    endpoints.MapFallbackToPage("/_Host");
//});

//app.UseHttpsRedirection();
app.UseRequestLocalization();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");


app.Run();