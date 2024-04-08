using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Seatly1.Migrations
{
    /// <inheritdoc />
    public partial class renameMemberRealName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MemberAccount",
                table: "AspNetUsers",
                newName: "MemberRealName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MemberRealName",
                table: "AspNetUsers",
                newName: "MemberAccount");
        }
    }
}
