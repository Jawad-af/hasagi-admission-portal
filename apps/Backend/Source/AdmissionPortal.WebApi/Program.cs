using AdmissionPortal.WebApi.Configurators;
using AdmissionPortal.WebApi.Injectors;
using Ultimate.Cors.Configurators;
using Ultimate.Cors.Injectors;
using Ultimate.Exceptions.Configurators;
using Ultimate.Exceptions.Injectors;

var builder = WebApplication.CreateBuilder(args);

// Kestrel run on 8000
builder.WebHost.InjectKestrel(builder.Configuration);

// Options
builder.Services.ConfigureJwtOptions(builder.Configuration);

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Services
builder.Services.InjectIdentity(builder.Configuration);
builder.Services.AddOpenApi();
builder.Services.InjectDbContext(builder.Configuration);
builder.Services.InjectMapster();
builder.Services.InjectMediator();
builder.Services.InjectDomainServices();
builder.Services.InjectHybridCaching(builder.Configuration);
builder.Services.AddControllers();
builder.Services.InjectSwagger();
builder.Services.InjectGlobalExceptionMiddleware();
builder.Services.InjectCORS();

var app = builder.Build();
app.ConfigureGlobalExceptionMiddleware();
app.ConfigureCORS();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
