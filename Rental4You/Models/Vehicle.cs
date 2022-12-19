namespace Rental4You.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public double Kms { get; set; }
        public bool IsActive { get; set; }
        public string? Location { get; set; }
        public decimal Cost { get; set; }
        // Foreign Keys:
        public int VehicleCategoryId { get; set; }
        public VehicleCategory? VehicleCategory { get; set; }
        public int CompanyId { get; set; }
        public Company? Company { get; set; }
    }
}
