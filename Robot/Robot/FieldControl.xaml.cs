using Robot.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Robot.Form
{
    /// <summary>
    /// форма управления полем
    /// </summary>
    public partial class FieldControl
    {
        FileClass _fileWork; //класс работы с файлами
        ControlWindows _controlWindow; //родительсая форма 
        Logic.Action _action; //класс управления
        Field _field; //форма поля
        AlgorithmSettings _algorithmNow; //рабочий алгоритм

        Timer _stepTimer; //таймер

        bool _isTimerEnabled; //включённось таймера

        int _step; //шаг
        int _colv; //номер окна
        string _algorithmNowName; //название уровня

        public FieldControl(string algorithmNowName, FileClass fileWork, ControlWindows controlWindow, int colv)
        {
            InitializeComponent();

            _fileWork = fileWork;
            _controlWindow = controlWindow;
            _algorithmNowName = algorithmNowName;

            Speed.Value = 0.5;
            _isTimerEnabled = false;
            _colv = colv;

            _action = new Logic.Action();
            _stepTimer = new Timer(TimerTick, null, 0, 150); //таймер

            Title = "Управление алг: " + _algorithmNowName + "(" + colv + ")";

            Start();

            DockableStyle = AvalonDock.DockableStyle.Single;
        }

        void Start()
        {
            _step = 1;

            _algorithmNow = _fileWork.ReadAlgorithm(_algorithmNowName);

            _field = new Field(_algorithmNow, _colv);
                        
            _controlWindow.CreateFieldShow(this);
            _controlWindow.FieldShow(_field);

            AlgView.Items.Clear();

            for (int i = 0; i < _algorithmNow.commands.Count; i++)
            {
                AlgView.Items.Add(_algorithmNow.commands[i]);
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
                if (_step > 0)
                {
                    AlgView.SelectedIndex = _step;
                    _step = _action.Doing(_step, _algorithmNow);

                    if (_algorithmNow.robot.row >= _algorithmNow.field.countGridX || _algorithmNow.robot.column >= _algorithmNow.field.countGridY)
                    {
                        _algorithmNow.robot.row = -1;
                    }

                    _field.FieldUpdate();
                }
                else
                {
                    _isTimerEnabled = false;
                    MessageBox.Show("Алгоритм завершён.");
                }
            }
            catch (Exception)
            {
                _isTimerEnabled = false;
                MessageBox.Show("Ошибка алгоритма!", "Внимание!");

            }
        }

        private void Speed_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            _stepTimer.Change(0, Convert.ToInt32(300 * (1.1 - Speed.Value)));
        }

        void TimerTick(object state)
        {
            if (_isTimerEnabled == true)
            {
                Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    RobotActive();
                }));
            }
        }

        private void Mod_Click(object sender, RoutedEventArgs e)
        {

            if (_isTimerEnabled == true)
            {
                _isTimerEnabled = false;
                Mod.Content = "Автоматически";
            }
            else if (_isTimerEnabled == false)
            {
                _isTimerEnabled = true;
                Mod.Content = "Пошагово";
            }
        }

        private void FieldVis_Click(object sender, RoutedEventArgs e)
        {
            _controlWindow.FieldShow(_field);
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            _field.Close();
            Start();
        }
    }
}
