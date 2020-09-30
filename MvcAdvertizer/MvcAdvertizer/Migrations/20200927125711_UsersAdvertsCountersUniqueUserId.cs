using Microsoft.EntityFrameworkCore.Migrations;

namespace MvcAdvertizer.Migrations
{
    public partial class UsersAdvertsCountersUniqueUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UsersAdvertsCounters_UserId",
                table: "UsersAdvertsCounters",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UsersAdvertsCounters_UserId",
                table: "UsersAdvertsCounters");
        }
    }
}
