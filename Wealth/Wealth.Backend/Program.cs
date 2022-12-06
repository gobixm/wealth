using DataAccess.Pg;
using Wealth.Backend;
using Wealth.Domain.Securities;
using Wealth.Infrastructure.Migrations;
using Wealth.Infrastructure.Repositories;
using Wealth.Services.Securities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<MigratorHostedService>();
builder.Services.AddPgSql(new PgRepositoryFactoryOptions()
        .RegisterRepository<ISecurityRepository, SecurityRepository>(),
    typeof(Initial_20221128).Assembly);

builder.Services.AddTransient<ISecuritySyncService, SecuritySyncService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGet("/", () => ".!.");

app.MapControllers();

await app.RunAsync();