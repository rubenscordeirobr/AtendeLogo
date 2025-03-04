using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AtendeLogo.Persistence.Identity.Migrations;

/// <inheritdoc />
public partial class InitialMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterDatabase()
            .Annotation("Npgsql:CollationDefinition:case_accent_insensitive", "und-u-ks-level1,und-u-ks-level1,icu,False")
            .Annotation("Npgsql:Enum:admin_user_role", "manager,operator,super_admin,viewer")
            .Annotation("Npgsql:Enum:authentication_type", "anonymous,email_password,facebook,google,microsoft,sms,system,unknown,whats_app")
            .Annotation("Npgsql:Enum:business_type", "civil_registry_office,system")
            .Annotation("Npgsql:Enum:country", "argentina,bolivia,brazil,canada,chile,colombia,ecuador,france,germany,guyana,italy,mexico,paraguay,peru,portugal,spain,suriname,united_kingdom,united_states,unknown,uruguay,venezuela")
            .Annotation("Npgsql:Enum:currency", "brl,eur,usd")
            .Annotation("Npgsql:Enum:language", "default,english,french,german,italian,portuguese,spanish")
            .Annotation("Npgsql:Enum:password_strength", "empty,medium,strong,weak")
            .Annotation("Npgsql:Enum:tenant_state", "cancelled,closed,new,onboarding,operational,system,trial")
            .Annotation("Npgsql:Enum:tenant_status", "active,archived,inactive,pending,suspended")
            .Annotation("Npgsql:Enum:tenant_type", "company,individual,system")
            .Annotation("Npgsql:Enum:tenant_user_role", "admin,chat_agent,manager,operator,owner,viewer")
            .Annotation("Npgsql:Enum:user_state", "active,blocked,deleted,inactive,new,pending_verification,suspended")
            .Annotation("Npgsql:Enum:user_status", "anonymous,away,busy,do_not_disturb,offline,online,system")
            .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

        migrationBuilder.CreateTable(
            name: "addresses",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                address_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, collation: "case_accent_insensitive"),
                street = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "case_accent_insensitive"),
                number = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, collation: "case_accent_insensitive"),
                complement = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, collation: "case_accent_insensitive"),
                neighborhood = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, collation: "case_accent_insensitive"),
                city = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, collation: "case_accent_insensitive"),
                state = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false, collation: "case_accent_insensitive"),
                zip_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, collation: "case_accent_insensitive"),
                country = table.Column<Country>(type: "country", nullable: false),
                tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                is_default = table.Column<bool>(type: "boolean", nullable: false),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                deleted_session_id = table.Column<Guid>(type: "uuid", nullable: true),
                sort_order = table.Column<double>(type: "double precision", nullable: true, defaultValueSql: "get_next_sort_order_asc('addresses', 'sort_order')"),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                last_updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                created_session_id = table.Column<Guid>(type: "uuid", nullable: false),
                last_updated_session_id = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_addresses", x => x.id);
                table.CheckConstraint("ck_addresses_created_session_id_not_empty", "created_session_id <> '00000000-0000-0000-0000-000000000000'::uuid");
                table.CheckConstraint("ck_addresses_id_not_empty", "id <> '00000000-0000-0000-0000-000000000000'::uuid");
                table.CheckConstraint("ck_addresses_last_updated_session_id_not_empty", "last_updated_session_id <> '00000000-0000-0000-0000-000000000000'::uuid");
                table.CheckConstraint("ck_addresses_tenant_id_not_empty", "tenant_id <> '00000000-0000-0000-0000-000000000000'::uuid");
            });

        migrationBuilder.CreateTable(
            name: "sessions",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                client_session_token = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false, collation: "case_accent_insensitive"),
                application_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, collation: "case_accent_insensitive"),
                ip_address = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false, collation: "case_accent_insensitive"),
                auth_token = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true, collation: "case_accent_insensitive"),
                user_agent = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false, collation: "case_accent_insensitive"),
                is_active = table.Column<bool>(type: "boolean", nullable: false),
                last_activity = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                terminated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                language = table.Column<Language>(type: "language", nullable: false),
                authentication_type = table.Column<AuthenticationType>(type: "authentication_type", nullable: false),
                termination_reason = table.Column<int>(type: "integer", nullable: true),
                user_id = table.Column<Guid>(type: "uuid", nullable: false),
                tenant_id = table.Column<Guid>(type: "uuid", nullable: true),
                GeoLocation_Latitude = table.Column<double>(type: "double precision", nullable: true),
                GeoLocation_Longitude = table.Column<double>(type: "double precision", nullable: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                last_updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                created_session_id = table.Column<Guid>(type: "uuid", nullable: false),
                last_updated_session_id = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_sessions", x => x.id);
                table.CheckConstraint("ck_sessions_created_session_id_not_empty", "created_session_id <> '00000000-0000-0000-0000-000000000000'::uuid");
                table.CheckConstraint("ck_sessions_id_not_empty", "id <> '00000000-0000-0000-0000-000000000000'::uuid");
                table.CheckConstraint("ck_sessions_last_updated_session_id_not_empty", "last_updated_session_id <> '00000000-0000-0000-0000-000000000000'::uuid");
                table.CheckConstraint("ck_sessions_user_id_not_empty", "user_id <> '00000000-0000-0000-0000-000000000000'::uuid");
            });

        migrationBuilder.CreateTable(
            name: "tenants",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, collation: "case_accent_insensitive"),
                fiscal_code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, collation: "case_accent_insensitive"),
                email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "case_accent_insensitive"),
                business_type = table.Column<BusinessType>(type: "business_type", nullable: false),
                country = table.Column<Country>(type: "country", nullable: false),
                currency = table.Column<Currency>(type: "currency", nullable: false),
                language = table.Column<Language>(type: "language", nullable: false),
                tenant_state = table.Column<TenantState>(type: "tenant_state", nullable: false),
                tenant_status = table.Column<TenantStatus>(type: "tenant_status", nullable: false),
                tenant_type = table.Column<TenantType>(type: "tenant_type", nullable: false),
                phone_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                owner_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                address_id = table.Column<Guid>(type: "uuid", nullable: true),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                deleted_session_id = table.Column<Guid>(type: "uuid", nullable: true),
                TimeZone_Location = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                Offset = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                last_updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                created_session_id = table.Column<Guid>(type: "uuid", nullable: false),
                last_updated_session_id = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_tenants", x => x.id);
                table.CheckConstraint("ck_tenants_created_session_id_not_empty", "created_session_id <> '00000000-0000-0000-0000-000000000000'::uuid");
                table.CheckConstraint("ck_tenants_id_not_empty", "id <> '00000000-0000-0000-0000-000000000000'::uuid");
                table.CheckConstraint("ck_tenants_last_updated_session_id_not_empty", "last_updated_session_id <> '00000000-0000-0000-0000-000000000000'::uuid");
                table.ForeignKey(
                    name: "fk_tenants_addresses_address_id",
                    column: x => x.address_id,
                    principalTable: "addresses",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "users",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, collation: "case_accent_insensitive"),
                email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "case_accent_insensitive"),
                user_state = table.Column<UserState>(type: "user_state", nullable: false),
                user_status = table.Column<UserStatus>(type: "user_status", nullable: false),
                phone_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                deleted_session_id = table.Column<Guid>(type: "uuid", nullable: true),
                sort_order = table.Column<double>(type: "double precision", nullable: true, defaultValueSql: "get_next_sort_order_asc('users', 'sort_order')"),
                discriminator = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false, collation: "case_accent_insensitive"),
                Password_HashValue = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                Password_Strength = table.Column<PasswordStrength>(type: "password_strength", nullable: false),
                admin_user_role = table.Column<AdminUserRole>(type: "admin_user_role", nullable: true),
                tenant_id = table.Column<Guid>(type: "uuid", nullable: true),
                tenant_user_role = table.Column<TenantUserRole>(type: "tenant_user_role", nullable: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                last_updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                created_session_id = table.Column<Guid>(type: "uuid", nullable: false),
                last_updated_session_id = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_users", x => x.id);
                table.CheckConstraint("ck_users_created_session_id_not_empty", "created_session_id <> '00000000-0000-0000-0000-000000000000'::uuid");
                table.CheckConstraint("ck_users_id_not_empty", "id <> '00000000-0000-0000-0000-000000000000'::uuid");
                table.CheckConstraint("ck_users_last_updated_session_id_not_empty", "last_updated_session_id <> '00000000-0000-0000-0000-000000000000'::uuid");
                table.CheckConstraint("ck_users_tenant_id_not_empty", "tenant_id <> '00000000-0000-0000-0000-000000000000'::uuid");
                table.ForeignKey(
                    name: "fk_users_tenants_tenant_id",
                    column: x => x.tenant_id,
                    principalTable: "tenants",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "ix_addresses_sort_order",
            table: "addresses",
            column: "sort_order");

        migrationBuilder.CreateIndex(
            name: "ix_addresses_tenant_id",
            table: "addresses",
            column: "tenant_id");

        migrationBuilder.CreateIndex(
            name: "ix_sessions_auth_token",
            table: "sessions",
            column: "auth_token");

        migrationBuilder.CreateIndex(
            name: "ix_sessions_client_session_token",
            table: "sessions",
            column: "client_session_token",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_sessions_tenant_id",
            table: "sessions",
            column: "tenant_id");

        migrationBuilder.CreateIndex(
            name: "ix_sessions_user_id",
            table: "sessions",
            column: "user_id");

        migrationBuilder.CreateIndex(
            name: "ix_tenants_address_id",
            table: "tenants",
            column: "address_id",
            unique: true,
            filter: "is_deleted = false");

        migrationBuilder.CreateIndex(
            name: "ix_tenants_email",
            table: "tenants",
            column: "email",
            unique: true,
            filter: "is_deleted = false");

        migrationBuilder.CreateIndex(
            name: "ix_tenants_fiscal_code",
            table: "tenants",
            column: "fiscal_code",
            unique: true,
            filter: "is_deleted = false");

        migrationBuilder.CreateIndex(
            name: "ix_tenants_owner_user_id",
            table: "tenants",
            column: "owner_user_id",
            unique: true,
            filter: "is_deleted = false");

        migrationBuilder.CreateIndex(
            name: "ix_users_email",
            table: "users",
            column: "email",
            unique: true,
            filter: "is_deleted = false");

        migrationBuilder.CreateIndex(
            name: "ix_users_sort_order",
            table: "users",
            column: "sort_order");

        migrationBuilder.CreateIndex(
            name: "ix_users_tenant_id",
            table: "users",
            column: "tenant_id");

        migrationBuilder.AddForeignKey(
            name: "fk_addresses_tenants_tenant_id",
            table: "addresses",
            column: "tenant_id",
            principalTable: "tenants",
            principalColumn: "id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "fk_sessions_tenants_tenant_id",
            table: "sessions",
            column: "tenant_id",
            principalTable: "tenants",
            principalColumn: "id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "fk_sessions_users_user_id",
            table: "sessions",
            column: "user_id",
            principalTable: "users",
            principalColumn: "id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "fk_tenants_users_owner_user_id",
            table: "tenants",
            column: "owner_user_id",
            principalTable: "users",
            principalColumn: "id",
            onDelete: ReferentialAction.Restrict);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "fk_addresses_tenants_tenant_id",
            table: "addresses");

        migrationBuilder.DropForeignKey(
            name: "fk_users_tenants_tenant_id",
            table: "users");

        migrationBuilder.DropTable(
            name: "sessions");

        migrationBuilder.DropTable(
            name: "tenants");

        migrationBuilder.DropTable(
            name: "addresses");

        migrationBuilder.DropTable(
            name: "users");
    }
}
