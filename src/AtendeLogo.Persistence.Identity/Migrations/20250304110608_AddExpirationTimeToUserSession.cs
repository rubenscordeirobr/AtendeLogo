using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AtendeLogo.Persistence.Identity.Migrations;

/// <inheritdoc />
public partial class AddExpirationTimeToUserSession : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<TimeSpan>(
            name: "expiration_time",
            table: "sessions",
            type: "interval",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "expiration_time",
            table: "sessions");
    }
}
