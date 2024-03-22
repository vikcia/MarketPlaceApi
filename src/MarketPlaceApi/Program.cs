using Application;
using Infrastructure;
using MarketPlaceApi;
using MarketPlaceApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddJWTAuthenticate(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ErrorHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Market Place API V1");
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();