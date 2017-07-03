using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Logic
{
    public class AlgSettings
    {
        public List<Commands> commands { get; set; } //список команд
        public int row { get; set; } //стартовая строка
        public int column { get; set; } //стартовый столбец

        public int countGrid { get; set; } //размер поля

        public int rotate { get; set; } //поворот робота

        public AlgSettings()
        {
            commands = new List<Commands>();
        }
    }
}
