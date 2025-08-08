using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations;

/// <inheritdoc />
public partial class First : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "dbo");

        migrationBuilder.CreateTable(
            name: "Members",
            schema: "dbo",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastUpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastUpdatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeletedByName = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Members", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "OutboxMessageConsumers",
            schema: "dbo",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_OutboxMessageConsumers", x => new { x.Id, x.Name });
            });

        migrationBuilder.CreateTable(
            name: "OutboxMessages",
            schema: "dbo",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                OccurredOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                ProcessedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                Error = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_OutboxMessages", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Permissions",
            schema: "dbo",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Permissions", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Roles",
            schema: "dbo",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                IsSystemRole = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastUpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastUpdatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeletedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Roles", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Users",
            schema: "dbo",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                ProfilePicturePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastUpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastUpdatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeletedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                UserName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                NormalizedUserName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                NormalizedEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                AccessFailedCount = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Gatherings",
            schema: "dbo",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                Type = table.Column<int>(type: "int", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ScheduledAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                MaximumNumberOfAttendees = table.Column<int>(type: "int", nullable: true),
                InvitationsExpireAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                NumberOfAttendees = table.Column<int>(type: "int", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastUpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastUpdatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeletedByName = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Gatherings", x => x.Id);
                table.ForeignKey(
                    name: "FK_Gatherings_Members_CreatorId",
                    column: x => x.CreatorId,
                    principalSchema: "dbo",
                    principalTable: "Members",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "AspNetRoleClaims",
            schema: "dbo",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_AspNetRoleClaims_Roles_RoleId",
                    column: x => x.RoleId,
                    principalSchema: "dbo",
                    principalTable: "Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "RolePermission",
            schema: "dbo",
            columns: table => new
            {
                RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RolePermission", x => new { x.RoleId, x.PermissionId });
                table.ForeignKey(
                    name: "FK_RolePermission_Permissions_PermissionId",
                    column: x => x.PermissionId,
                    principalSchema: "dbo",
                    principalTable: "Permissions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_RolePermission_Roles_RoleId",
                    column: x => x.RoleId,
                    principalSchema: "dbo",
                    principalTable: "Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserClaims",
            schema: "dbo",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_AspNetUserClaims_Users_UserId",
                    column: x => x.UserId,
                    principalSchema: "dbo",
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserLogins",
            schema: "dbo",
            columns: table => new
            {
                LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                table.ForeignKey(
                    name: "FK_AspNetUserLogins_Users_UserId",
                    column: x => x.UserId,
                    principalSchema: "dbo",
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserRoles",
            schema: "dbo",
            columns: table => new
            {
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                table.ForeignKey(
                    name: "FK_AspNetUserRoles_Roles_RoleId",
                    column: x => x.RoleId,
                    principalSchema: "dbo",
                    principalTable: "Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_AspNetUserRoles_Users_UserId",
                    column: x => x.UserId,
                    principalSchema: "dbo",
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserTokens",
            schema: "dbo",
            columns: table => new
            {
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                table.ForeignKey(
                    name: "FK_AspNetUserTokens_Users_UserId",
                    column: x => x.UserId,
                    principalSchema: "dbo",
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "RoleUser",
            schema: "dbo",
            columns: table => new
            {
                RolesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                UsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RoleUser", x => new { x.RolesId, x.UsersId });
                table.ForeignKey(
                    name: "FK_RoleUser_Roles_RolesId",
                    column: x => x.RolesId,
                    principalSchema: "dbo",
                    principalTable: "Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_RoleUser_Users_UsersId",
                    column: x => x.UsersId,
                    principalSchema: "dbo",
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Attendees",
            schema: "dbo",
            columns: table => new
            {
                GatheringId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastUpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastUpdatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeletedByName = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Attendees", x => new { x.GatheringId, x.MemberId });
                table.ForeignKey(
                    name: "FK_Attendees_Gatherings_GatheringId",
                    column: x => x.GatheringId,
                    principalSchema: "dbo",
                    principalTable: "Gatherings",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Attendees_Members_MemberId",
                    column: x => x.MemberId,
                    principalSchema: "dbo",
                    principalTable: "Members",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Invitations",
            schema: "dbo",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                GatheringId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Status = table.Column<int>(type: "int", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastUpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastUpdatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeletedByName = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Invitations", x => x.Id);
                table.ForeignKey(
                    name: "FK_Invitations_Gatherings_GatheringId",
                    column: x => x.GatheringId,
                    principalSchema: "dbo",
                    principalTable: "Gatherings",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Invitations_Members_MemberId",
                    column: x => x.MemberId,
                    principalSchema: "dbo",
                    principalTable: "Members",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_AspNetRoleClaims_RoleId",
            schema: "dbo",
            table: "AspNetRoleClaims",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUserClaims_UserId",
            schema: "dbo",
            table: "AspNetUserClaims",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUserLogins_UserId",
            schema: "dbo",
            table: "AspNetUserLogins",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUserRoles_RoleId",
            schema: "dbo",
            table: "AspNetUserRoles",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "IX_Attendees_MemberId",
            schema: "dbo",
            table: "Attendees",
            column: "MemberId");

        migrationBuilder.CreateIndex(
            name: "IX_Gatherings_CreatorId",
            schema: "dbo",
            table: "Gatherings",
            column: "CreatorId");

        migrationBuilder.CreateIndex(
            name: "IX_Invitations_GatheringId",
            schema: "dbo",
            table: "Invitations",
            column: "GatheringId");

        migrationBuilder.CreateIndex(
            name: "IX_Invitations_MemberId",
            schema: "dbo",
            table: "Invitations",
            column: "MemberId");

        migrationBuilder.CreateIndex(
            name: "IX_Members_Email",
            schema: "dbo",
            table: "Members",
            column: "Email",
            unique: true,
            filter: "[Email] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "IX_RolePermission_PermissionId",
            schema: "dbo",
            table: "RolePermission",
            column: "PermissionId");

        migrationBuilder.CreateIndex(
            name: "RoleNameIndex",
            schema: "dbo",
            table: "Roles",
            column: "NormalizedName",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_RoleUser_UsersId",
            schema: "dbo",
            table: "RoleUser",
            column: "UsersId");

        migrationBuilder.CreateIndex(
            name: "EmailIndex",
            schema: "dbo",
            table: "Users",
            column: "NormalizedEmail");

        migrationBuilder.CreateIndex(
            name: "IX_Users_Email",
            schema: "dbo",
            table: "Users",
            column: "Email",
            unique: true,
            filter: "[Email] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "UserNameIndex",
            schema: "dbo",
            table: "Users",
            column: "NormalizedUserName",
            unique: true,
            filter: "[NormalizedUserName] IS NOT NULL");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "AspNetRoleClaims",
            schema: "dbo");

        migrationBuilder.DropTable(
            name: "AspNetUserClaims",
            schema: "dbo");

        migrationBuilder.DropTable(
            name: "AspNetUserLogins",
            schema: "dbo");

        migrationBuilder.DropTable(
            name: "AspNetUserRoles",
            schema: "dbo");

        migrationBuilder.DropTable(
            name: "AspNetUserTokens",
            schema: "dbo");

        migrationBuilder.DropTable(
            name: "Attendees",
            schema: "dbo");

        migrationBuilder.DropTable(
            name: "Invitations",
            schema: "dbo");

        migrationBuilder.DropTable(
            name: "OutboxMessageConsumers",
            schema: "dbo");

        migrationBuilder.DropTable(
            name: "OutboxMessages",
            schema: "dbo");

        migrationBuilder.DropTable(
            name: "RolePermission",
            schema: "dbo");

        migrationBuilder.DropTable(
            name: "RoleUser",
            schema: "dbo");

        migrationBuilder.DropTable(
            name: "Gatherings",
            schema: "dbo");

        migrationBuilder.DropTable(
            name: "Permissions",
            schema: "dbo");

        migrationBuilder.DropTable(
            name: "Roles",
            schema: "dbo");

        migrationBuilder.DropTable(
            name: "Users",
            schema: "dbo");

        migrationBuilder.DropTable(
            name: "Members",
            schema: "dbo");
    }
}
