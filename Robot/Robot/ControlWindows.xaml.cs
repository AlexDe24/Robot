using System;
using System.IO;
using System.Linq;
using System.Windows;
using Robot.Logic;
using System.Collections.Generic;

namespace Robot.Form
{
    /// <summary>
    /// Логика взаимодействия для ControlWindows.xaml
    /// </summary>
    public partial class ControlWindows : Window
    {
        

        public ControlWindows()
        {
            InitializeComponent();

            CreateControl();
        }

        private void CreateControl()
        {
            Field control = new Field(this);

            control.Show(DockManager);
            control.Activate();            
        }

        public void CreateField(FieldSet fields)
        {
            fields.CreateField();

            fields.Show(DockManager);
            fields.Activate();
        }
    }
}
