//using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using PRSProject.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<PRSDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("PRSDb")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(); //Products DB (PhotoPath), See wwwroot

//app.UseHttpsRedirection();
//app.UseRewriter(new Microsoft.AspNetCore.Rewrite.RewriteOptions().AddRewrite("", ".xml", true));
app.UseAuthorization();
app.MapControllers();

app.Run();
