using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Logic
{
    /// <summary>
    /// Класс параметров робота
    /// </summary>
    public class RobotSettings
    {
        public int row { get; set; } //строка
        public int column { get; set; } //столбец

        public string rotate { get; set; } //поворот робота
    }
}
