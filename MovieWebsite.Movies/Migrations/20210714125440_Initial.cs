using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieWebsite.Movies.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PosterId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OriginalName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullOriginalName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DbComposition",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OriginalName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Genres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AverageRating = table.Column<float>(type: "real", nullable: true),
                    Movie_AverageRating = table.Column<float>(type: "real", nullable: true),
                    Serial_AverageRating = table.Column<float>(type: "real", nullable: true),
                    PosterId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbComposition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullOriginalName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    PosterId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookCharacter",
                columns: table => new
                {
                    BookId = table.Column<long>(type: "bigint", nullable: false),
                    CharacterId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookCharacter", x => new { x.BookId, x.CharacterId });
                    table.ForeignKey(
                        name: "FK_BookCharacter_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookCharacter_DbComposition_BookId",
                        column: x => x.BookId,
                        principalTable: "DbComposition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MovieCharacter",
                columns: table => new
                {
                    MovieId = table.Column<long>(type: "bigint", nullable: false),
                    CharacterId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieCharacter", x => new { x.CharacterId, x.MovieId });
                    table.ForeignKey(
                        name: "FK_MovieCharacter_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovieCharacter_DbComposition_MovieId",
                        column: x => x.MovieId,
                        principalTable: "DbComposition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    DbCompositionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rating_DbComposition_DbCompositionId",
                        column: x => x.DbCompositionId,
                        principalTable: "DbComposition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SerialCharacter",
                columns: table => new
                {
                    SerialId = table.Column<long>(type: "bigint", nullable: false),
                    CharacterId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SerialCharacter", x => new { x.CharacterId, x.SerialId });
                    table.ForeignKey(
                        name: "FK_SerialCharacter_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SerialCharacter_DbComposition_SerialId",
                        column: x => x.SerialId,
                        principalTable: "DbComposition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MoviePerson",
                columns: table => new
                {
                    MovieId = table.Column<long>(type: "bigint", nullable: false),
                    PersonId = table.Column<long>(type: "bigint", nullable: false),
                    MovieId1 = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoviePerson", x => new { x.MovieId, x.PersonId });
                    table.ForeignKey(
                        name: "FK_MoviePerson_DbComposition_MovieId",
                        column: x => x.MovieId,
                        principalTable: "DbComposition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MoviePerson_DbComposition_MovieId1",
                        column: x => x.MovieId1,
                        principalTable: "DbComposition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MoviePerson_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PersonBook",
                columns: table => new
                {
                    BookId = table.Column<long>(type: "bigint", nullable: false),
                    PersonId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonBook", x => new { x.BookId, x.PersonId });
                    table.ForeignKey(
                        name: "FK_PersonBook_DbComposition_BookId",
                        column: x => x.BookId,
                        principalTable: "DbComposition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonBook_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PersonCharacter",
                columns: table => new
                {
                    CharacterId = table.Column<long>(type: "bigint", nullable: false),
                    PersonId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonCharacter", x => new { x.CharacterId, x.PersonId });
                    table.ForeignKey(
                        name: "FK_PersonCharacter_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonCharacter_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PersonSerial",
                columns: table => new
                {
                    SerialId = table.Column<long>(type: "bigint", nullable: false),
                    PersonId = table.Column<long>(type: "bigint", nullable: false),
                    SerialId1 = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonSerial", x => new { x.PersonId, x.SerialId });
                    table.ForeignKey(
                        name: "FK_PersonSerial_DbComposition_SerialId",
                        column: x => x.SerialId,
                        principalTable: "DbComposition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonSerial_DbComposition_SerialId1",
                        column: x => x.SerialId1,
                        principalTable: "DbComposition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonSerial_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookCharacter_CharacterId",
                table: "BookCharacter",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieCharacter_MovieId",
                table: "MovieCharacter",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_MoviePerson_MovieId1",
                table: "MoviePerson",
                column: "MovieId1");

            migrationBuilder.CreateIndex(
                name: "IX_MoviePerson_PersonId",
                table: "MoviePerson",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonBook_PersonId",
                table: "PersonBook",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonCharacter_PersonId",
                table: "PersonCharacter",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonSerial_SerialId",
                table: "PersonSerial",
                column: "SerialId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonSerial_SerialId1",
                table: "PersonSerial",
                column: "SerialId1");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_DbCompositionId",
                table: "Rating",
                column: "DbCompositionId");

            migrationBuilder.CreateIndex(
                name: "IX_SerialCharacter_SerialId",
                table: "SerialCharacter",
                column: "SerialId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookCharacter");

            migrationBuilder.DropTable(
                name: "MovieCharacter");

            migrationBuilder.DropTable(
                name: "MoviePerson");

            migrationBuilder.DropTable(
                name: "PersonBook");

            migrationBuilder.DropTable(
                name: "PersonCharacter");

            migrationBuilder.DropTable(
                name: "PersonSerial");

            migrationBuilder.DropTable(
                name: "Rating");

            migrationBuilder.DropTable(
                name: "SerialCharacter");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "DbComposition");
        }
    }
}
