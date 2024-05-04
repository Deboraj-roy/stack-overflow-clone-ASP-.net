using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stackoverflow.Domain.Entities
{
    public class Tag : IEntity<Guid>
    {
        public Guid Id { get; set; } // unique identifier
        public string Name { get; set; }
    }
}
