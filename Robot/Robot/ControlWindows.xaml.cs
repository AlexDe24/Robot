using System;
using System.IO;
using System.Linq;
using System.Windows;
using Robot.Logic;
using System.Collections.Generic;

namespace Robot.Form
{
    /// <summary>
    /// Родительская форма
    /// </summary>
    public partial class ControlWindows : Window
    {
        AlgorinthmsControl _control; //класс управления алгоритмами

        public ControlWindows()
        {
            InitializeComponent();

            ControlShow();
        }

        private void ControlShow()
        {
            _control = new AlgorinthmsControl(this);

            _control.Show(DockManager);
            _control.Activate();            
        }

        public void CreateFieldShow(FieldControl fieldControl)
        {
            fieldControl.Show(DockManager);
            fieldControl.Activate();                     
        }

        public void FieldShow(Field field)
        {
            field.Show(DockManager);
            field.Activate();
        }

        private void CreateAlg_Click(object sender, RoutedEventArgs e)
        {
            _control.AlgoCreate_Click(null, null);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AlgPanel_Click(object sender, RoutedEventArgs e)
        {
            _control.Show(DockManager);
            _control.Activate();
        }
    }
}
