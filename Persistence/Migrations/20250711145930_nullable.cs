using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class nullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskUser_AspNetUsers_UserId",
                table: "TaskUser");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskUser_ProjectTasks_TaskId",
                table: "TaskUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskUser",
                table: "TaskUser");

            migrationBuilder.RenameTable(
                name: "TaskUser",
                newName: "TaskUsers");

            migrationBuilder.RenameIndex(
                name: "IX_TaskUser_TaskId",
                table: "TaskUsers",
                newName: "IX_TaskUsers_TaskId");

            migrationBuilder.AlterColumn<string>(
                name: "Patronymic",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskUsers",
                table: "TaskUsers",
                columns: new[] { "UserId", "TaskId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TaskUsers_AspNetUsers_UserId",
                table: "TaskUsers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskUsers_ProjectTasks_TaskId",
                table: "TaskUsers",
                column: "TaskId",
                principalTable: "ProjectTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskUsers_AspNetUsers_UserId",
                table: "TaskUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskUsers_ProjectTasks_TaskId",
                table: "TaskUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskUsers",
                table: "TaskUsers");

            migrationBuilder.RenameTable(
                name: "TaskUsers",
                newName: "TaskUser");

            migrationBuilder.RenameIndex(
                name: "IX_TaskUsers_TaskId",
                table: "TaskUser",
                newName: "IX_TaskUser_TaskId");

            migrationBuilder.AlterColumn<string>(
                name: "Patronymic",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskUser",
                table: "TaskUser",
                columns: new[] { "UserId", "TaskId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TaskUser_AspNetUsers_UserId",
                table: "TaskUser",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskUser_ProjectTasks_TaskId",
                table: "TaskUser",
                column: "TaskId",
                principalTable: "ProjectTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
