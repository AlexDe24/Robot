using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Robot.Logic;

namespace Robot.Form
{
    /// <summary>
    /// Поле для работа
    /// </summary>
    public partial class Field
    {
        Color _blackColor; //цвета на поле
        Color _whiteColor;

        Grid[,] _colorGrid; //список ячеек
        Image _robot; //ячейка робота

        AlgorithmSettings _algorithmNow;

        public Field(AlgorithmSettings algorithmNow, int colv)
        {
            InitializeComponent();

            _algorithmNow = algorithmNow;

            _blackColor = new Color()
            {
                A = 200,
                R = 0,
                G = 0,
                B = 0
            };

            _whiteColor = new Color()
            {
                A = 0,
                R = 100,
                G = 100,
                B = 100
            };

            CreateField();

            Title = "Алгоритм: " + algorithmNow.algName + "(" + colv + ")";

            DataContext = this;
        }

        public void FieldUpdate()
        {
            for (int i = 0; i < _algorithmNow.field.countGridX; i++)
            {
                for (int j = 0; j < _algorithmNow.field.countGridY; j++)
                {
                    if (_algorithmNow.field.colorList[i, j] == 0)
                    {
                        _colorGrid[i, j].Background = new SolidColorBrush(_whiteColor);
                    }
                    else
                        _colorGrid[i, j].Background = new SolidColorBrush(_blackColor);
                }
            }

            RobotUpdate();
        }

        void RobotUpdate()
        {
            switch (_algorithmNow.robot.rotate)
            {
                case "Направо":
                    _robot.Source = new BitmapImage(new Uri(@"C:\Программы\Robot\Robot\Robot\Resources\RobotRight.PNG"));
                    break;
                case "Вверх":
                    _robot.Source = new BitmapImage(new Uri(@"C:\Программы\Robot\Robot\Robot\Resources\RobotTop.PNG"));
                    break;
                case "Налево":
                    _robot.Source = new BitmapImage(new Uri(@"C:\Программы\Robot\Robot\Robot\Resources\RobotLeft.PNG"));
                    break;
                case "Вниз":
                    _robot.Source = new BitmapImage(new Uri(@"C:\Программы\Robot\Robot\Robot\Resources\RobotBot.PNG"));
                    break;
                default:
                    break;
            }

            Grid.SetRow(_robot, _algorithmNow.robot.row);
            Grid.SetColumn(_robot, _algorithmNow.robot.column);
        }

        public void CreateField()
        {
            _algorithmNow.field.colorList = new int[_algorithmNow.field.countGridX, _algorithmNow.field.countGridY];

            MainGrid.Children.Clear();
            MainGrid.ColumnDefinitions.Clear();
            MainGrid.RowDefinitions.Clear();

            _colorGrid = new Grid[_algorithmNow.field.countGridX, _algorithmNow.field.countGridY];

            _robot = new Image();

            RobotUpdate();

            for (int i = 0; i < _algorithmNow.field.countGridX; i++)
            {
                RowDefinition rd = new RowDefinition();

                MainGrid.RowDefinitions.Add(rd);
            }

            for (int i = 0; i < _algorithmNow.field.countGridY; i++)
            {
                ColumnDefinition cd = new ColumnDefinition();

                MainGrid.ColumnDefinitions.Add(cd);
            }

            for (int i = 0; i < _algorithmNow.field.countGridX; i++)
            {
                for (int j = 0; j < _algorithmNow.field.countGridY; j++)
                {
                    Grid bton = new Grid();

                    bton.Background = new SolidColorBrush(_whiteColor);
                    
                    MainGrid.Children.Add(bton);

                    Grid.SetRow(bton, i);
                    Grid.SetColumn(bton, j);

                    _algorithmNow.field.colorList[i, j] = 0;
                    _colorGrid[i, j] = bton;
                }
            }

            for (int i = 0; i < _algorithmNow.field.countGridY; i++)
            {
                _algorithmNow.field.colorList[0, i] = 1;
                _algorithmNow.field.colorList[_algorithmNow.field.countGridX - 1, i] = 1;

                _colorGrid[0, i].Background = new SolidColorBrush(_blackColor);
                _colorGrid[_algorithmNow.field.countGridX - 1, i].Background = new SolidColorBrush(_blackColor);
            }

            for (int i = 0; i < _algorithmNow.field.countGridX; i++)
            {
                _algorithmNow.field.colorList[i, 0] = 1;
                _algorithmNow.field.colorList[i, _algorithmNow.field.countGridY - 1] = 1;

                _colorGrid[i, 0].Background = new SolidColorBrush(_blackColor);
                _colorGrid[i, _algorithmNow.field.countGridY - 1].Background = new SolidColorBrush(_blackColor);
            }

            MainGrid.Children.Add(_robot);
        }
    }
}
