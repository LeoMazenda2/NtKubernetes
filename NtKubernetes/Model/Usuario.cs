using Microsoft.AspNetCore.Identity;

namespace NtKubernetes.Model;

public class Usuario : IdentityUser
{
    public string? Nome { get; set; }
    public string? Sobrenome { get; set; }
    public string? Telefone { get; set; }
}
