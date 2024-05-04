using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stackoverflow.Domain.Entities
{
    public class UserBadge : IEntity<Guid>
    {
        public Guid Id { get; set; } // unique identifier
        public Guid UserId { get; set; } // foreign key referencing the User who earned the badge
        public string BadgeName { get; set; }
        public string BadgeDescription { get; set; }
        public DateTime DateEarned { get; set; }
    }
}
