using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WOTTracker.Models
{
    public class DownTimeRecord
    {
        public int Id { get; set; }
        public int AppSettingsVersionId { get; set; } // Clé étrangère
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int IsConge { get; set; }
        public int IsPermission { get; set; }

        public virtual ICollection<Compensation> Compensations { get; set; }
    }
}
