using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatientManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExternalLoging : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "License",
                table: "Professionals",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Patients",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ExternalLogins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProviderUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LinkedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalLogins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalLogins_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Professionals_IsActive",
                table: "Professionals",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Professionals_IsDeleted",
                table: "Professionals",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Professionals_License",
                table: "Professionals",
                column: "License",
                unique: true,
                filter: "[License] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Professionals_ProfessionalStatus",
                table: "Professionals",
                column: "ProfessionalStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_IsActive",
                table: "Prescriptions",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_IsActive",
                table: "Patients",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_IsDeleted",
                table: "Patients",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PhoneNumber",
                table: "Patients",
                column: "PhoneNumber");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_IsDeleted",
                table: "AspNetUsers",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Role",
                table: "AspNetUsers",
                column: "Role");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserName",
                table: "AspNetUsers",
                column: "UserName",
                unique: true,
                filter: "[UserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalLogins_ApplicationUserId",
                table: "ExternalLogins",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalLogins_ProviderUserId_Provider",
                table: "ExternalLogins",
                columns: new[] { "ProviderUserId", "Provider" },
                unique: true,
                filter: "[ProviderUserId] IS NOT NULL AND [Provider] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalLogins");

            migrationBuilder.DropIndex(
                name: "IX_Professionals_IsActive",
                table: "Professionals");

            migrationBuilder.DropIndex(
                name: "IX_Professionals_IsDeleted",
                table: "Professionals");

            migrationBuilder.DropIndex(
                name: "IX_Professionals_License",
                table: "Professionals");

            migrationBuilder.DropIndex(
                name: "IX_Professionals_ProfessionalStatus",
                table: "Professionals");

            migrationBuilder.DropIndex(
                name: "IX_Prescriptions_IsActive",
                table: "Prescriptions");

            migrationBuilder.DropIndex(
                name: "IX_Patients_IsActive",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_IsDeleted",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_PhoneNumber",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_IsDeleted",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Role",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserName",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "License",
                table: "Professionals",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
