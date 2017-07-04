using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Threading;
using System.Linq;
using Robot.Logic;

namespace Robot.Form
{
    /// <summary>
    /// Логика взаимодействия для Field.xaml
    /// </summary>
    public partial class Field : Window
    {
        TimerCallback _timeCB;
        Timer _stepTimer;

        List<AlgorithmSettings> _algorithms;
        AlgorithmSettings _algorithmNow;

        FileClass _fileWork;
        Action _action;
        CreateAlg _createAlg;

        Color _blackColor;
        Color _whiteColor;
        Color _robotColor;

        Grid[,] _colorGrid;
        Grid _robotGrid;

        int _step;

        bool isTimer;

        public Field()
        {
            InitializeComponent();

            _timeCB = new TimerCallback(TimerTick); //функция таймера
            _stepTimer = new Timer(TimerTick, null, 0, 1); //таймер

            _fileWork = new FileClass(); //класс работы с файлами
            _action = new Action(); //класс управления

            UpdateAlgoBox();

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

            isTimer = false;
        }

        void UpdateAlgoBox()
        {
            _algorithms = new List<AlgorithmSettings>();
            _algorithms = _fileWork.Readalgorithms();

            AlgoBox.Items.Clear();

            for (int i = 0; i < _algorithms.Count; i++)
            {
                AlgoBox.Items.Add(_algorithms[i].algName);
            }
        }

        void CreateField()
        {
            MainGrid.Children.Clear();
            MainGrid.ColumnDefinitions.Clear();
            MainGrid.RowDefinitions.Clear();

            _step = 0;

            _colorGrid = new Grid[100, 100];
            _robotGrid = new Grid();

            for (int i = 0; i < _algorithmNow.field.countGrid; i++)
            {
                RowDefinition rd = new RowDefinition();
                ColumnDefinition cd = new ColumnDefinition();

                MainGrid.RowDefinitions.Add(rd);
                MainGrid.ColumnDefinitions.Add(cd);
            }

            for (int i = 0; i < _algorithmNow.field.countGrid; i++)
            {
                for (int j = 0; j < _algorithmNow.field.countGrid; j++)
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

            for (int i = 0; i < _algorithmNow.field.countGrid; i++)
            {
                _algorithmNow.field.colorList[0, i] = 1;
                _algorithmNow.field.colorList[_algorithmNow.field.countGrid - 1, i] = 1;
                _colorGrid[0, i].Background = new SolidColorBrush(_blackColor);
                _colorGrid[_algorithmNow.field.countGrid - 1, i].Background = new SolidColorBrush(_blackColor);

                _algorithmNow.field.colorList[i, 0] = 1;
                _algorithmNow.field.colorList[i, _algorithmNow.field.countGrid - 1] = 1;
                _colorGrid[i, 0].Background = new SolidColorBrush(_blackColor);
                _colorGrid[i, _algorithmNow.field.countGrid - 1].Background = new SolidColorBrush(_blackColor);
            }

            _robotGrid.Background = new SolidColorBrush(_robotColor);

            MainGrid.Children.Add(_robotGrid);
        }

        void TimerTick(object state)
        {
            if (isTimer == true)
            {
                Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    RobotActive();
                }));
            }
        }

        private void Step_Click(object sender, RoutedEventArgs e)
        {
            RobotActive();
        }

        void RobotActive()
        {
            if (_step >= 0)
            {
                Grid.SetRow(_robotGrid, _algorithmNow.robot.row);
                Grid.SetColumn(_robotGrid, _algorithmNow.robot.column);

                for (int i = 0; i < _algorithmNow.field.countGrid; i++)
                {
                    for (int j = 0; j < _algorithmNow.field.countGrid; j++)
                    {
                        if (_algorithmNow.field.colorList[i, j] == 0)
                        {
                            _colorGrid[i, j].Background = new SolidColorBrush(_whiteColor);
                        }
                        else
                            _colorGrid[i, j].Background = new SolidColorBrush(_blackColor);
                    }
                }

                _step = _action.Move(_step, _algorithmNow);
            }
            else 
            {
                isTimer = false;
                MessageBox.Show("Алгоритм завершён.");
            }
            
        }
        private void Start_Click(object sender, RoutedEventArgs e)
        {

            if (AlgoBox.SelectedIndex != -1)
            {
                int k = AlgoBox.SelectedIndex;

                UpdateAlgoBox();

                AlgoBox.SelectedIndex = k;

                _algorithmNow = _algorithms.Where(x => x.algName == AlgoBox.Items[k]).First();

                Mod.IsEnabled = true;
                Step.IsEnabled = true;
                isTimer = false;

                CreateField();
            }
        }

        private void Mod_Click(object sender, RoutedEventArgs e)
        {
            
            if (isTimer == true)
            {
                isTimer = false;
                Mod.Content = "Пошагово Вкл.";
            }
            else if (isTimer == false)
            {
                isTimer = true;
                Mod.Content = "Пошагово Выкл.";
                }
        }

        private void AlgoCreate_Click(object sender, RoutedEventArgs e)
        {
            List<string> algNames = new List<string>();

            for (int i = 0; i < _algorithms.Count; i++)
            {
                algNames.Add(_algorithms[i].algName);
            }
                
            _createAlg = new CreateAlg(_fileWork, algNames);

            _createAlg.ShowDialog();

            UpdateAlgoBox();
        }
    }
}
