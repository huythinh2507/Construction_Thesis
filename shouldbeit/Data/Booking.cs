namespace Thesis_web.Data
{
    public class Booking
    {
        public Guid Id { get; set; }

        public string ServiceType { get; set; }
        public string Location { get; set; }
        public string Workers { get; set; }
        public string WorkingTime { get; set; }
        public string StartTime { get; set; }
        public string Note { get; set; }
        public string PromoCode { get; set; }
        public string WorkDate { get; set; }
        public string Phone { get; set; }
        public string TotalPrice { get; set; }
        public string User_email { get; set; }
        public string Status { get; set; }
        public DateTime SubmittedDate { get; internal set; }
        public int requiredBuilders {  get; set; }
        public int requiredOperators { get; set; }
        public int requiredDesigners { get; set; }
        public int requiredPManagers { get; set; }

    }
}