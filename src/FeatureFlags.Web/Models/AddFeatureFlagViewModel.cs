using System.ComponentModel.DataAnnotations;

namespace FeatureFlags.Web.Models
{
    public class AddFeatureFlagViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string NewName { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string? NewDescription { get; set; }
    }
}