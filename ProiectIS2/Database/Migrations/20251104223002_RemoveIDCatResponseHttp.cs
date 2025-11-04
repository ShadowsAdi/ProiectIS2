using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProiectIS2.Database.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIDCatResponseHttp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CatImgResponses",
                table: "CatImgResponses");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CatImgResponses");

            migrationBuilder.AlterColumn<int>(
                name: "ResponseCode",
                table: "CatImgResponses",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CatImgResponses",
                table: "CatImgResponses",
                column: "ResponseCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CatImgResponses",
                table: "CatImgResponses");

            migrationBuilder.AlterColumn<int>(
                name: "ResponseCode",
                table: "CatImgResponses",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CatImgResponses",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CatImgResponses",
                table: "CatImgResponses",
                column: "Id");
        }
    }
}
