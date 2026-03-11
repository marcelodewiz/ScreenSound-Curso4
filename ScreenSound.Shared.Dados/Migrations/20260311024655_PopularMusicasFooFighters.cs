using Microsoft.EntityFrameworkCore.Migrations;
using ScreenSound.Modelos;

#nullable disable

namespace ScreenSound.Migrations
{
    /// <inheritdoc />
    public partial class PopularMusicasFooFighters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData("Musicas", new string[] { "Nome", "AnoLancamento" }, new object[] { "Best of You", 2005 });

            migrationBuilder.InsertData("Musicas", new string[] { "Nome", "AnoLancamento"}, new object[] { "Big me", 1995 });

            migrationBuilder.InsertData("Musicas", new string[] { "Nome", "AnoLancamento"}, new object[] { "These days", 2011 });

            migrationBuilder.InsertData("Musicas", new string[] { "Nome", "AnoLancamento"}, new object[] { "Waiting on a war", 2021 });

            migrationBuilder.Sql(@"update Musicas set ArtistaId = (select top 1 Id from Artistas where Nome = 'Foo Fighters') 
                                    where Nome = 'Best of You' and AnoLancamento = 2005");
            migrationBuilder.Sql(@"update Musicas set ArtistaId = (select top 1 Id from Artistas where Nome = 'Foo Fighters') 
                                    where Nome = 'Big me' and AnoLancamento = 1995");
            migrationBuilder.Sql(@"update Musicas set ArtistaId = (select top 1 Id from Artistas where Nome = 'Foo Fighters') 
                                    where Nome = 'These days' and AnoLancamento = 2011");
            migrationBuilder.Sql(@"update Musicas set ArtistaId = (select top 1 Id from Artistas where Nome = 'Foo Fighters') 
                                    where Nome = 'Waiting on a war' and AnoLancamento = 2021");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
