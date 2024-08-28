using Application;
using MyAtelier.DAL;
using Presentation;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddDataAccessLayer(builder.Configuration);
    builder.Services.AddPresentationLayer(builder.Configuration);
    builder.Services.AddBusinessLayer(builder.Configuration);
    builder.Services.AddMvc(config => config.EnableEndpointRouting = false);
}

var app = builder.Build();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();