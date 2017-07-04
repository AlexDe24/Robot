using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Logic
{
    public class AlgorithmSettings
    {
        public FieldSettings field;
        public List<Commands> commands;
        public RobotSettings robot;

        public string algName { get; set; }

        public AlgorithmSettings()
        {
            field = new FieldSettings();
            commands = new List<Commands>();
            robot = new RobotSettings();
        }
    }
}
