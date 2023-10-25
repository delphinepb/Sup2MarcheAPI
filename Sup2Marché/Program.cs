using Microsoft.AspNetCore.Mvc;
using Sup2Marché.Model;
using Sup2Marché.Repo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Ajoutez le service CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        builder =>
        {
            builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithMethods("PUT", "DELETE", "GET");
        });
});

// Ajouter les services d'autorisation
builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    });
}

app.UseCors(MyAllowSpecificOrigins);


app.UseHttpsRedirection();

// -------------------------------------------------
// --- CRUD USER ---
// -------------------------------------------------
app.MapPost("/userLogin", ([FromBody] UserEntity model) =>
{
    return new UserRepo(builder.Configuration).Login(model);
}).WithName("_User Login").WithName("User Login").WithTags("User");



// -------------------------------------------------
// --- CRUD PRODUIT ---
// -------------------------------------------------
//  CREATE

app.MapPut("/CreatedProduit", (IConfiguration configuration, produitEntity model) =>
{
    IResult res;
    try
    {
        produitRepo repo = new produitRepo(configuration);
        repo.Created(model);
        res = Results.Ok();
    }
    catch (Exception ex)
    {
        res = Results.BadRequest($"Une erreur s'est produite : {ex.Message}");
    }
    return res;
}).WithTags("Produit");

app.MapGet("/Produits", (IConfiguration configuration) =>
{
    IResult res;
    try
    {
        produitRepo repo = new produitRepo(configuration);
        res = Results.Ok(repo.ReadListProd());
    }
    catch (Exception ex)
    {
        res = Results.BadRequest($"Une erreur s'est produite : {ex.Message}");
    }
    return res;
}).WithTags("Produit");
app.MapDelete("/DeleteProduit", (IConfiguration configuration, int id) =>
{
    IResult res;
    try
    {
        produitRepo repo = new produitRepo(configuration);
        repo.Delete(id);
        res = Results.Ok();
    }
    catch (Exception ex)
    {
        res = Results.BadRequest($"Une erreur s'est produite : {ex.Message}");
    }
    return res;
}).WithTags("Produit");

app.Run();


