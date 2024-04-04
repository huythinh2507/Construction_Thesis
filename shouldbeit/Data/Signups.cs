namespace Thesis_web.Data
{
	public class Signups
	{
		public Guid Id { get; set; }
		public required string Name { get; set; }
		public required string Email { get; set; }
		public required string Password { get; set; }
		public required string Role { get; set; }
		public required string Status { get; set; }
		public required string AnswerField { get; set; }
    }
}
