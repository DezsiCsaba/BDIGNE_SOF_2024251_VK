using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace bdigne_api.Migrations
{
    /// <inheritdoc />
    public partial class SeedUserData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TodoItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoItems", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TodoItems",
                columns: new[] { "Id", "Email", "Password", "Role", "UserName" },
                values: new object[,]
                {
                    { 1, "admin@example.com", "AdminPass123", 0, "admin" },
                    { 2, "dev1@example.com", "DevPass123", 1, "dev1" },
                    { 3, "dev2@example.com", "DevPass123", 1, "dev2" },
                    { 4, "dev3@example.com", "DevPass123", 1, "dev3" },
                    { 5, "dev4@example.com", "DevPass123", 1, "dev4" },
                    { 6, "dev5@example.com", "DevPass123", 1, "dev5" },
                    { 7, "pm1@example.com", "PmPass123", 2, "pm1" },
                    { 8, "pm2@example.com", "PmPass123", 2, "pm2" },
                    { 9, "pm3@example.com", "PmPass123", 2, "pm3" },
                    { 10, "pm4@example.com", "PmPass123", 2, "pm4" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TodoItems");
        }
    }
}
