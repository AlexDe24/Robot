using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Logic
{
    /// <summary>
    /// Класс параметров поля
    /// </summary>
    public class FieldSettings
    {
        public int countGrid { get; set; } //размер поля

        public int[,] colorList = new int[100, 100]; //закрашенность поля
    }
}
