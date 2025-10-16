using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rise.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Navigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContentLocation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentLocation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NavigationItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Label = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Icon = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false),
                    Url = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NavigationItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleNavigationItems",
                columns: table => new
                {
                    RoleNavigationItemContentLocation_RoleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoleNavigationItemContentLocation_NavigationItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    RoleNavigationItemContentLocation_ContentLocationId = table.Column<int>(type: "INTEGER", nullable: false),
                    SequenceNr = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    RoleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    NavigationItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    ContentLocationId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleNavigationItems", x => new { x.RoleNavigationItemContentLocation_RoleId, x.RoleNavigationItemContentLocation_NavigationItemId, x.RoleNavigationItemContentLocation_ContentLocationId });
                    table.ForeignKey(
                        name: "FK_RoleNavigationItems_ContentLocation_RoleNavigationItemContentLocation_ContentLocationId",
                        column: x => x.RoleNavigationItemContentLocation_ContentLocationId,
                        principalTable: "ContentLocation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RoleNavigationItems_NavigationItem_RoleNavigationItemContentLocation_NavigationItemId",
                        column: x => x.RoleNavigationItemContentLocation_NavigationItemId,
                        principalTable: "NavigationItem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RoleNavigationItems_Role_RoleNavigationItemContentLocation_RoleId",
                        column: x => x.RoleNavigationItemContentLocation_RoleId,
                        principalTable: "Role",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleNavigationItems_RoleNavigationItemContentLocation_ContentLocationId",
                table: "RoleNavigationItems",
                column: "RoleNavigationItemContentLocation_ContentLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleNavigationItems_RoleNavigationItemContentLocation_NavigationItemId",
                table: "RoleNavigationItems",
                column: "RoleNavigationItemContentLocation_NavigationItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleNavigationItems");

            migrationBuilder.DropTable(
                name: "ContentLocation");

            migrationBuilder.DropTable(
                name: "NavigationItem");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
