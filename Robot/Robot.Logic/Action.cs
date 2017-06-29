using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Logic
{
    public class Action
    {
        public int[] Move(int[] startRowColumn, int countGrid, List<Commands> commands)
        {
            startRowColumn[0]++;
            /*switch (e.Key)
            {
                case Key.Down:
                    if (startRowColumn[0] < countGrid)
                        startRowColumn[0]++;
                    break;
                case Key.Up:
                    if (startRowColumn[0] > 0)
                        startRowColumn[0]--;
                    break;
                case Key.Left:
                    if (startRowColumn[1] > 0)
                        startRowColumn[1]--;
                    break;
                case Key.Right:
                    if (startRowColumn[1] < countGrid)
                        startRowColumn[1]++;
                    break;
                case Key.Enter:
                    imageGrid[_startRow, _startColumn].Background = new SolidColorBrush(blackColor);
                    break;
                default:
                    break;
            }*/
            return startRowColumn;
        }
    }
}
