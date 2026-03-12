using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ScreenSound.Modelos;
using ScreenSound.Shared.Dados.Modelos;
using ScreenSound.Shared.Modelos.Modelos;

namespace ScreenSound.Banco;
public class ScreenSoundContext: IdentityDbContext<PessoaComAcesso, PerfilDeAcesso, int>
{
    public DbSet<Artista> Artistas { get; set; }
    public DbSet<Musica> Musicas { get; set; }
    public DbSet<Genero> Generos { get; set; }

    public ScreenSoundContext()
    {
        LoadEnvironmentVariables();
    }
    public ScreenSoundContext(DbContextOptions options) : base(options)
    {
        LoadEnvironmentVariables();
    }

    private static void LoadEnvironmentVariables()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var envPath = Path.Combine(currentDirectory, ".env");

        if (!File.Exists(envPath))
        {
            var solutionRoot = Directory.GetParent(currentDirectory)?.Parent?.Parent?.FullName;
            if (solutionRoot != null)
            {
                envPath = Path.Combine(solutionRoot, ".env");
            }
        }

        if (File.Exists(envPath))
        {
            DotNetEnv.Env.Load(envPath);
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }

        var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

        optionsBuilder
            .UseSqlServer(connectionString)
            .UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Musica>()
            .HasMany(c => c.Generos)
            .WithMany(c => c.Musicas);
    }

}
