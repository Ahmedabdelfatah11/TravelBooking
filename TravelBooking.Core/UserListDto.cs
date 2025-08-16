namespace TravelBooking.Service
{
    // File: UserListDto.cs
    public class UserListDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public bool EmailConfirmed { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();
        public string? EntityName { get; set; }
        public List<UserManagedCompanyDto> ManagedCompanies { get; set; } = new();

    }
    // Helper DTO
    public class UserManagedCompanyDto
    {
        public string Role { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public string CompanyType { get; set; } = string.Empty;
    }
}
