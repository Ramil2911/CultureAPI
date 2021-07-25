using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieWebsite.Movies.Migrations
{
    public partial class Initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rating_DbComposition_DbCompositionId",
                table: "Rating");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rating",
                table: "Rating");

            migrationBuilder.DropIndex(
                name: "IX_Rating_DbCompositionId",
                table: "Rating");

            migrationBuilder.DropColumn(
                name: "DbCompositionId",
                table: "Rating");

            migrationBuilder.RenameTable(
                name: "Rating",
                newName: "Ratings");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_CompositionId",
                table: "Ratings",
                column: "CompositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_DbComposition_CompositionId",
                table: "Ratings",
                column: "CompositionId",
                principalTable: "DbComposition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_DbComposition_CompositionId",
                table: "Ratings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_CompositionId",
                table: "Ratings");

            migrationBuilder.RenameTable(
                name: "Ratings",
                newName: "Rating");

            migrationBuilder.AddColumn<long>(
                name: "DbCompositionId",
                table: "Rating",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rating",
                table: "Rating",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_DbCompositionId",
                table: "Rating",
                column: "DbCompositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_DbComposition_DbCompositionId",
                table: "Rating",
                column: "DbCompositionId",
                principalTable: "DbComposition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
