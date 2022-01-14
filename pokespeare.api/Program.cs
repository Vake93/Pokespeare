using Microsoft.OpenApi.Models;
using Pokespeare;
using Pokespeare.Extensions;

const string PokespeareCorsPolicy = nameof(PokespeareCorsPolicy);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: PokespeareCorsPolicy,
        builder => builder.AllowAnyOrigin());
});

builder.Services.AddConfiguration(builder.Configuration);

builder.Services.AddPokemonRepository();

builder.Services.AddPokemonCache();

builder.Services.AddTranslatorService();

builder.Services.AddEndpointsApiExplorer();

var openapiInfo = builder.Configuration.GetSection("OpenApiInfo").Get<OpenApiInfo>();

builder.Services.AddSwaggerGen(options => options.SwaggerDoc("Pokespeare", openapiInfo));

var app = builder.Build();

app.UseCors(PokespeareCorsPolicy);

app.MapPokemonEndpoints();

app.UseDefaultFiles();

app.UseStaticFiles();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/Pokespeare/swagger.json", "Pokespeare API V1");
});

app.Execute();