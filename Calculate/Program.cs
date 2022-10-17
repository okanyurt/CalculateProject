using static Calculate.Core.BaseController;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//builder.Services.AddAuthentication(CommonStrings.AuthCookieName)
//    .AddCookie(CommonStrings.AuthCookieName,
//        options =>
//        {
//            options.LoginPath = new PathString("/Login/Index");
//            //options.AccessDeniedPath = new PathString("/Account/Forbidden");

//        });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

