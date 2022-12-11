namespace Rental4You.Models
{
    public class Pickup
    {
        public int Id { get; set; }
        public DateTime PickupDate { get; set; }
        public double Kms { get; set; }
        public string? Damage { get; set; }
        public string? Remarks { get; set; }
    }
}
