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

        List<AlgorithmSettings> _algorithms;

        public CreateAlg(FileClass fileWork)
        {
            InitializeComponent();

            _fileWork = fileWork;//класс работы с файлами

            _algorithms = _fileWork.Readalgorithms();//все алгоритмы

            _algorithm = new AlgorithmSettings();//работающий в данный момент алгоритм

            RoborRotate.Items.Add("0");
            RoborRotate.Items.Add("90");
            RoborRotate.Items.Add("180");
            RoborRotate.Items.Add("270");

            for (int i = 0; i < _algorithms.Count; i++)
            {
                AlgBox.Items.Add(_algorithms[i].algName);
            }
        }

        StackPanel AddCommand(string name, string firsArg, string secondArg)
        {
            ComboBox CommandName = new ComboBox()
            {
                Height = 25,
                Width = 95,
                SelectedIndex = 0,
            };

            CommandName.SelectionChanged += SelectionChanged;
            CommandName.Items.Add("Движение");
            CommandName.Items.Add("Поворот");
            CommandName.Items.Add("Заливка");
            CommandName.Items.Add("Изучение");

            
            TextBox CommandNom = new TextBox()
            {
                Text = Convert.ToString(AlgList.Items.Count),
                IsEnabled = false,
                Height = 25,
                Width = 105,
            };

            TextBox CommandFirstArg = new TextBox()
            {
                Text = firsArg,
                Height = 25,
                Width = 105,
            };

            TextBox CommandSecondArg = new TextBox()
            {
                Text = secondArg,
                Height = 25,
                Width = 105,
            };

            switch (name)
            {
                case "Движение":
                    CommandName.SelectedIndex = 0;
                    CommandName.ToolTip = "Движение вперёд";
                    CommandFirstArg.ToolTip = "Количество клеток";
                    CommandSecondArg.ToolTip = "Номер следующей команды";
                    break;
                case "Поворот":
                    CommandName.SelectedIndex = 1;
                    CommandName.ToolTip = "Поворот";
                    CommandFirstArg.ToolTip = "Направление поворота 1 – налево 0 – направо";
                    CommandSecondArg.ToolTip = "Номер следующей команды";
                    break;
                case "Заливка":
                    CommandName.SelectedIndex = 2;
                    CommandName.ToolTip = "Закрашивание ячейки";
                    CommandFirstArg.ToolTip = "Цвет ячейки 1 – черный 0 – белый";
                    CommandSecondArg.ToolTip = "Номер следующей команды";
                    break;
                case "Изучение":
                    CommandName.SelectedIndex = 3;
                    CommandName.ToolTip = "Выбор действия в зависимости от цвета блока";
                    CommandFirstArg.ToolTip = "Номер команды, если текущая ячейка белая";
                    CommandSecondArg.ToolTip = "Номер команды, если текущая ячейка чёрная";
                    break;
                default:
                    break;
            }

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
            AlgList.Items.Add(AddCommand("Движение", null,null));
        }

        private void DelCom_Click(object sender, RoutedEventArgs e)
        {
            if (AlgList.SelectedIndex != -1)
            {
                AlgList.Items.RemoveAt(AlgList.SelectedIndex);

                for (int i = 0; i < AlgList.Items.Count; i++)
                {
                    TextBox CommandNom = ((AlgList.Items[i] as StackPanel).Children[0] as TextBox);

                    CommandNom.Text = Convert.ToString(i);
                }
            }
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AlgList.Items.Count != 0)
            {
                for (int i = 0; i < AlgList.Items.Count; i++)
                {
                    ComboBox CommandName = ((AlgList.Items[i] as StackPanel).Children[1] as ComboBox);
                    TextBox CommandFirstArg = ((AlgList.Items[i] as StackPanel).Children[2] as TextBox);
                    TextBox CommandSecondArg = ((AlgList.Items[i] as StackPanel).Children[3] as TextBox);

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
                _algorithm.commands.Clear();

                for (int i = 0; i < AlgList.Items.Count; i++)
                {
                    _algorithm.commands.Add(new Commands
                    {
                        nom = Convert.ToInt32(((AlgList.Items[i] as StackPanel).Children[0] as TextBox).Text),
                        name = ((AlgList.Items[i] as StackPanel).Children[1] as ComboBox).Text,
                        firstArg = Convert.ToInt32(((AlgList.Items[i] as StackPanel).Children[2] as TextBox).Text),
                        secondArg = Convert.ToInt32(((AlgList.Items[i] as StackPanel).Children[3] as TextBox).Text)
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
                if (AlgBox.SelectedIndex != -1)
                {
                    if (_algorithms[AlgBox.SelectedIndex].algName == AlgName.Text)
                    {

                    }
                }
                else if (_algorithms.Any(x => x.algName == AlgName.Text))
                {
                    MessageBox.Show("Название уровня занято!", "Внимание!");
                    error = true;
                }
            }

            if (error == false)
            {
                _fileWork.WriteAlgorithm(_algorithm);
                //Close();
            }
            else
                _algorithm.commands.Clear();
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            if (AlgBox.SelectedIndex != -1)
            {               
                _fileWork.DelAlgorithm(_algorithms[AlgBox.SelectedIndex].algName);
                AlgBox.Items.RemoveAt(AlgBox.SelectedIndex);

                _algorithms = _fileWork.Readalgorithms();

                AlgList.Items.Clear();

                AlgName.Text = "";

                FieldSize.Text = "";
                RoborColumn.Text = "";
                RoborRow.Text = "";
                RoborRotate.Text = "";
            }
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            if (AlgBox.SelectedIndex != -1)
            {
                AlgList.Items.Clear();

                for (int i = 0; i < _algorithms[AlgBox.SelectedIndex].commands.Count; i++)
                {
                    AlgList.Items.Add(AddCommand(
                        _algorithms[AlgBox.SelectedIndex].commands[i].name,
                        Convert.ToString(_algorithms[AlgBox.SelectedIndex].commands[i].firstArg),
                        Convert.ToString(_algorithms[AlgBox.SelectedIndex].commands[i].secondArg)
                        ));
                }

                AlgName.Text = _algorithms[AlgBox.SelectedIndex].algName;

                FieldSize.Text = Convert.ToString(_algorithms[AlgBox.SelectedIndex].field.countGrid);
                RoborColumn.Text = Convert.ToString(_algorithms[AlgBox.SelectedIndex].robot.column);
                RoborRow.Text = Convert.ToString(_algorithms[AlgBox.SelectedIndex].robot.row);
                RoborRotate.Text = Convert.ToString(_algorithms[AlgBox.SelectedIndex].robot.rotate);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
