using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Application.Exeptions
{
    public class ForbidenExeption : Exception
    {
        public ForbidenExeption(string error) : base(error) { }
    }
}
