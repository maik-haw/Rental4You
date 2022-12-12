namespace Rental4You.Models
{
    public class DeliveryImage
    {
        public int Id { get; set; }
        public string? FilePath { get; set; }
        // Foreign Keys:
        public int DeliveryId { get; set; }
        public Delivery? Delivery { get; set; }
    }
}
