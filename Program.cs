using Microsoft.AspNetCore.Antiforgery;
using Mod3ASPNET;
using Mod3ASPNET.Filter;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Mod3ASPNET.Doc;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Service Versioning 

//builder.Services.AddApiVersioning();

//swager Maj
builder.Services.AddApiVersioning()
    .AddApiExplorer(opt =>
    {
        opt.GroupNameFormat = "'v'VVV";
        opt.SubstituteApiVersionInUrl = true;
    })
    .EnableApiVersionBinding();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>();
#endregion

var app = builder.Build();

#region verioning swagger


var versionset = app.NewApiVersionSet()
    .HasApiVersion(new Asp.Versioning.ApiVersion(1))
     .HasApiVersion(new Asp.Versioning.ApiVersion(2))
     .ReportApiVersions()
     .Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(conf =>
    {
        var versions = app.DescribeApiVersions();

        foreach (var version in versions)
        {
            var url = $"/swagger/{version.GroupName}/swagger.json";
            conf.SwaggerEndpoint(url,  version.GroupName);
}
    });
}
#endregion

#region Gérer le streaming de données


app.MapPost("/file", async (
    HttpContext httpContext) =>
{
    using var memoryStream = new MemoryStream();
    //copyToasync
    await httpContext.Request.Body.CopyToAsync(memoryStream);
    memoryStream.Seek(0, SeekOrigin.Begin);
    File.WriteAllBytes("monimage.png", memoryStream.ToArray());
    return Results.Ok();
})
    .AddEndpointFilter<LoggingFilter>()
    .WithApiVersionSet(versionset)
        .MapToApiVersion(new Asp.Versioning.ApiVersion(2.0));
#endregion

#region Uploader un fichier avec IFormFile


//avec la nouvel version 
app.MapPost("/formfile", async (IFormFile file, HttpContext httpContext) =>
{
    
    using var memoryStream = new MemoryStream();
    await file.CopyToAsync(memoryStream);
    memoryStream.Seek(0, SeekOrigin.Begin);
    File.WriteAllBytes(file.FileName, memoryStream.ToArray());
    
    return Results.Ok();
})
    .DisableAntiforgery()
    .WithApiVersionSet(versionset)
        .MapToApiVersion(new Asp.Versioning.ApiVersion(2.0));
#endregion

#region Personnaliser les résultats de l'API 



app.MapGet("/hello-htm", () =>
{
    return new HtmlResult("""
<html>
    <body>
        <h1> Hello World! Depuis l'Api en HTML! </h1>
    </body>
</html>
""");
})
    .WithApiVersionSet(versionset)
        .MapToApiVersion(new Asp.Versioning.ApiVersion(2.0));
#endregion

#region Versionner l'api 
//NewApiVersionSet()
//c'est une methode d'extention qui vas permetre
//de définir l'ensemble des version de l'api 
// et de dire sur les endpoint quel version utiliser 
//var versionset = app.NewApiVersionSet()
//    .HasApiVersion(new Asp.Versioning.ApiVersion (1.0))
//     .HasApiVersion(new Asp.Versioning.ApiVersion(2.0))
//     .ReportApiVersions()
//     .Build();

//app.MapGet("/info", () => "info v1.0")
//    .AddEndpointFilter<LoggingFilter>()
//    .WithApiVersionSet(versionset)
//    //signale que il est associer a la version 1.0
//    .MapToApiVersion(new Asp.Versioning.ApiVersion(1.0));

//app.MapGet("/info", () => "info v2.0")
//    .WithApiVersionSet(versionset)
//        .MapToApiVersion(new Asp.Versioning.ApiVersion(2.0));
#endregion

#region versioning swagger


app.MapGet("v{version:apiVersion}/info", () => "info v1.0")
    .AddEndpointFilter<LoggingFilter>()
    .WithApiVersionSet(versionset)
    //signale que il est associer a la version 1.0
    .MapToApiVersion(new Asp.Versioning.ApiVersion(1));

app.MapGet("v{version:apiVersion}/info", () => "info v2.0")
    .WithApiVersionSet(versionset)
        .MapToApiVersion(new Asp.Versioning.ApiVersion(2));
#endregion
app.Run();

