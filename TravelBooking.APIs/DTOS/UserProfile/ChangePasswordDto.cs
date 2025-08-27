using System.Text.Json.Serialization;

namespace TravelBooking.APIs.DTOS.UserProfile
{
    public class ChangePasswordDto
    {
         [JsonPropertyName("currentPassword")]
        public string CurrentPassword { get; set; } = null!;

        [JsonPropertyName("newPassword")]
        public string NewPassword { get; set; } = null!;
    }
}
