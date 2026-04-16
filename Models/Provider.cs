namespace WebApplication1.Models
{
    public class Provider
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Service { get; set; } = "";
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}