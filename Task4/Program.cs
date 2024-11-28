using Microsoft.AspNetCore.Antiforgery;
using Microsoft.EntityFrameworkCore.Internal;
using Task4.AuthorizationHelpers;
using Task4.Db;
using Task4.Services;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<AppDbContext>();
builder.Services.AddScoped<RegistrationServices>();

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NDaF5cWWtCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWH5cdHRcR2BcV0d0V0s=");

builder.Services.AddAuthentication(AuthHelper.AUTH_COOKIE).AddCookie(AuthHelper.AUTH_COOKIE, options =>
{
    options.Cookie.Name = AuthHelper.AUTH_COOKIE;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
});

builder.Services.AddAuthorization(options =>
{
    AppPolicies.SetAdminPolicy(options);    
});

builder.Services.AddAntiforgery(o => o.HeaderName = AuthHelper.ANTI_FORGERY);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();



app.Run();
