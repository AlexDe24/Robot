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
        public int countGridX { get; set; } //размер поля по X
        public int countGridY { get; set; } //размер поля по Y

        public int[,] colorList = new int[100, 100]; //закрашенность поля
    }
}
