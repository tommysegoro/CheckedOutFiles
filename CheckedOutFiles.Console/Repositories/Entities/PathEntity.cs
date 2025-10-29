using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckedOutFiles.Console.Repositories.Entities
{
    public class PathEntity
    {
        public string Path { get; set; }
        public bool Recursive { get; set; }
    }
}
