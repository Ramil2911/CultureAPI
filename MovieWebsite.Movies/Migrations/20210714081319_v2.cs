using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieWebsite.Movies.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookCharacter_Books_BookId",
                table: "BookCharacter");

            migrationBuilder.DropForeignKey(
                name: "FK_BookCharacter_Characters_CharacterId",
                table: "BookCharacter");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieCharacter_Characters_CharacterId",
                table: "MovieCharacter");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieCharacter_Movies_MovieId",
                table: "MovieCharacter");

            migrationBuilder.DropForeignKey(
                name: "FK_MoviePerson_Movies_MovieId",
                table: "MoviePerson");

            migrationBuilder.DropForeignKey(
                name: "FK_MoviePerson_Movies_MovieId1",
                table: "MoviePerson");

            migrationBuilder.DropForeignKey(
                name: "FK_MoviePerson_Persons_PersonId",
                table: "MoviePerson");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonBook_Books_BookId",
                table: "PersonBook");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonBook_Persons_PersonId",
                table: "PersonBook");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonCharacter_Characters_CharacterId",
                table: "PersonCharacter");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonCharacter_Persons_PersonId",
                table: "PersonCharacter");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonSerial_Persons_PersonId",
                table: "PersonSerial");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonSerial_Serials_SerialId",
                table: "PersonSerial");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonSerial_Serials_SerialId1",
                table: "PersonSerial");

            migrationBuilder.DropForeignKey(
                name: "FK_SerialCharacter_Characters_CharacterId",
                table: "SerialCharacter");

            migrationBuilder.DropForeignKey(
                name: "FK_SerialCharacter_Serials_SerialId",
                table: "SerialCharacter");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Serials");

            migrationBuilder.AlterColumn<long>(
                name: "SerialId",
                table: "SerialCharacter",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "CharacterId",
                table: "SerialCharacter",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "SerialId1",
                table: "PersonSerial",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "SerialId",
                table: "PersonSerial",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "PersonId",
                table: "PersonSerial",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "PersonId",
                table: "PersonCharacter",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "CharacterId",
                table: "PersonCharacter",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "PersonId",
                table: "PersonBook",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "BookId",
                table: "PersonBook",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "MovieId1",
                table: "MoviePerson",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "PersonId",
                table: "MoviePerson",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "MovieId",
                table: "MoviePerson",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "MovieId",
                table: "MovieCharacter",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "CharacterId",
                table: "MovieCharacter",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "CharacterId",
                table: "BookCharacter",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "BookId",
                table: "BookCharacter",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "Composition",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Book_OriginalName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Book_PosterId = table.Column<long>(type: "bigint", nullable: true),
                    Book_Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Genres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AverageRating = table.Column<float>(type: "real", nullable: true),
                    OriginalName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PosterId = table.Column<long>(type: "bigint", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Movie_OriginalName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Movie_PosterId = table.Column<long>(type: "bigint", nullable: true),
                    Movie_Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Movie_Genres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Movie_AverageRating = table.Column<float>(type: "real", nullable: true),
                    Person_OriginalName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Person_PosterId = table.Column<long>(type: "bigint", nullable: true),
                    Person_Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Serial_OriginalName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Serial_PosterId = table.Column<long>(type: "bigint", nullable: true),
                    Serial_Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Serial_Genres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Serial_AverageRating = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Composition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rating",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    CompositionId = table.Column<long>(type: "bigint", nullable: false),
                    BookId = table.Column<long>(type: "bigint", nullable: true),
                    MovieId = table.Column<long>(type: "bigint", nullable: true),
                    SerialId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rating_Composition_BookId",
                        column: x => x.BookId,
                        principalTable: "Composition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rating_Composition_CompositionId",
                        column: x => x.CompositionId,
                        principalTable: "Composition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rating_Composition_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Composition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rating_Composition_SerialId",
                        column: x => x.SerialId,
                        principalTable: "Composition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rating_BookId",
                table: "Rating",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_CompositionId",
                table: "Rating",
                column: "CompositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_MovieId",
                table: "Rating",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_SerialId",
                table: "Rating",
                column: "SerialId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookCharacter_Composition_BookId",
                table: "BookCharacter",
                column: "BookId",
                principalTable: "Composition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BookCharacter_Composition_CharacterId",
                table: "BookCharacter",
                column: "CharacterId",
                principalTable: "Composition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieCharacter_Composition_CharacterId",
                table: "MovieCharacter",
                column: "CharacterId",
                principalTable: "Composition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieCharacter_Composition_MovieId",
                table: "MovieCharacter",
                column: "MovieId",
                principalTable: "Composition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MoviePerson_Composition_MovieId",
                table: "MoviePerson",
                column: "MovieId",
                principalTable: "Composition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MoviePerson_Composition_MovieId1",
                table: "MoviePerson",
                column: "MovieId1",
                principalTable: "Composition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MoviePerson_Composition_PersonId",
                table: "MoviePerson",
                column: "PersonId",
                principalTable: "Composition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonBook_Composition_BookId",
                table: "PersonBook",
                column: "BookId",
                principalTable: "Composition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonBook_Composition_PersonId",
                table: "PersonBook",
                column: "PersonId",
                principalTable: "Composition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonCharacter_Composition_CharacterId",
                table: "PersonCharacter",
                column: "CharacterId",
                principalTable: "Composition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonCharacter_Composition_PersonId",
                table: "PersonCharacter",
                column: "PersonId",
                principalTable: "Composition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonSerial_Composition_PersonId",
                table: "PersonSerial",
                column: "PersonId",
                principalTable: "Composition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonSerial_Composition_SerialId",
                table: "PersonSerial",
                column: "SerialId",
                principalTable: "Composition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonSerial_Composition_SerialId1",
                table: "PersonSerial",
                column: "SerialId1",
                principalTable: "Composition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SerialCharacter_Composition_CharacterId",
                table: "SerialCharacter",
                column: "CharacterId",
                principalTable: "Composition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SerialCharacter_Composition_SerialId",
                table: "SerialCharacter",
                column: "SerialId",
                principalTable: "Composition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookCharacter_Composition_BookId",
                table: "BookCharacter");

            migrationBuilder.DropForeignKey(
                name: "FK_BookCharacter_Composition_CharacterId",
                table: "BookCharacter");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieCharacter_Composition_CharacterId",
                table: "MovieCharacter");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieCharacter_Composition_MovieId",
                table: "MovieCharacter");

            migrationBuilder.DropForeignKey(
                name: "FK_MoviePerson_Composition_MovieId",
                table: "MoviePerson");

            migrationBuilder.DropForeignKey(
                name: "FK_MoviePerson_Composition_MovieId1",
                table: "MoviePerson");

            migrationBuilder.DropForeignKey(
                name: "FK_MoviePerson_Composition_PersonId",
                table: "MoviePerson");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonBook_Composition_BookId",
                table: "PersonBook");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonBook_Composition_PersonId",
                table: "PersonBook");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonCharacter_Composition_CharacterId",
                table: "PersonCharacter");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonCharacter_Composition_PersonId",
                table: "PersonCharacter");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonSerial_Composition_PersonId",
                table: "PersonSerial");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonSerial_Composition_SerialId",
                table: "PersonSerial");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonSerial_Composition_SerialId1",
                table: "PersonSerial");

            migrationBuilder.DropForeignKey(
                name: "FK_SerialCharacter_Composition_CharacterId",
                table: "SerialCharacter");

            migrationBuilder.DropForeignKey(
                name: "FK_SerialCharacter_Composition_SerialId",
                table: "SerialCharacter");

            migrationBuilder.DropTable(
                name: "Rating");

            migrationBuilder.DropTable(
                name: "Composition");

            migrationBuilder.AlterColumn<int>(
                name: "SerialId",
                table: "SerialCharacter",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "CharacterId",
                table: "SerialCharacter",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "SerialId1",
                table: "PersonSerial",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SerialId",
                table: "PersonSerial",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "PersonId",
                table: "PersonSerial",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "PersonId",
                table: "PersonCharacter",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "CharacterId",
                table: "PersonCharacter",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "PersonId",
                table: "PersonBook",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "BookId",
                table: "PersonBook",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "MovieId1",
                table: "MoviePerson",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PersonId",
                table: "MoviePerson",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "MovieId",
                table: "MoviePerson",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "MovieId",
                table: "MovieCharacter",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "CharacterId",
                table: "MovieCharacter",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "CharacterId",
                table: "BookCharacter",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "BookId",
                table: "BookCharacter",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Serials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Serials", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_BookCharacter_Books_BookId",
                table: "BookCharacter",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookCharacter_Characters_CharacterId",
                table: "BookCharacter",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieCharacter_Characters_CharacterId",
                table: "MovieCharacter",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieCharacter_Movies_MovieId",
                table: "MovieCharacter",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MoviePerson_Movies_MovieId",
                table: "MoviePerson",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MoviePerson_Movies_MovieId1",
                table: "MoviePerson",
                column: "MovieId1",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MoviePerson_Persons_PersonId",
                table: "MoviePerson",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonBook_Books_BookId",
                table: "PersonBook",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonBook_Persons_PersonId",
                table: "PersonBook",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonCharacter_Characters_CharacterId",
                table: "PersonCharacter",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonCharacter_Persons_PersonId",
                table: "PersonCharacter",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonSerial_Persons_PersonId",
                table: "PersonSerial",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonSerial_Serials_SerialId",
                table: "PersonSerial",
                column: "SerialId",
                principalTable: "Serials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonSerial_Serials_SerialId1",
                table: "PersonSerial",
                column: "SerialId1",
                principalTable: "Serials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SerialCharacter_Characters_CharacterId",
                table: "SerialCharacter",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SerialCharacter_Serials_SerialId",
                table: "SerialCharacter",
                column: "SerialId",
                principalTable: "Serials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
