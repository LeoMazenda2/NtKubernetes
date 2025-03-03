using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NtKubernetes.Model;

public class Producto
{
    [Key]
    [Required]
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? Drescicao { get; set; }
    [Required]
    [Column(TypeName = "decimal(18,4)")]
    public decimal Preco { get; set; }
    public string? Imagem { get; set; }
    public DateTime? DataCriacao { get; set; }
}
