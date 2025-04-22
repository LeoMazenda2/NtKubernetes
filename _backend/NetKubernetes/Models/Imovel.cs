using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace NetKubernetes.Models;

//essa é a classe Inmueble
public class Imovel {
    [Key]
    [Required]
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? Direccao { get; set; }
    [Required]
    [Column(TypeName ="decimal(18.4)")]
    public decimal Preco{ get; set; }
    public string Picture{ get; set; }
    public DateTime DataCriacao { get; set; }
}
