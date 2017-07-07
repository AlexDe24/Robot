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
    public partial class Field
    {
        ControlWindows _controlForm;
        FieldSet _field;
        PermissionDel permission;

        Timer _stepTimer;

        List<AlgorithmSettings> _algorithms;
        AlgorithmSettings _algorithmNow;

        FileClass _fileWork;
        Logic.Action _action;
        CreateAlg _createAlg;

        int _step;

        bool isTimerEnabled;

        public Field(ControlWindows controlForm)
        {
            InitializeComponent();

            _controlForm = controlForm;

            _stepTimer = new Timer(TimerTick, null, 0, 150); //таймер

            _fileWork = new FileClass(); //класс работы с файлами
            _action = new Logic.Action(); //класс управления

            UpdateAlgoBox();

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
                    _step = _action.Doing(_step, _algorithmNow);

                    if (_algorithmNow.robot.row >= _algorithmNow.field.countGridX || _algorithmNow.robot.column >= _algorithmNow.field.countGridY)
                    {
                        _algorithmNow.robot.row = -1;
                    }

                    _field.FieldUpdate();
                    
                    
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

                _step = 0;


                _field = new FieldSet(_algorithmNow);
                
                _controlForm.CreateField(_field);  
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
