namespace Thesis_web.wwwroot.Hubs
{
    public class NotificationHub : Hub
    {
        public void SendBookingCount(int bookingCount)
        {
            Clients.All.receiveBookingCount(bookingCount);
        }
    }
}
