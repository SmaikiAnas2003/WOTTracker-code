using System;
using System.Collections.Generic;


namespace WOTTracker.Models
{
    public class WorkSession
    {
        public int Id { get; set; }
        public int AppSettingsVersionId { get; set; } // Clé étrangère
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; } // Nullable pour les sessions en cours
        public int? OverTimeMinutes { get; set; }
        public string TypeOverTime { get; set; }

        public string? WorkingType { get; set; } // "Home Office" ou "On Site"
        public string Notes { get; set; }

        // LIGNE À AJOUTER
        public virtual ICollection<Compensation> Compensations { get; set; }
    }
}
