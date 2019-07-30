using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDesk.Api.Data
{
    public class Ticket
    {
        public Ticket()
        {
            this.Context = new Dictionary<string, object>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int AssignedToUserId { get; set; }

        public int CreatedByUserId { get; set; }

        public DateTime CreatedTime { get; set; }

        public string Status { get; set; }

        public Dictionary<string, object> Context { get; }
    }
}
