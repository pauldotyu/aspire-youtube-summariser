using AspireYouTubeSummariser.WebApp.Clients;
using AspireYouTubeSummariser.WebApp.Components;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddRedisOutputCache("cache");
builder.AddAzureQueueService("queue");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<IApiAppClient, ApiAppClient>(client =>
{
    //client.BaseAddress = new Uri("http://localhost:5050");
    client.BaseAddress = new Uri("http://apiapp");
});
builder.Services.AddScoped<IQueueServiceClientWrapper, QueueServiceClientWrapper>();

var app = builder.Build();

app.MapDefaultEndpoints();
app.UseOutputCache();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
