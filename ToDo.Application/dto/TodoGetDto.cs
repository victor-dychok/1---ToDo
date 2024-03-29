using Application.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Application.dto
{
    public class TodoGetDto
    {
        public int Id { get; set; }
        public string? Label { get; set; } = default!;
        public bool IsDone { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int OwnerId { get; set; }
    }
}
