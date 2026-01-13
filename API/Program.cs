using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using CView.API.Data;
using CView.API.Repositories;
using CView.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure EPPlus license
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

// Add DbContext
builder.Services.AddDbContext<CViewDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Repositories
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ISprintRepository, SprintRepository>();
builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

// Add Services
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ISprintService, SprintService>();
builder.Services.AddScoped<IOwnerService, OwnerService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IExcelImportService, ExcelImportService>();

// Add Controllers
builder.Services.AddControllers();

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "CView API", Version = "v1" });
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularClient", policy =>
    {
        policy.WithOrigins("http://localhost:4001", "http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CView API v1");
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularClient");
app.UseAuthorization();
app.MapControllers();

// Apply migrations automatically in development
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<CViewDbContext>();
    db.Database.Migrate();
}

app.Run();
