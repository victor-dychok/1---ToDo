using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoBL.dto
{
    public class CreateToDo
    {
        public string? Label { get; set; } = default!;
        public bool IsDone { get; set; }
        public int OwnerId { get; set; }
    }
}
