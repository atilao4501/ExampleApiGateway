using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
                     .AddJsonFile($"ocelot.json", optional: false, reloadOnChange: true)
                     .AddEnvironmentVariables();

// Add services to the container.

builder.Services.AddOcelot(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerForOcelot(builder.Configuration);

var app = builder.Build();


app.UsePathBase("/gateway");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();

}

app.UseSwaggerForOcelotUI(opt =>
{
    opt.DownstreamSwaggerEndPointBasePath = "/gateway/swagger/docs";
    opt.PathToSwaggerGenerator = "/swagger/docs";

});

app.UseAuthorization();

app.UseOcelot().Wait();

app.MapControllers();

app.Run();
