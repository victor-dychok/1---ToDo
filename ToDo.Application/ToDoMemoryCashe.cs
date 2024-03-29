using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Application
{
    public class ToDoMemoryCashe
    {
        public MemoryCache Cache { get; } = new MemoryCache(
            new MemoryCacheOptions
            {
                SizeLimit = 1024,
            });
    }
}
