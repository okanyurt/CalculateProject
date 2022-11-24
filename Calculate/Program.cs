using Calculate.Data;
using Calculate.Service.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);



var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IOperationService, OperationService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IEndDayService, EndDayService>();
builder.Services.AddScoped<IEndDayReportService, EndDayReportService>();
builder.Services.AddScoped<IOfficeService, OfficeService>();
builder.Services.AddScoped<ICaseService, CaseService>();
builder.Services.AddScoped<IAccountService, AccountService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(option =>
                {
                    option.LoginPath = "/Login/Index";
                });

builder.Services.AddCors(options =>
{
    options.AddPolicy("policy",
                policy =>
                {
                    policy.WithOrigins("http://localhost:5000").AllowAnyMethod().AllowAnyHeader();
                });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Operation}/{action=Index}/{id?}");

app.UseStaticFiles();
app.UseRouting();
app.UseCors("policy");
app.UseAuthorization();
app.MapControllers();

app.Run();

