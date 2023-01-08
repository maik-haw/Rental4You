using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rental4You.Migrations
{
    public partial class UpdateDeliveryImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "DeliveryImages");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "DeliveryImages",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "DeliveryImages");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "DeliveryImages",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
