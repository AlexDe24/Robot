using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Logic
{
    public class Action
    {
        /// <summary>
        /// Функция для передвижения робота
        /// </summary>
        /// <param name="rowColumn">номер строки/столбца</param>
        /// <param name="countGrid">количество строк</param>
        /// <param name="commands">комманды</param>
        /// <returns></returns>
        public int Move(AlgSettings settings, int step, int[,] colorList)
        {
            switch (settings.commands[step].name)
            {
                case "Движение":
                    if (settings.rotate == 0 && settings.column < settings.countGrid)
                        settings.column += settings.commands[step].firstArg;
                    else if (settings.rotate == 90 && settings.row > 0)
                        settings.row -= settings.commands[step].firstArg;
                    else if (settings.rotate == 180 && settings.column > 0)
                        settings.column -= settings.commands[step].firstArg;
                    else if (settings.rotate == 270 && settings.row < settings.countGrid)
                        settings.row += settings.commands[step].firstArg;
                    break;
                case "Поворот":
                    if (settings.commands[step].firstArg == 1)
                    {
                        if (settings.rotate == 0)
                            settings.rotate = 90;
                        else if (settings.rotate == 90)
                            settings.rotate = 180;
                        else if (settings.rotate == 180)
                            settings.rotate = 270;
                        else if (settings.rotate == 270)
                            settings.rotate = 0;
                    }
                    else if (settings.commands[step].firstArg == 0)
                    {
                        if (settings.rotate == 90)
                            settings.rotate = 0;
                        else if (settings.rotate == 180)
                            settings.rotate = 90;
                        else if (settings.rotate == 270)
                            settings.rotate = 180;
                        else if (settings.rotate == 0)
                            settings.rotate = 270;
                    }
                    break;
                default:
                    break;
            }
            return settings.commands[step].secondArg;
        }
    }
}
