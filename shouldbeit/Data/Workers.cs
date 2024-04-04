namespace Thesis_web.Data
{
    public class Workers
    {
		public Guid Id { get; set; }
		public required string Name { get; set; }
		public required string Position { get; set; }
		public string Phone { get; set; }
		public required string Description { get; set; }
		public required string Location { get; set; }
		public required string Skills { get; set; }
		public required string ImageLink { get; set; }
	}
}
