using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Logic
{
    public class Action
    {
        FieldSettings _settings;
        List<Commands> _commands;
        RobotSettings _robot;

        /// <summary>
        /// Выполнение команд
        /// </summary>
        /// <param name="step">шаг</param>
        /// <param name="algorithm">класс агоритма</param>
        /// <returns></returns>
        public int Doing(int step, AlgorithmSettings algorithm)
        {
            step--;
            _settings = algorithm.field;
            _commands = algorithm.commands;
            _robot = algorithm.robot;

            switch (_commands[step].name)
            {
                case "Движение":
                    if (_robot.rotate == "Направо")
                        _robot.column += Convert.ToInt32(_commands[step].firstArg);
                    else if (_robot.rotate == "Вверх")
                        _robot.row -= Convert.ToInt32(_commands[step].firstArg);
                    else if (_robot.rotate == "Налево")
                        _robot.column -= Convert.ToInt32(_commands[step].firstArg);
                    else if (_robot.rotate == "Вниз")
                        _robot.row += Convert.ToInt32(_commands[step].firstArg);
                    break;
                case "Поворот":
                    if (_commands[step].firstArg == "Налево")
                    {
                        if (_robot.rotate == "Направо")
                            _robot.rotate = "Вверх";
                        else if (_robot.rotate == "Вверх")
                            _robot.rotate = "Налево";
                        else if (_robot.rotate == "Налево")
                            _robot.rotate = "Вниз";
                        else if (_robot.rotate == "Вниз")
                            _robot.rotate = "Направо";
                    }
                    else if (_commands[step].firstArg == "Направо")
                    {
                        if (_robot.rotate == "Вверх")
                            _robot.rotate = "Направо";
                        else if (_robot.rotate == "Направо")
                            _robot.rotate = "Вниз";
                        else if (_robot.rotate == "Вниз")
                            _robot.rotate = "Налево";
                        else if (_robot.rotate == "Налево")
                            _robot.rotate = "Вверх";
                    }
                    break;
                case "Заливка":
                    /*if (step == -1)
                    {
                        step++;
                    }*/
                    if (_commands[step].firstArg == "Белый")
                        _settings.colorList[_robot.row, _robot.column] = 0;
                    else
                        _settings.colorList[_robot.row, _robot.column] = 1;
                    break;
                case "Изучение":
                    if (_settings.colorList[_robot.row, _robot.column] == 0)
                        return Convert.ToInt32(_commands[step].firstArg);
                    else
                        return Convert.ToInt32(_commands[step].secondArg);
                default:
                    break;
            }
            return Convert.ToInt32(_commands[step].secondArg);
        }
    }
}
