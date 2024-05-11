using FluentValidation;
using FluentValidation.AspNetCore;
using GokstadHageVennerAPI.Data;
using GokstadHageVennerAPI.Extensions;
using GokstadHageVennerAPI.Mappers;
using GokstadHageVennerAPI.Middleware;
using GokstadHageVennerAPI.Models.DTOs;
using GokstadHageVennerAPI.Models.Entities;
using GokstadHageVennerAPI.Repository;
using GokstadHageVennerAPI.Repository.Interface;
using GokstadHageVennerAPI.Services;
using GokstadHageVennerAPI.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// EXTENSTIONS
builder.AddSwaggerWithBasicAuthentication();

// DI REPOS
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();

// DI SERVICE
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();


// DI MAPPERS
builder.Services.AddScoped<Imapper<Member, MemberDTO>, MemberMapper>();
builder.Services.AddScoped<Imapper<Event, EventDTO>, EventMapper>();
builder.Services.AddScoped<Imapper<Registration, RegistrationDTO>, RegistrationMapper>();
builder.Services.AddScoped<Imapper<Member, MemberSignUpDTO>, MemberSignUpMapper>();

// BASIC AUTHENTICATION
builder.Services.AddScoped<GokstadHageVennerBasicAuthentication>();


// VALIDATORS 
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddFluentValidationAutoValidation(config => config.DisableDataAnnotationsValidation = false);




// EXCEPTION HANDLER
builder.Services.AddTransient<GlobalExceptionMiddleware>();


// DB
builder.Services.AddDbContext<GokstadHageVennerDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 35))));






builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ADD MIDDLEWARES
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<GokstadHageVennerBasicAuthentication>();

app.MapControllers();

app.Run();
