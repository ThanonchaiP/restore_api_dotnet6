using Microsoft.EntityFrameworkCore;
using EcomApi.Data;
using EcomApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

//CORS
builder.Services.AddCors();

builder.Services.AddDbContext<APIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("APIContext") ?? throw new InvalidOperationException("Connection string 'APIContext' not found.")));
builder.Services.AddTransient<DbInitializer>();
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(options => options
    .AllowAnyHeader()
    .AllowAnyMethod()
    .WithOrigins("http://localhost:3000").AllowCredentials()
//.AllowAnyOrigin()
);

//seed
if (args.Length == 1 && args[0].ToLower() == "seeddata")
{
    SeedData(app);
}

void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory!.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<DbInitializer>()!;
        service.Seed();
    }

}

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
