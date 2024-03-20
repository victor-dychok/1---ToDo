using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class RefreshToken
    {
        public string Id { get; set; }
        public int UserId {  get; set; }
        public virtual AppUser User { get; set; }
    }
}
