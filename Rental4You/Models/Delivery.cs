namespace Rental4You.Models
{
    public class Delivery
    {
        public int Id { get; set; }
        public DateTime DeliveryDate { get; set; }
        public double Kms { get; set; }
        public string? Damage { get; set; }
        public string? Remarks { get; set; }
    }
}
