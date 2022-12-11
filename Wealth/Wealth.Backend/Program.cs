using DataAccess.Pg;
using Moex.Extenstions;
using Wealth.Backend;
using Wealth.Domain.Securities;
using Wealth.Infrastructure.Migrations;
using Wealth.Infrastructure.Repositories;
using Wealth.Services.Securities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<MigratorHostedService>();
builder.Services.AddPgSql(new PgRepositoryFactoryOptions()
        .RegisterRepository<ISecurityRepository, SecurityRepository>(),
    typeof(Initial_20221128).Assembly);
builder.Services.AddMoex();

builder.Services.AddSingleton<ISecuritySyncService, SecuritySyncService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyOrigin());
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();