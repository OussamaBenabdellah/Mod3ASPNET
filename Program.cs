using Microsoft.AspNetCore.Antiforgery;
using Mod3ASPNET;
using System.Reflection.Metadata.Ecma335;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
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

app.MapPost("/file", async (
    HttpContext httpContext) =>
{
    using var memoryStream = new MemoryStream();
    //copyToasync
    await httpContext.Request.Body.CopyToAsync(memoryStream);
    memoryStream.Seek(0, SeekOrigin.Begin);
    File.WriteAllBytes("monimage.png", memoryStream.ToArray());
    return Results.Ok();
});


//avec la nouvel version 
app.MapPost("/formfile", async (IFormFile file, HttpContext httpContext) =>
{
    
    using var memoryStream = new MemoryStream();
    await file.CopyToAsync(memoryStream);
    memoryStream.Seek(0, SeekOrigin.Begin);
    File.WriteAllBytes(file.FileName, memoryStream.ToArray());
    
    return Results.Ok();
})
    .DisableAntiforgery();

app.MapGet("/hello-htm", () =>
{
    return new  HtmlResult("""
<html>
    <body>
        <h1> Hello World! Depuis l'Api en HTML! </h1>
    </body>
</html>
""");
});

app.Run();

