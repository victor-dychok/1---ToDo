using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Common.BL
{
    public class NotFoundExeption : Exception
    {
        public NotFoundExeption(object filter) : base("Not found by filter: " + JsonSerializer.Serialize(filter)) { }
    }
}
