using System;
using System.ComponentModel.DataAnnotations;

namespace WOTTracker.Models
{
    public class AppSettingsHistory
    {
        public int VersionId { get; set; }

        [Required]
        public bool isActive { get; set; }
        
        [Required]
        public DateTime ActivationDate { get; set; }

        [Required]
        public string UserRole { get; set; }

        [Required]
        public string WorkingHoursStart { get; set; }

        [Required]
        public string WorkingHoursEnd { get; set; }

        [Required]
        public int ToleranceStartMinutes { get; set; }

        [Required]
        public int ToleranceEndMinutes { get; set; }

        [Required]
        public string BreakHoursStart { get; set; }

        [Required]
        public string BreakHoursEnd { get; set; }

        [Required]
        public string EmailRecipient { get; set; }

        [Required]
        public string EmailSender { get; set; }

        [Required]
        public DateTime ExpirationOverTime { get; set; }  
    }
}
