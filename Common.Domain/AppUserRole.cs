﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Domain
{
    public class AppUserRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<AppUserAppRole> AppUserAppRole { get; set; }
    }
}
