using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortal_New.Migrations
{
    /// <inheritdoc />
    public partial class removedcontactnumberinuser1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            


            migrationBuilder.DropColumn(
               name: "ContactNumber",
               table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                 name: "ContactNumber",
                 table: "Users",
                 type: "nvarchar(15)",
                 maxLength: 15,
                 nullable: false,
                 defaultValue: "");
        }
    }
}
