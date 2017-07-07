using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Threading;
using System.Linq;
using System;
using Robot.Logic;
using AvalonDock;

namespace Robot.Form
{
    /// <summary>
    /// Логика взаимодействия для Field.xaml
    /// </summary>
    public partial class Field : Window
    {
        PermissionDel permission;

        TimerCallback _timeCB;
        Timer _stepTimer;

        List<AlgorithmSettings> _algorithms;
        AlgorithmSettings _algorithmNow;

        FileClass _fileWork;
        Logic.Action _action;
        CreateAlg _createAlg;

        Color _blackColor;
        Color _whiteColor;
        Color _robotColor;

        Grid[,] _colorGrid;
        Grid _robotGrid;

        int _step;

        bool isTimerEnabled;

        public Field()
        {
            InitializeComponent();

            _timeCB = new TimerCallback(TimerTick); //функция таймера
            _stepTimer = new Timer(TimerTick, null, 0, 150); //таймер

            _fileWork = new FileClass(); //класс работы с файлами
            _action = new Logic.Action(); //класс управления

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

            Speed.Value = 0.5;

            isTimerEnabled = false;
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
            _algorithmNow.field.colorList = new int[_algorithmNow.field.countGridX, _algorithmNow.field.countGridY];

            MainGrid.Children.Clear();
            MainGrid.ColumnDefinitions.Clear();
            MainGrid.RowDefinitions.Clear();

            _step = 0;

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

        void TimerTick(object state)
        {
            if (isTimerEnabled == true)
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
            try
            {
                if (_step >= 0)
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

                    _step = _action.Doing(_step, _algorithmNow);

                    if (_algorithmNow.robot.row >= _algorithmNow.field.countGridX || _algorithmNow.robot.column >= _algorithmNow.field.countGridY)
                    {
                        _algorithmNow.robot.row = -1;
                    }

                    Grid.SetRow(_robotGrid, _algorithmNow.robot.row);
                    Grid.SetColumn(_robotGrid, _algorithmNow.robot.column);
                    
                }
                else
                {
                    isTimerEnabled = false;
                    MessageBox.Show("Алгоритм завершён.");
                }
            }
            catch (Exception)
            {
                isTimerEnabled = false;
                MessageBox.Show("Ошибка алгоритма!", "Внимание!");
                
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
                isTimerEnabled = false;

                CreateField();
            }
        }

        private void Mod_Click(object sender, RoutedEventArgs e)
        {
            
            if (isTimerEnabled == true)
            {
                isTimerEnabled = false;
                Mod.Content = "Пошагово Вкл.";
            }
            else if (isTimerEnabled == false)
            {
                isTimerEnabled = true;
                Mod.Content = "Пошагово Выкл.";
                }
        }

        private void AlgoCreate_Click(object sender, RoutedEventArgs e)
        {
            UpdateAlgoBox();

            _createAlg = new CreateAlg(_fileWork, null);
            _createAlg.ShowDialog();

            UpdateAlgoBox();
        }

        private void Speed_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            _stepTimer.Change(0, Convert.ToInt32(300 * (1.1 - Speed.Value)));
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AlgoRedact_Click(object sender, RoutedEventArgs e)
        {
            if (AlgoBox.SelectedIndex != -1)
            {
                _createAlg = new CreateAlg(_fileWork, _algorithms.Where(x => x.algName == AlgoBox.Items[AlgoBox.SelectedIndex]).First());
                _createAlg.ShowDialog();

                UpdateAlgoBox();
            }
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            if (AlgoBox.SelectedIndex != -1)
            {
                permission = new PermissionDel();
                if (permission.ShowDialog() == true)
                {
                    _fileWork.DelAlgorithm(AlgoBox.Items[AlgoBox.SelectedIndex] as string);
                    UpdateAlgoBox();
                }
            }
        }
    }
}
