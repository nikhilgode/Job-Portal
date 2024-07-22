using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortal_New.Migrations
{
    /// <inheritdoc />
    public partial class Addedjobinapplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobId",
                table: "Applicationes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Applicationes_JobId",
                table: "Applicationes",
                column: "JobId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applicationes_Jobs_JobId",
                table: "Applicationes",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "JobId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applicationes_Jobs_JobId",
                table: "Applicationes");

            migrationBuilder.DropIndex(
                name: "IX_Applicationes_JobId",
                table: "Applicationes");

            migrationBuilder.DropColumn(
                name: "JobId",
                table: "Applicationes");
        }
    }
}
