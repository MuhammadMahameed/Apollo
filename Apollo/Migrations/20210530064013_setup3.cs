using Microsoft.EntityFrameworkCore.Migrations;

namespace Apollo.Migrations
{
    public partial class setup3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StagetName",
                table: "Artist",
                newName: "StageName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StageName",
                table: "Artist",
                newName: "StagetName");
        }
    }
}
