using WebApi.DAL.Repositories.MessageRepository;
using WebApi.Services;
using WebApi.Services.SocketConnectionService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
});

builder.Services.AddSingleton<ISocketConnectionService, SocketConnectionService>();
builder.Services.AddSingleton<IMessageRepository, MessageRepository>();

var app = builder.Build();

// Configure Swagger.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Services.GetRequiredService<IMessageRepository>().EnsureCreated();

// Add websocket support.
app.UseWebSockets();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
