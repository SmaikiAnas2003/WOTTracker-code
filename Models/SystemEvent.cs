using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WOTTracker
{
    public class SystemEvent
    {
        public DateTime Timestamp { get; set; }
        public string EventType { get; set; } // ex: "Démarrage", "Arrêt", "Veille", "Réveil"
        public long EventId { get; set; }
    }
}
