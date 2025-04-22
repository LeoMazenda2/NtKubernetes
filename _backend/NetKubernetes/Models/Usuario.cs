using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
namespace NetKubernetes.Models;

public class Usuario : IdentityUser
{
    public string? Nome { get; set; }
    public string? Apelido { get; set; }
    public string? Telefone { get; set; }
}
