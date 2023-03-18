using Blazored.LocalStorage;
using cah.blazor.Hubs;
using cah.services.repositories;
using cah.services.services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// add custom services here
builder.Services.AddTransient<ICardsRepository, CardsRepository>();
builder.Services.AddTransient<ICardsService, CardsService>();


// add local storage for 'cookie' type behavior
builder.Services.AddBlazoredLocalStorage(config =>
{
    config.JsonSerializerOptions.WriteIndented = true;
});


// Add compression to the data sent across the wire
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults
                        .MimeTypes.Concat(new[] { "application/octet-stream" });
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();

app.MapHub<ChatHub>("/chathub");
app.MapHub<GameHub>("/gamehub");

app.MapFallbackToPage("/_Host");

app.Run();
