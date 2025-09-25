using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WOTTracker.Models
{
    public class Compensation
    {
        public int Id { get; set; } // Clé primaire simple pour la compensation elle-même

        public int CompensationMinutes { get; set; } // Le montant de la compensation
        public DateTime TransactionDate { get; set; } // La date à laquelle la compensation a été faite

        // Clés étrangères vers les deux tables liées
        public int WorkSessionId { get; set; }
        public int DownTimeRecordId { get; set; }

        // Propriétés de navigation pour qu'EF Core comprenne les liens
        public WorkSession WorkSession { get; set; }
        public DownTimeRecord DownTimeRecord { get; set; }
    }
}
