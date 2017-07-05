using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Logic
{
    /// <summary>
    /// Класс алгоритма
    /// </summary>
    public class AlgorithmSettings
    {
        public FieldSettings field;
        public List<Commands> commands;
        public RobotSettings robot;

        public string algName { get; set; } //название алгоритма

        public AlgorithmSettings()
        {
            field = new FieldSettings(); //класс поля
            commands = new List<Commands>(); //класс команд
            robot = new RobotSettings(); //класс робота
        }
    }
}
