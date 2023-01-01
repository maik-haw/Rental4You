namespace Rental4You.Models
{
    public class VehicleCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // Foreign Keys:
        public ICollection<Vehicle>? Vehicles { get; set; }
    }
}
