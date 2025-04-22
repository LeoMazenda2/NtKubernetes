using Microsoft.AspNetCore.Identity;
using NetKubernetes.Models;
namespace NetKubernetes.Data;

public class LoadDatabase
{
    public static async Task InserirDados(AppDbContext context, UserManager<Usuario> usuarioManager)
    {
        if (!usuarioManager.Users.Any())
        {
            var usuario = new Usuario
            {
                Nome = "Leonildo",
                Apelido = "Vivaldo",
                Email = "leonildo.mazenda@hotmail.com",
                UserName = "leonildo.mazenda",
                Telefone = "923923923"
            };

            await usuarioManager.CreateAsync(usuario, "Leonildo123#");
        }

        if (!context.Imoveis.Any())
        {
            context.Imoveis!.AddRange(
                new Imovel
                {
                    Nome = "Casa de Praia",
                    Direccao = "Luanda. Mussulo 234a",
                    Preco = 45500M,
                    DataCriacao = DateTime.Now
                },

                new Imovel
                {
                    Nome = "Casa de Campo",
                    Direccao = "Luanda. Viana-Kalumbo 54654",
                    Preco = 25000M,
                    DataCriacao = DateTime.Now
                }
            );
        }
    }  
}