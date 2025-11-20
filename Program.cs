using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Конфигурация
builder.Configuration.AddEnvironmentVariables();

// Сервисы
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(24);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddResponseCaching();

var app = builder.Build();

// Настройка портов из переменных окружения (для хостингов)
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Urls.Add($"http://*:{port}");

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = false,
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=3600");
    }
});

app.UseRouting();
app.UseAuthorization();
app.UseSession();
app.UseResponseCaching();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Глобальная обработка ошибок
app.Use(async (context, next) =>
{
    try
    {
        await next();

        if (context.Response.StatusCode == 404)
        {
            context.Request.Path = "/Home/Index";
            await next();
        }
    }
    catch (Exception)
    {
        context.Response.Redirect("/Home/Index");
    }
});

Console.WriteLine($"🚀 Application starting on port {port}");
Console.WriteLine($"📊 Environment: {app.Environment.EnvironmentName}");

app.Run();