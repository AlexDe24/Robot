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
    /// Логика взаимодействия для FieldSet.xaml
    /// </summary>
    public partial class FieldSet
    {

        Color _blackColor;
        Color _whiteColor;
        Color _robotColor;

        Grid[,] _colorGrid;
        Grid _robotGrid;

        AlgorithmSettings _algorithmNow;

        public FieldSet(AlgorithmSettings algorithmNow)
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

            _robotColor = new Color()
            {
                A = 200,
                R = 130,
                G = 130,
                B = 180
            };

            Title = algorithmNow.algName;

            Init();
        }

        void Init()
        {
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

            Grid.SetRow(_robotGrid, _algorithmNow.robot.row);
            Grid.SetColumn(_robotGrid, _algorithmNow.robot.column);
        }

        public void CreateField()
        {
            _algorithmNow.field.colorList = new int[_algorithmNow.field.countGridX, _algorithmNow.field.countGridY];

            MainGrid.Children.Clear();
            MainGrid.ColumnDefinitions.Clear();
            MainGrid.RowDefinitions.Clear();

            _colorGrid = new Grid[_algorithmNow.field.countGridX, _algorithmNow.field.countGridY];
            _robotGrid = new Grid();

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

            _robotGrid.Background = new SolidColorBrush(_robotColor);

            MainGrid.Children.Add(_robotGrid);
        }
    }
}
