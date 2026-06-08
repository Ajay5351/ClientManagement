using ClientManagement.Data;
using ClientManagement.Models;
using ClientManagement.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<ClientManagementDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ClientDb")));

builder.Services.AddIdentity<ApplicationModel, IdentityRole>()
    .AddEntityFrameworkStores<ClientManagementDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddTransient<IClientRepository, ClientRepository>();
builder.Services.AddTransient<IAccountRepository, AccountRepository>();

builder.Services.AddAutoMapper(typeof(Program));

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
