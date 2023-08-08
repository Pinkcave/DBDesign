using WebAPI.Controllers;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.WebHost.UseUrls(new[] { "http://*:8081" });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:8080");
                          policy.WithOrigins("http://110.42.220.245:8080");
                          policy.WithHeaders("Access-Control-Allow-Origin");
                          policy.WithMethods("POST");
                          policy.WithMethods("OPTION");
                          policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                          
                      });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseStaticFiles();
//app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseCors();
app.MapControllers();

app.Run();
