using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProiectIS2.DatabaseMigrations
{
    /// <inheritdoc />
    public partial class CatImgChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Data",
                table: "CatImgResponses",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "MEDIUMBLOB")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Data",
                table: "CatImgResponses",
                type: "MEDIUMBLOB",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
