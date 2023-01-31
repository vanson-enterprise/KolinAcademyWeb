using AutoMapper;
using KA.DataProvider;
using KA.DataProvider.Entities;
using KA.EmailService.Models;
using KA.EmailService.Services;
using KA.Infrastructure.Authen;
using KA.PaymentAPI.CyberSource;
using KA.Repository.Base;
using KA.Service.Address;
using KA.Service.Base;
using KA.Service.Blogs;
using KA.Service.Carts;
using KA.Service.Contacts;
using KA.Service.Courses;
using KA.Service.Mapper;
using KA.Service.Orders;
using KA.Service.Users;
using KAWebHost.Areas.Identity;
using KAWebHost.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// =========================== SERVICE ===============================
// Blazor service
builder.Services.AddRazorPages().AddViewLocalization();
builder.Services.AddServerSideBlazor();
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
    //cfg.AddProfile(new MappingProfile(provider.GetService<IConfiguration>()));
    cfg.AddProfile(new MappingProfile());
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
builder.Services.AddHttpClient();
// DI
builder.Services.AddTransient<IRepository<AppUser>, BaseRepository<AppUser>>();
builder.Services.AddTransient<IRepository<Province>, BaseRepository<Province>>();
builder.Services.AddTransient<IRepository<Contact>, BaseRepository<Contact>>();
builder.Services.AddTransient<IRepository<District>, BaseRepository<District>>();
builder.Services.AddTransient<IRepository<Ward>, BaseRepository<Ward>>();
builder.Services.AddTransient<IRepository<Course>, BaseRepository<Course>>();
builder.Services.AddTransient<IRepository<OfflineCourseStartDate>, BaseRepository<OfflineCourseStartDate>>();
builder.Services.AddTransient<IRepository<OfflineCourseRegister>, BaseRepository<OfflineCourseRegister>>();
builder.Services.AddTransient<IRepository<Lesson>, BaseRepository<Lesson>>();
builder.Services.AddTransient<IRepository<UserLesson>, BaseRepository<UserLesson>>();
builder.Services.AddTransient<IRepository<AppUser>, BaseRepository<AppUser>>();
builder.Services.AddTransient<IRepository<AppRole>, BaseRepository<AppRole>>();
builder.Services.AddTransient<IRepository<AppRole>, BaseRepository<AppRole>>();
builder.Services.AddTransient<IRepository<Cart>, BaseRepository<Cart>>();
builder.Services.AddTransient<IRepository<Order>, BaseRepository<Order>>();
builder.Services.AddTransient<IRepository<CartProduct>, BaseRepository<CartProduct>>();
builder.Services.AddTransient<IRepository<UserCourse>, BaseRepository<UserCourse>>();
builder.Services.AddTransient<IRepository<Blog>, BaseRepository<Blog>>();
builder.Services.AddTransient<IRepository<OrderDetail>, BaseRepository<OrderDetail>>();
builder.Services.AddTransient<IRepository<IdentityUserRole<string>>, BaseRepository<IdentityUserRole<string>>>();

builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<CyberSourceService>();

var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<MailSenderConfiguration>();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<IEmailSender, EmailSender>();
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