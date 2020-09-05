using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WildberriesParser
{
    class FieldInfo
    {
        public FieldInfo(int column, string name)
        {
            Column = column;
            Name = name;
        }

        public int Column { get; }

        public string Name { get; }
    }
}
