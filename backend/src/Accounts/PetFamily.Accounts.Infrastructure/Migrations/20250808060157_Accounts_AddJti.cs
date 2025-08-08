using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Accounts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Accounts_AddJti : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_refresh_sessions_users_user_id",
                schema: "accounts",
                table: "refresh_sessions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_refresh_sessions",
                schema: "accounts",
                table: "refresh_sessions");

            migrationBuilder.RenameTable(
                name: "refresh_sessions",
                schema: "accounts",
                newName: "refresh_session",
                newSchema: "accounts");

            migrationBuilder.RenameIndex(
                name: "ix_refresh_sessions_user_id",
                schema: "accounts",
                table: "refresh_session",
                newName: "ix_refresh_session_user_id");

            migrationBuilder.AddColumn<Guid>(
                name: "jti",
                schema: "accounts",
                table: "refresh_session",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "pk_refresh_session",
                schema: "accounts",
                table: "refresh_session",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_refresh_session_users_user_id",
                schema: "accounts",
                table: "refresh_session",
                column: "user_id",
                principalSchema: "accounts",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_refresh_session_users_user_id",
                schema: "accounts",
                table: "refresh_session");

            migrationBuilder.DropPrimaryKey(
                name: "pk_refresh_session",
                schema: "accounts",
                table: "refresh_session");

            migrationBuilder.DropColumn(
                name: "jti",
                schema: "accounts",
                table: "refresh_session");

            migrationBuilder.RenameTable(
                name: "refresh_session",
                schema: "accounts",
                newName: "refresh_sessions",
                newSchema: "accounts");

            migrationBuilder.RenameIndex(
                name: "ix_refresh_session_user_id",
                schema: "accounts",
                table: "refresh_sessions",
                newName: "ix_refresh_sessions_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_refresh_sessions",
                schema: "accounts",
                table: "refresh_sessions",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_refresh_sessions_users_user_id",
                schema: "accounts",
                table: "refresh_sessions",
                column: "user_id",
                principalSchema: "accounts",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
