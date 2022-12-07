using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/saveusuario", (Usuario usuario) =>
{
    UsuarioRepository.Add(usuario);
});

app.MapGet("/getusuario/{cpf}", ([FromRoute] string cpf) =>
{
    var usuario = UsuarioRepository.GetBy(cpf);
    return usuario;
});

app.MapPut("/editusuario", (Usuario usuario) =>
{
    var usuarioSaved = UsuarioRepository.GetBy(usuario.Cpf);
    usuarioSaved.Name = usuario.Name;
});

app.MapDelete("deleteusuario/{cpf}", ([FromRoute] string cpf) =>
{
    var usuarioSaved = UsuarioRepository.GetBy(cpf);
    UsuarioRepository.Remove(usuarioSaved);
});

app.Run();

public static class UsuarioRepository
{
    public static List<Usuario> Usuarios { get; set; }

    public static void Add(Usuario usuarios)
    {
        if (Usuarios == null)
        {
            Usuarios = new List<Usuario>();
        }
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