namespace TravelBooking.APIs.DTOS.Users
{
    public class UserListResponse<T>
    {
        public List<T> Data { get; set; } = new();
        public int TotalCount { get; set; }
    }
}
