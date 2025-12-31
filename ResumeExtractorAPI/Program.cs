using Microsoft.EntityFrameworkCore;
using ResumeExtractorAPI.DataAccess.Context;
using ResumeExtractorAPI.DataAccess.Entities;
using ResumeExtractorAPI.Interfaces.IServices;
using ResumeExtractorAPI.Interfaces.IManagers;
using ResumeExtractorAPI.Interfaces.IRepository;
using ResumeExtractorAPI.Services;
using ResumeExtractorAPI.Managers;
using ResumeExtractorAPI.DataAccess.Repositories;
using ResumeExtractorAPI.Profiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// register AutoMapper BEFORE building the app
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Register Services
builder.Services.AddScoped<IPdfExtractionService, PdfExtractionService>();
builder.Services.AddScoped<IPdfExtractionManager, PdfExtractionManager>();
builder.Services.AddScoped<IResumeManager, ResumeManager>();
builder.Services.AddScoped<IResumeService, ResumeService>();

// Register Repositories
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();


