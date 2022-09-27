namespace WebCoreFrontend.Models
{
    public class Booking
    {
        public Nullable<int> Id { get; set; }
        public string UserName { get; set; }
        public int CentreId { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public Centre Centre { get; set; }
    }
}
