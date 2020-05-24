using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Esen.Notes.Persistence.Migrations
{
    public partial class artistsadded2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Artists");

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "Artists",
                type: "xid",
                nullable: false,
                defaultValue: 0u);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "xmin",
                table: "Artists");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Artists",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[] {  });
        }
    }
}
