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
        TimerCallback timeCB;
        Timer stepsTimer;

        FileClass fileWork;
        AlgSettings settings;
        Action act;

        Color blackColor;
        Color whiteColor;
        Color robotColor;

        Grid[,] imageGrid;
        Grid robot;

        int _countGrid;

        int[] _startRowColumn;

        public delegate void InvokeDelegate();

        public Field()
        {
            InitializeComponent();

            timeCB = new TimerCallback(TimerTick);

            stepsTimer = new Timer(timeCB,null,0,100);

            fileWork = new FileClass();
            settings = new AlgSettings();
            act = new Action();

            _countGrid = 15;

            _startRowColumn = new int[2];

            blackColor = new Color()
            {
                A = 200,
                R = 0,
                G = 0,
                B = 0
            };

            whiteColor = new Color()
            {
                A = 0,
                R = 100,
                G = 100,
                B = 100
            };

            robotColor = new Color()
            {
                A = 200,
                R = 130,
                G = 130,
                B = 180
            };

            CreateField();
        }

        void CreateField()
        {
            MainGrid.Children.Clear();

            imageGrid = new Grid[100, 100];
            robot = new Grid();

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

                    bton.Background = new SolidColorBrush(whiteColor);

                    MainGrid.Children.Add(bton);

                    Grid.SetRow(bton, i);
                    Grid.SetColumn(bton, j);

                    imageGrid[i, j] = bton;
                }
            }

            for (int i = 0; i < _countGrid; i++)
            {
                imageGrid[0, i].Background = new SolidColorBrush(blackColor);
                imageGrid[_countGrid - 1, i].Background = new SolidColorBrush(blackColor);

                imageGrid[i, 0].Background = new SolidColorBrush(blackColor);
                imageGrid[i, _countGrid - 1].Background = new SolidColorBrush(blackColor);
            }

            robot.Background = new SolidColorBrush(robotColor);

            MainGrid.Children.Add(robot);
        }

        void TimerTick(object state)
        {
            InvokeDelegate handler = new InvokeDelegate(InvokeMethod);
            handler.BeginInvoke(null,null);
            //_startRow = act.Move(_startRow);
            //Grid.SetRow(robot, _startRow);
        }

        public void InvokeMethod()
        {
            _startRowColumn = act.Move(_startRowColumn,_countGrid, settings.commands);
            Grid.SetRow(robot, _startRowColumn[0]);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Step_Click(object sender, RoutedEventArgs e)
        {
            Grid.SetRow(robot, _startRowColumn[0]);
            Grid.SetColumn(robot, _startRowColumn[1]);
        }
    }
}
