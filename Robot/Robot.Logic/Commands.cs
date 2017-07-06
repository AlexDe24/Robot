using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Logic
{
    /// <summary>
    /// Класс команд
    /// </summary>
    public class Commands
    {
        public int nom { get; set; } //номер команды
        public string name { get; set; } //название команды 
        public string firstArg { get; set; } //первый аргумент
        public string secondArg { get; set; } //второй аргумент
    }
}
