using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Logic
{
    public class AlgSettings
    {
        public List<Commands> commands;
        public int startRow;
        public int startColumn;

        public int countGrid;

        public AlgSettings()
        {
            commands = new List<Commands>();
        }
    }
}
