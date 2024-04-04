namespace Thesis_web.Data
{
    public class Applicants
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Position { get; set; }
        public required string Phone { get; set; }
        public required string Description { get; set; }
        public required string ImageLink { get; set; }
        public string User_email { get; set; }
        public string Status { get; set; }
    }
}
