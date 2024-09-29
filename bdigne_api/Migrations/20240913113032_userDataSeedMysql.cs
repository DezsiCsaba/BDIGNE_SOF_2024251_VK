using bdigne_api.Db.Models;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bdigne_api.Migrations
{
    /// <inheritdoc />
    public partial class userDataSeedMysql : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserName", "Email", "Password", "Role" },
                values: new object[,]
                {
                    { 1, "admin", "admin@example.com", "AdminPass123", UserRole.Admin },
                    { 2, "dev1", "dev1@example.com", "DevPass123", UserRole.Dev },
                    { 3, "dev2", "dev2@example.com", "DevPass123", UserRole.Dev },
                    { 4, "dev3", "dev3@example.com", "DevPass123", UserRole.Dev },
                    { 5, "dev4", "dev4@example.com", "DevPass123", UserRole.Dev },
                    { 6, "dev5", "dev5@example.com", "DevPass123", UserRole.Dev },
                    { 7, "pm1", "pm1@example.com", "PmPass123", UserRole.ProjectManager },
                    { 8, "pm2", "pm2@example.com", "PmPass123", UserRole.ProjectManager },
                    { 9, "pm3", "pm3@example.com", "PmPass123", UserRole.ProjectManager },
                    { 10, "pm4", "pm4@example.com", "PmPass123", UserRole.ProjectManager }
                });
        }
        

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "TodoItems");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "TodoItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "TodoItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "TodoItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TodoItems",
                table: "TodoItems",
                column: "Id");
        }
    }
}
