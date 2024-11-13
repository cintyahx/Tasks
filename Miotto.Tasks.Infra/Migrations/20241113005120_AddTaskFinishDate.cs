using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miotto.Tasks.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskFinishDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FinishDate",
                table: "ProjectTask",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinishDate",
                table: "ProjectTask");
        }
    }
}
