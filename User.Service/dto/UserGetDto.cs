﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserServices.dto
{
    public class UserGetDto
    {
        public int Id { get; set; }
        public string Login { get; set; } = default!;
    }
}
