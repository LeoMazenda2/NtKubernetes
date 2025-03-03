using Microsoft.EntityFrameworkCore;
using NtKubernetes.Model;

namespace NtKubernetes.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
{
}
    public DbSet<Producto> Produtos { get; set; }

}