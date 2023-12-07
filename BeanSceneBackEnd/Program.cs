using BeanSceneBackEnd.Models;
using BeanSceneBackEnd.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<BeanSceneDataBaseSettings>(builder.Configuration.GetSection("BeanSceneDataBaseSettings"));


//add CORS policy - cross origin resource sharing 

builder.Services.AddCors(o => o.AddPolicy("BeanSceneCorsPolicy", builder => {
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    ;
}));


//adding basic authtication     
builder.Services.AddAuthentication()
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", options => { });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("BasicAuthentication", new AuthorizationPolicyBuilder("BasicAuthentication").RequireAuthenticatedUser().Build());


});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseCors("BeanSceneCorsPolicy");

app.UseHttpsRedirection();
//https://localhost:7044;

app.UseAuthorization();

app.MapControllers();

app.Run();
