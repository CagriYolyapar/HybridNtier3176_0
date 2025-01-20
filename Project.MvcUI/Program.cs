using Project.Bll.DependencyResolvers;
using Project.MvcUI.DependencyResolvers;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContextService();
builder.Services.AddRepositoryService();
builder.Services.AddIdentityService();
builder.Services.AddMapperService();
builder.Services.AddManagerService();
builder.Services.AddVmMapperService();

builder.Services.AddDistributedMemoryCache(); //Eger Session kompleks yapılarla calısmak icin Extension metodu ekleme durumuna maruz kalacaksa bu kod projenizin o ilgili Session alanını saglıklı calıstırabilmesi icin gereklidir...

builder.Services.AddSession(x =>
{

    x.IdleTimeout = TimeSpan.FromDays(1);
    x.Cookie.HttpOnly = true;
    x.Cookie.IsEssential = true;

});

builder.Services.ConfigureApplicationCookie(x =>
{
    x.AccessDeniedPath = "/Home/SignIn"; //Authorization hataları icin gidilecek path
    x.LoginPath = "/Home/SignIn"; //Authentication hataları icin gidilecek path
});




WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles(); //wwwroot klasorüne direkt ulasımınızı saglar

app.UseRouting();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "Admin",
    pattern: "{area}/{controller=Category}/{action=Index}/{id?}"
    );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Shopping}/{action=Index}/{id?}");

app.Run();
