using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var configuration = app.Configuration;
UsuarioRepository.Init(configuration);

app.MapPost("/usuarios", (Usuario usuario) =>
{
    UsuarioRepository.Add(usuario);
    return Results.Created($"/usuarios/{usuario.Cpf}", usuario.Cpf);
});

app.MapGet("/usuarios/{cpf}", ([FromRoute] string cpf) =>
{
    var usuario = UsuarioRepository.GetBy(cpf);
    if (usuario != null)
    {
        return Results.Ok(usuario);
    }
    else
    {
        return Results.NotFound();
    }

});

app.MapPut("/usuarios", (Usuario usuario) =>
{
    var usuarioSaved = UsuarioRepository.GetBy(usuario.Cpf);
    usuarioSaved.Name = usuario.Name;
    return Results.Ok();
});

app.MapDelete("usuarios/{cpf}", ([FromRoute] string cpf) =>
{
    var usuarioSaved = UsuarioRepository.GetBy(cpf);
    UsuarioRepository.Remove(usuarioSaved);
    return Results.Ok();
});

//Retorna a string de conexão
app.MapGet("/configuration/database", (IConfiguration configuration) =>
{
    return Results.Ok($"{configuration["database:connection"]}/{configuration["database:Port"]}");
});

app.Run();

public static class UsuarioRepository
{
    public static List<Usuario> Usuarios { get; set; } = Usuarios = new List<Usuario>();

    public static void Init(IConfiguration configuration)
    {
        var usuarios = configuration.GetSection("Usuarios").Get<List<Usuario>>();
        Usuarios = Usuarios;
    }

    public static void Add(Usuario usuarios)
    {
        Usuarios.Add(usuarios);
    }

    public static Usuario GetBy(string cpf)
    {
        return Usuarios.FirstOrDefault(p => p.Cpf == cpf);
    }

    public static void Remove(Usuario usuario)
    {
        Usuarios.Remove(usuario);
    }
}

public class Usuario
{
    public string Name { get; set; }
    public string Cpf { get; set; }
}