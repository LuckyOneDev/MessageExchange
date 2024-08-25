using WebApi.DAL;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (connectionString == null)
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}

builder.Services.AddSingleton<ISocketConnectionService>(x => new SocketConnectionService(x.GetRequiredService<ILogger<ISocketConnectionService>>()));
builder.Services.AddSingleton<IMessageRepository>(x => new MessageRepository(connectionString, x.GetRequiredService<ILogger<MessageRepository>>()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Services.GetRequiredService<IMessageRepository>().EnsureCreated();

app.UseWebSockets();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
