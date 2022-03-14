using k8s;
using LWSSandboxService.Repository;
using LWSSandboxService.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Scoped
builder.Services.AddScoped<UbuntuContainerService>();

// Add Singleton
builder.Services.AddSingleton<KubernetesRepository>();
builder.Services.AddSingleton<AuthorizationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseStaticFiles(new StaticFileOptions
    {
        ServeUnknownFileTypes = true
    });
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger.yml", "SandboxAPI v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();