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
        /// Функция для передвижения робота
        /// </summary>
        /// <param name="rowColumn">номер строки/столбца</param>
        /// <param name="countGrid">количество строк</param>
        /// <param name="commands">комманды</param>
        /// <returns></returns>
        public int Move(int step, AlgorithmSettings algorithm)
        {
            _settings = algorithm.field;
            _commands = algorithm.commands;
            _robot = algorithm.robot;

            switch (_commands[step].name)
            {
                case "Движение":
                    if (_robot.rotate == 0 && _robot.column < _settings.countGrid)
                        _robot.column += _commands[step].firstArg;
                    else if (_robot.rotate == 90 && _robot.row > 0)
                        _robot.row -= _commands[step].firstArg;
                    else if (_robot.rotate == 180 && _robot.column > 0)
                        _robot.column -= _commands[step].firstArg;
                    else if (_robot.rotate == 270 && _robot.row < _settings.countGrid)
                        _robot.row += _commands[step].firstArg;
                    break;
                case "Поворот":
                    if (_commands[step].firstArg == 1)
                    {
                        if (_robot.rotate == 0)
                            _robot.rotate = 90;
                        else if (_robot.rotate == 90)
                            _robot.rotate = 180;
                        else if (_robot.rotate == 180)
                            _robot.rotate = 270;
                        else if (_robot.rotate == 270)
                            _robot.rotate = 0;
                    }
                    else if (_commands[step].firstArg == 0)
                    {
                        if (_robot.rotate == 90)
                            _robot.rotate = 0;
                        else if (_robot.rotate == 180)
                            _robot.rotate = 90;
                        else if (_robot.rotate == 270)
                            _robot.rotate = 180;
                        else if (_robot.rotate == 0)
                            _robot.rotate = 270;
                    }
                    break;
                case "Заливка":
                    _settings.colorList[_robot.row, _robot.column] = _commands[step].firstArg;
                    break;
                case "Изучение":
                    if (_settings.colorList[_robot.row, _robot.column] == 0)
                        return _commands[step].firstArg;
                    else
                        return _commands[step].secondArg;
                default:
                    break;
            }
            return _commands[step].secondArg;
        }
    }
}
