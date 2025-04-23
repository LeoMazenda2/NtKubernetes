using Microsoft.IdentityModel.Tokens;
using NetKubernetes.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NetKubernetes.Token;

public class JwtGeradorToken : IJwtGeradorToken
{
    public string GerarToken(Usuario usuario)
    {
        var claims = new List<Claim> {
           new Claim(JwtRegisteredClaimNames.NameId, usuario.UserName!),
           new Claim("userId", usuario.Id!),
           new Claim("email", usuario.Email!),
       };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Minha palavra Secreta"));
        var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescripton = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = credenciais
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescripton);

        return tokenHandler.WriteToken(token);
    }
}
