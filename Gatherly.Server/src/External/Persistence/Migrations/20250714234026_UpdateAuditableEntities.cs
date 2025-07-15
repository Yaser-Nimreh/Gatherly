using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAuditableEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Invitations");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Attendees",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Attendees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByName",
                table: "Attendees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Attendees",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "Attendees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedByName",
                table: "Attendees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Attendees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "Attendees",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastUpdatedById",
                table: "Attendees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedByName",
                table: "Attendees",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Attendees");

            migrationBuilder.DropColumn(
                name: "CreatedByName",
                table: "Attendees");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Attendees");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Attendees");

            migrationBuilder.DropColumn(
                name: "DeletedByName",
                table: "Attendees");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Attendees");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                table: "Attendees");

            migrationBuilder.DropColumn(
                name: "LastUpdatedById",
                table: "Attendees");

            migrationBuilder.DropColumn(
                name: "LastUpdatedByName",
                table: "Attendees");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Attendees",
                newName: "CreatedOn");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Invitations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Invitations",
                type: "datetime2",
                nullable: true);
        }
    }
}
