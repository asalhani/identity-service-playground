var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthentication("Bearer").AddIdentityServerAuthentication("Bearer", options =>
{
    options.ApiName = "ApiOne"; // should match to name of one in configuration >> ApiResources
    options.Authority = "https://localhost:7136";
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();