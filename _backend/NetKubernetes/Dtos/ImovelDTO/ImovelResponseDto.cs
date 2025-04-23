namespace NetKubernetes.Dtos.ImovelDTO;

public class ImovelResponseDto
{  
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? Direccao { get; set; }
    public decimal Preco { get; set; }
    public string? Picture { get; set; }
    public DateTime DataCriacao { get; set; }
    public Guid? UsuarioId { get; set; }
}
