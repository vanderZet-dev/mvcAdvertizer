using Microsoft.EntityFrameworkCore.Migrations;

namespace MvcAdvertizer.Migrations
{
    public partial class alter_advert_add_deleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Adverts",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Adverts");
        }
    }
}
