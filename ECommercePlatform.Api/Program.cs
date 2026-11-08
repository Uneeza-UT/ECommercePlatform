using ECommercePlatform.Api.Middleware;
using ECommercePlatform.Application;
using ECommercePlatform.Identity;
using ECommercePlatform.Infrastructure;
using ECommercePlatform.Persistence;
using Microsoft.OpenApi;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddHttpContextAccessor();


builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token."
    });


    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });

});


var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseStaticFiles();


// Configure the HTTP request pipeline.

app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.ConfigObject.PersistAuthorization = true;

    options.UseResponseInterceptor("""
        (response) => {
            if (
                response.url?.includes("/api/Auth/login") &&
                response.status >= 200 &&
                response.status < 300
            ) {
                const token = response.obj?.token;

                if (token) {
                    window.ui.preauthorizeApiKey("Bearer", token);
                }
            }

            return response;
        }
        """);
});
app.MapOpenApi();


app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
