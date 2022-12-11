namespace Rental4You.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Status { get; set; }

    }
}
