using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolOfDevs.Migrations
{
    public partial class RenameTyperToType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TyperUser",
                table: "Users",
                newName: "TypeUser");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TypeUser",
                table: "Users",
                newName: "TyperUser");
        }
    }
}
