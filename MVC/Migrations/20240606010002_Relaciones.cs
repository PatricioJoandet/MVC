using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC.Migrations
{
    /// <inheritdoc />
    public partial class Relaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "tableroId",
                table: "Tareas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "userId",
                table: "Tableros",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_tableroId",
                table: "Tareas",
                column: "tableroId");

            migrationBuilder.CreateIndex(
                name: "IX_Tableros_userId",
                table: "Tableros",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tableros_Usuarios_userId",
                table: "Tableros",
                column: "userId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tareas_Tableros_tableroId",
                table: "Tareas",
                column: "tableroId",
                principalTable: "Tableros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tableros_Usuarios_userId",
                table: "Tableros");

            migrationBuilder.DropForeignKey(
                name: "FK_Tareas_Tableros_tableroId",
                table: "Tareas");

            migrationBuilder.DropIndex(
                name: "IX_Tareas_tableroId",
                table: "Tareas");

            migrationBuilder.DropIndex(
                name: "IX_Tableros_userId",
                table: "Tableros");

            migrationBuilder.DropColumn(
                name: "tableroId",
                table: "Tareas");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "Tableros");
        }
    }
}
