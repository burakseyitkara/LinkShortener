using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LinkShortener.Infrastructure.Data.Migrations;

/// <summary>
/// İlk veritabanı migration'ı
/// </summary>
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Users tablosu
        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                PasswordHash = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                IsEmailVerified = table.Column<bool>(type: "boolean", nullable: false),
                LastLoginAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                IsActive = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
            });

        // Links tablosu
        migrationBuilder.CreateTable(
            name: "Links",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                OriginalUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                ShortCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                ClickCount = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                UserId = table.Column<Guid>(type: "uuid", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                IsActive = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Links", x => x.Id);
                table.ForeignKey(
                    name: "FK_Links_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.SetNull);
            });

        // LinkClicks tablosu
        migrationBuilder.CreateTable(
            name: "LinkClicks",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                LinkId = table.Column<Guid>(type: "uuid", nullable: false),
                IpAddress = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                UserAgent = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                Referer = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                DeviceType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                OperatingSystem = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                IsActive = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_LinkClicks", x => x.Id);
                table.ForeignKey(
                    name: "FK_LinkClicks_Links_LinkId",
                    column: x => x.LinkId,
                    principalTable: "Links",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        // İndeksler
        migrationBuilder.CreateIndex(
            name: "IX_LinkClicks_CreatedAt",
            table: "LinkClicks",
            column: "CreatedAt");

        migrationBuilder.CreateIndex(
            name: "IX_LinkClicks_DeviceType",
            table: "LinkClicks",
            column: "DeviceType");

        migrationBuilder.CreateIndex(
            name: "IX_LinkClicks_IpAddress",
            table: "LinkClicks",
            column: "IpAddress");

        migrationBuilder.CreateIndex(
            name: "IX_LinkClicks_LinkId",
            table: "LinkClicks",
            column: "LinkId");

        migrationBuilder.CreateIndex(
            name: "IX_Links_ShortCode",
            table: "Links",
            column: "ShortCode",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Links_UserId",
            table: "Links",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_Users_Email",
            table: "Users",
            column: "Email",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Users_Username",
            table: "Users",
            column: "Username",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "LinkClicks");
        migrationBuilder.DropTable(name: "Links");
        migrationBuilder.DropTable(name: "Users");
    }
} 