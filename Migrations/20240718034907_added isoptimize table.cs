using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortal_New.Migrations
{
    /// <inheritdoc />
    public partial class addedisoptimizetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "aPIOptimizes",
                columns: table => new
                {
                    apiId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApiName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsOptimised = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aPIOptimizes", x => x.apiId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "aPIOptimizes");
        }
    }
}
