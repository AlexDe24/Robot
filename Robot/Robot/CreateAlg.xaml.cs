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
    /// Логика взаимодействия для CreateAlg.xaml
    /// </summary>
    public partial class CreateAlg : Window
    {
        FileClass _fileWork;
        AlgorithmSettings _algorithm;

        List<string> _algNames;
        public CreateAlg(FileClass fileWork, List<string> algNames)
        {
            InitializeComponent();

            _algNames = algNames;

            _algorithm = new AlgorithmSettings();
            _fileWork = fileWork;
        }

        StackPanel AddCommand()
        {
            TextBox CommandNom = new TextBox()
            {
                Text = Convert.ToString(AlgBox.Items.Count),
                IsEnabled = false,
                Height = 25,
                Width = 105,
                ToolTip = "Номер команды"
            };

            ComboBox CommandName = new ComboBox()
            {
                Height = 25,
                Width = 95,
                SelectedIndex = 0,
                ToolTip = "Движение вперёд"                
            };
            CommandName.SelectionChanged += SelectionChanged;
            CommandName.Items.Add("Движение");
            CommandName.Items.Add("Поворот");
            CommandName.Items.Add("Заливка");
            CommandName.Items.Add("Изучение");

            TextBox CommandFirstArg = new TextBox()
            {
                Height = 25,
                Width = 105,
                ToolTip = "Количество клеток"
            };

            TextBox CommandSecondArg = new TextBox()
            {
                Height = 25,
                Width = 105,
                ToolTip = "Номер следующей команды"
            };

            StackPanel Command = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Width = 420,
            };

            Command.Children.Add(CommandNom);
            Command.Children.Add(CommandName);
            Command.Children.Add(CommandFirstArg);
            Command.Children.Add(CommandSecondArg);

            return Command;
        }

        private void AddCom_Click(object sender, RoutedEventArgs e)
        {
            AlgBox.Items.Add(AddCommand());
        }

        private void DelCom_Click(object sender, RoutedEventArgs e)
        {
            if (AlgBox.SelectedIndex != -1)
            {
                AlgBox.Items.RemoveAt(AlgBox.SelectedIndex);
            }
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AlgBox.Items.Count != 0)
            {
                for (int i = 0; i < AlgBox.Items.Count; i++)
                {
                    ComboBox CommandName = ((AlgBox.Items[i] as StackPanel).Children[1] as ComboBox);
                    TextBox CommandFirstArg = ((AlgBox.Items[i] as StackPanel).Children[2] as TextBox);
                    TextBox CommandSecondArg = ((AlgBox.Items[i] as StackPanel).Children[3] as TextBox);

                    switch (CommandName.Items[CommandName.SelectedIndex])
                    {
                        case "Движение":
                            CommandName.ToolTip = "Движение вперёд";
                            CommandFirstArg.ToolTip = "Количество клеток";
                            CommandSecondArg.ToolTip = "Номер следующей команды";
                            break;
                        case "Поворот":
                            CommandName.ToolTip = "Поворот";
                            CommandFirstArg.ToolTip = "Направление поворота 1 – налево 0 – направо";
                            CommandSecondArg.ToolTip = "Номер следующей команды";
                            break;
                        case "Заливка":
                            CommandName.ToolTip = "Закрашивание ячейки";
                            CommandFirstArg.ToolTip = "Цвет ячейки 1 – черный 0 – белый";
                            CommandSecondArg.ToolTip = "Номер следующей команды";
                            break;
                        case "Изучение":
                            CommandName.ToolTip = "Выбор действия в зависимости от цвета блока";
                            CommandFirstArg.ToolTip = "Номер команды, если текущая ячейка белая";
                            CommandSecondArg.ToolTip = "Номер команды, если текущая ячейка чёрная";
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            bool error = false;

            try
            {
                for (int i = 0; i < AlgBox.Items.Count; i++)
                {
                    _algorithm.commands.Add(new Commands
                    {
                        nom = Convert.ToInt32(((AlgBox.Items[i] as StackPanel).Children[0] as TextBox).Text),
                        name = ((AlgBox.Items[i] as StackPanel).Children[1] as ComboBox).Text,
                        firstArg = Convert.ToInt32(((AlgBox.Items[i] as StackPanel).Children[2] as TextBox).Text),
                        secondArg = Convert.ToInt32(((AlgBox.Items[i] as StackPanel).Children[3] as TextBox).Text)
                    });
                }

                _algorithm.algName = AlgName.Text;
                _algorithm.field.countGrid = Convert.ToInt32(FieldSize.Text);
                _algorithm.robot.column = Convert.ToInt32(RoborColumn.Text);
                _algorithm.robot.row = Convert.ToInt32(RoborRow.Text);
                _algorithm.robot.rotate = Convert.ToInt32(RoborRotate.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Некоторые окна не заполнены!", "Внимание!");
                error = true;
            }

            if (AlgName.Text == "")
            {
                MessageBox.Show("Введите название уровня!", "Внимание!");
                error = true;
            }
            else
            {
                if (_algNames.Any(x => x == AlgName.Text))
                {
                    MessageBox.Show("Название уровня занято!", "Внимание!");
                    error = true;
                }
            }

            if (error == false)
            {
                _fileWork.WriteAlgorithm(_algorithm);
                Close();
            }
            else
                _algorithm.commands.Clear();
        }
    }
}
