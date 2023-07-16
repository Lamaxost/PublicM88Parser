var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(context =>
{
    var data = File.ReadAllText(app.Configuration.GetValue<string>("respondFilePath"));
    context.Response.Headers.ContentType = "application/json";
    return context.Response.WriteAsync(data);
});
app.Run();
