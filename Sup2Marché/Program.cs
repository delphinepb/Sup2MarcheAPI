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
//  CREATE
app.MapPut("/createUser", (UserEntity model) =>
{
    var ok = new UserRepo(builder.Configuration).Insert(model);
    return ok != -1 ? Results.Created($"/{ok}", model.id = ok) : Results.Problem(new ProblemDetails { Detail = "L'insert n'a pas marché", Status = 500 });
}).WithName("CREATE User").WithTags("User");

app.MapGet("/getUserByEmail/{email}", (string email) =>
{
    return new UserRepo(builder.Configuration).GetByEmail(email);
}).WithName("READ User by email").WithTags("User");



// -------------------------------------------------
// --- CRUD PRODUIT ---
// -------------------------------------------------
//  CREATE

app.MapPost("/CreatedProduit", (IConfiguration configuration, produitEntity model) =>
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

app.MapPost("/UpdateQuantite", (IConfiguration configuration, produitEntity model) =>
{
    IResult res;
    try
    {
        produitRepo repo = new produitRepo(configuration);
        repo.Update(model);
        res = Results.Ok();
    }
    catch (Exception ex)
    {
        res = Results.BadRequest($"Une erreur s'est produite : {ex.Message}");
    }
    return res;
}).WithTags("Produit");

app.MapDelete("/DeleteProduit/{id}", (IConfiguration configuration, int id) =>
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


// -------------------------------------------------
// --- CRUD CATEGORIE ---
// -------------------------------------------------
//  CREATE

app.MapPost("/CreatedCategorie", (IConfiguration configuration, categorieEntity model) =>
{
    IResult res;
    try
    {
        categorieRepo repo = new categorieRepo(configuration);
        repo.Created(model);
        res = Results.Ok();
    }
    catch (Exception ex)
    {
        res = Results.BadRequest($"Une erreur s'est produite : {ex.Message}");
    }
    return res;
}).WithTags("Catégorie");

app.MapGet("/Categories", (IConfiguration configuration) =>
{
    IResult res;
    try
    {
        categorieRepo repo = new categorieRepo(configuration);
        res = Results.Ok(repo.ReadListCat());
    }
    catch (Exception ex)
    {
        res = Results.BadRequest($"Une erreur s'est produite : {ex.Message}");
    }
    return res;
}).WithTags("Catégorie");

app.MapPost("/Update", (IConfiguration configuration, categorieEntity model) =>
{
    IResult res;
    try
    {
        categorieRepo repo = new categorieRepo(configuration);
        repo.Update(model);
        res = Results.Ok();
    }
    catch (Exception ex)
    {
        res = Results.BadRequest($"Une erreur s'est produite : {ex.Message}");
    }
    return res;
}).WithTags("Catégorie");

app.MapDelete("/DeleteCatégorie/{id}", (IConfiguration configuration, int id) =>
{
    IResult res;
    try
    {
        categorieRepo repo = new categorieRepo(configuration);
        repo.Delete(id);
        res = Results.Ok();
    }
    catch (Exception ex)
    {
        res = Results.BadRequest($"Une erreur s'est produite : {ex.Message}");
    }
    return res;
}).WithTags("Catégorie");

app.Run();


