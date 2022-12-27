using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rental4You.Data.Migrations
{
    public partial class updatedVehiclesModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_VehicleCategories_VehicleCategoryId",
                table: "Vehicles");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Vehicles",
                newName: "IsAvailable");

            migrationBuilder.AlterColumn<int>(
                name: "VehicleCategoryId",
                table: "Vehicles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_VehicleCategories_VehicleCategoryId",
                table: "Vehicles",
                column: "VehicleCategoryId",
                principalTable: "VehicleCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_VehicleCategories_VehicleCategoryId",
                table: "Vehicles");

            migrationBuilder.RenameColumn(
                name: "IsAvailable",
                table: "Vehicles",
                newName: "IsActive");

            migrationBuilder.AlterColumn<int>(
                name: "VehicleCategoryId",
                table: "Vehicles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_VehicleCategories_VehicleCategoryId",
                table: "Vehicles",
                column: "VehicleCategoryId",
                principalTable: "VehicleCategories",
                principalColumn: "Id");
        }
    }
}
