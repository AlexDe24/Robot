using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Robot.Logic;
//using System.Timers;
using System.Threading;

namespace Robot.Form
{
    /// <summary>
    /// Логика взаимодействия для Field.xaml
    /// </summary>
    public partial class Field : Window
    {
        TimerCallback _timeCB;
        Timer _stepTimer;

        FileClass _fileWork;
        AlgSettings _settings;
        Action _action;

        Color _blackColor;
        Color _whiteColor;
        Color _robotColor;

        int[,] _colorList;
        Grid[,] _colorGrid;
        Grid _robot;

        int _countGrid;

        int[] _rowColumn;

        int _step;

        bool isTimer;

        public Field()
        {
            InitializeComponent();

            _timeCB = new TimerCallback(TimerTick); //функция таймера
            _stepTimer = new Timer(TimerTick, null, 100, 700); //таймер

            _rowColumn = new int[2];

            _fileWork = new FileClass(); //класс работы с файлами
            _settings = new AlgSettings(); //класс настроек 
            _action = new Action(); //класс управления

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

            _colorList = new int[100, 100];
            isTimer = false;
            _step = 0;

            _settings = _fileWork.ReadAlgoritm();

            _countGrid = _settings.countGrid; 

            CreateField();
        }

        void CreateField()
        {
            MainGrid.Children.Clear();

            _colorGrid = new Grid[100, 100];
            _robot = new Grid();

            for (int i = 0; i < _countGrid; i++)
            {
                RowDefinition rd = new RowDefinition();
                ColumnDefinition cd = new ColumnDefinition();

                MainGrid.RowDefinitions.Add(rd);
                MainGrid.ColumnDefinitions.Add(cd);
            }

            for (int i = 0; i < _countGrid; i++)
            {
                for (int j = 0; j < _countGrid; j++)
                {
                    Grid bton = new Grid();

                    bton.Background = new SolidColorBrush(_whiteColor);

                    MainGrid.Children.Add(bton);

                    Grid.SetRow(bton, i);
                    Grid.SetColumn(bton, j);

                    _colorList[i, j] = 0;
                    _colorGrid[i, j] = bton;
                }
            }

            for (int i = 0; i < _countGrid; i++)
            {
                _colorList[0, i] = 1;
                _colorList[_countGrid - 1, i] = 1;
                _colorGrid[0, i].Background = new SolidColorBrush(_blackColor);
                _colorGrid[_countGrid - 1, i].Background = new SolidColorBrush(_blackColor);

                _colorList[i, 0] = 1;
                _colorList[i, _countGrid - 1] = 1;
                _colorGrid[i, 0].Background = new SolidColorBrush(_blackColor);
                _colorGrid[i, _countGrid - 1].Background = new SolidColorBrush(_blackColor);
            }

            _robot.Background = new SolidColorBrush(_robotColor);

            MainGrid.Children.Add(_robot);
        }

        void TimerTick(object state)
        {
            if (isTimer == true)
            {
                Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    Grid.SetRow(_robot, _settings.row);
                    Grid.SetColumn(_robot, _settings.column);
                }));
                
                _step = _action.Move(_settings, _step, _colorList);
            }
        }


        private void Step_Click(object sender, RoutedEventArgs e)
        {
            Grid.SetRow(_robot, _settings.row);
            Grid.SetColumn(_robot, _settings.column);

            _step = _action.Move(_settings, _step, _colorList);
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            isTimer = true;
        }
    }
}
