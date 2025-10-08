using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rise.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentActivitiesConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentActivities_Locations_LocationId",
                table: "StudentActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentActivities_StudentClubs_StudentClubId",
                table: "StudentActivities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentClubs",
                table: "StudentClubs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentActivities",
                table: "StudentActivities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Locations",
                table: "Locations");

            migrationBuilder.RenameTable(
                name: "StudentClubs",
                newName: "StudentClub");

            migrationBuilder.RenameTable(
                name: "StudentActivities",
                newName: "StudentActivity");

            migrationBuilder.RenameTable(
                name: "Locations",
                newName: "Location");

            migrationBuilder.RenameIndex(
                name: "IX_StudentActivities_StudentClubId",
                table: "StudentActivity",
                newName: "IX_StudentActivity_StudentClubId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentActivities_LocationId",
                table: "StudentActivity",
                newName: "IX_StudentActivity_LocationId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "StudentClub",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "StudentClub",
                type: "INTEGER",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "StudentClub",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "StudentActivity",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "StudentActivity",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "StudentActivity",
                type: "INTEGER",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "StudentActivity",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "StudentActivity",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "StudentActivity",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Location",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Location",
                type: "INTEGER",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Location",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentClub",
                table: "StudentClub",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentActivity",
                table: "StudentActivity",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Location",
                table: "Location",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentActivity_Location_LocationId",
                table: "StudentActivity",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentActivity_StudentClub_StudentClubId",
                table: "StudentActivity",
                column: "StudentClubId",
                principalTable: "StudentClub",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentActivity_Location_LocationId",
                table: "StudentActivity");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentActivity_StudentClub_StudentClubId",
                table: "StudentActivity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentClub",
                table: "StudentClub");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentActivity",
                table: "StudentActivity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Location",
                table: "Location");

            migrationBuilder.RenameTable(
                name: "StudentClub",
                newName: "StudentClubs");

            migrationBuilder.RenameTable(
                name: "StudentActivity",
                newName: "StudentActivities");

            migrationBuilder.RenameTable(
                name: "Location",
                newName: "Locations");

            migrationBuilder.RenameIndex(
                name: "IX_StudentActivity_StudentClubId",
                table: "StudentActivities",
                newName: "IX_StudentActivities_StudentClubId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentActivity_LocationId",
                table: "StudentActivities",
                newName: "IX_StudentActivities_LocationId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "StudentClubs",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "StudentClubs",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "StudentClubs",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "StudentActivities",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "StudentActivities",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "StudentActivities",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "StudentActivities",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "StudentActivities",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "StudentActivities",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Locations",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Locations",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Locations",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentClubs",
                table: "StudentClubs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentActivities",
                table: "StudentActivities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Locations",
                table: "Locations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentActivities_Locations_LocationId",
                table: "StudentActivities",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentActivities_StudentClubs_StudentClubId",
                table: "StudentActivities",
                column: "StudentClubId",
                principalTable: "StudentClubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
