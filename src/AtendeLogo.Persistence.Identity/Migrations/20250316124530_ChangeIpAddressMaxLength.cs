using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AtendeLogo.Persistence.Identity.Migrations;

/// <inheritdoc />
public partial class ChangeIpAddressMaxLength : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        Guard.NotNull(migrationBuilder);

        migrationBuilder.AlterColumn<string>(
            name: "ip_address",
            table: "sessions",
            type: "character varying(45)",
            maxLength: 45,
            nullable: false,
            collation: "case_accent_insensitive",
            oldClrType: typeof(string),
            oldType: "character varying(15)",
            oldMaxLength: 15,
            oldCollation: "case_accent_insensitive");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        Guard.NotNull(migrationBuilder);

        migrationBuilder.AlterColumn<string>(
            name: "ip_address",
            table: "sessions",
            type: "character varying(15)",
            maxLength: 15,
            nullable: false,
            collation: "case_accent_insensitive",
            oldClrType: typeof(string),
            oldType: "character varying(45)",
            oldMaxLength: 45,
            oldCollation: "case_accent_insensitive");
    }
}
