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
        ControlWindows _controlWin; //родительсая форма 
        Logic.Action _action; //класс управления
        Field _field; //форма поля
        AlgorithmSettings _algorithmNow; //рабочий алгоритм

        Timer _stepTimer; //таймер

        bool _isTimerEnabled; //включённось таймера

        int _step; //шаг

        public FieldControl(AlgorithmSettings algorithmNow, ControlWindows controlWin, int colv)
        {
            InitializeComponent();

            _field = new Field(algorithmNow, colv);

            _algorithmNow = algorithmNow;

            _controlWin = controlWin;
            _controlWin.CreateFieldShow(this);
            _controlWin.FieldShow(_field);

            Speed.Value = 0.5;

            _action = new Logic.Action();
            _stepTimer = new Timer(TimerTick, null, 0, 150); //таймер

            _isTimerEnabled = false;

            _step = 1;

            Title = "Управление алг: " + algorithmNow.algName + "(" + colv + ")";

            for (int i = 0; i < algorithmNow.commands.Count; i++)
            {
                AlgView.Items.Add(algorithmNow.commands[i]);
            }

            DockableStyle = AvalonDock.DockableStyle.Single;
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
                Mod.Content = "Пошагово";
            }
            else if (_isTimerEnabled == false)
            {
                _isTimerEnabled = true;
                Mod.Content = "Автоматически";
            }
        }

        private void FieldVis_Click(object sender, RoutedEventArgs e)
        {
            _controlWin.FieldShow(_field);
        }
    }
}
