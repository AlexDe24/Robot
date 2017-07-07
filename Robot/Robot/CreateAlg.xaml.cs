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

        public CreateAlg(FileClass fileWork, AlgorithmSettings algorithm)
        {
            InitializeComponent();

            _fileWork = fileWork;//класс работы с файлами

            _algorithms = _fileWork.Readalgorithms();//все алгоритмы

            if (algorithm != null)
            {
                _algorithm = _fileWork.Readalgorithms().Where(x => x.algName == algorithm.algName).First();
                Load();
            }
            else
                _algorithm = new AlgorithmSettings();

            RoborRotate.Items.Add("Направо");
            RoborRotate.Items.Add("Вверх");
            RoborRotate.Items.Add("Налево");
            RoborRotate.Items.Add("Вниз");
        }

        void Load()
        {
            for (int i = 0; i < _algorithm.commands.Count; i++)
            {
                AlgList.Items.Add(AddCommand(
                    _algorithm.commands[i].name,
                    _algorithm.commands[i].firstArg,
                    _algorithm.commands[i].secondArg
                    ));
            }

            AlgName.Text = _algorithm.algName;

            FieldSizeX.Text = Convert.ToString(_algorithm.field.countGridX);
            FieldSizeY.Text = Convert.ToString(_algorithm.field.countGridY);
            RoborColumn.Text = Convert.ToString(_algorithm.robot.column);
            RoborRow.Text = Convert.ToString(_algorithm.robot.row);

            if (_algorithm.robot.rotate == "Направо")
                RoborRotate.SelectedIndex = 0;
            else if (_algorithm.robot.rotate == "Вверх")
                RoborRotate.SelectedIndex = 1;
            else if (_algorithm.robot.rotate == "Налево")
                RoborRotate.SelectedIndex = 2;
            else if (_algorithm.robot.rotate == "Вниз")
                RoborRotate.SelectedIndex = 3;
        }

        StackPanel AddCommand(string name, string firstArg, string secondArg)
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

            ComboBox CommandFirstArg = new ComboBox()
            {
                Height = 25,
                Width = 105,
            };

            ComboBox CommandSecondArg = new ComboBox()
            {
                Height = 25,
                Width = 105,
            };

            switch (name)
            {
                case "Движение":
                    CommandName.SelectedIndex = 0;
                    CommandName.ToolTip = "Движение вперёд";
                    CommandFirstArg.ToolTip = "Количество клеток";

                    CommandFirstArg.Items.Clear();
                    for (int j = 0; j < 15; j++)
                    {
                        CommandFirstArg.Items.Add(j);
                    }

                    CommandFirstArg.SelectedIndex = Convert.ToInt32(firstArg);

                    CommandSecondArg.ToolTip = "Номер следующей команды";
                    break;
                case "Поворот":
                    CommandName.SelectedIndex = 1;
                    CommandName.ToolTip = "Поворот";
                    CommandFirstArg.ToolTip = "Направление поворота";

                    CommandFirstArg.Items.Clear();

                    CommandFirstArg.Items.Add("Направо");
                    CommandFirstArg.Items.Add("Налево");

                    if (firstArg == "Налево")
                        CommandFirstArg.SelectedIndex = 0;
                    else
                        CommandFirstArg.SelectedIndex = 1;

                    CommandSecondArg.ToolTip = "Номер следующей команды";
                    break;
                case "Заливка":
                    CommandName.SelectedIndex = 2;
                    CommandName.ToolTip = "Закрашивание ячейки";
                    CommandFirstArg.ToolTip = "Цвет ячейки";

                    CommandFirstArg.Items.Clear();
                    CommandFirstArg.Items.Add("Белый");
                    CommandFirstArg.Items.Add("Чёрный");

                    if (firstArg == "Белый")
                        CommandFirstArg.SelectedIndex = 1;
                    else
                        CommandFirstArg.SelectedIndex = 0;

                    CommandSecondArg.ToolTip = "Номер следующей команды";
                    break;
                case "Изучение":
                    CommandName.SelectedIndex = 3;
                    CommandName.ToolTip = "Выбор действия в зависимости от цвета блока";
                    CommandFirstArg.ToolTip = "Номер команды, если текущая ячейка белая";

                    CommandFirstArg.Items.Clear();
                    for (int j = -1; j <= AlgList.Items.Count; j++)
                    {
                        CommandFirstArg.Items.Add(j);
                    }

                    CommandFirstArg.SelectedIndex = Convert.ToInt32(firstArg) + 1;

                    CommandSecondArg.ToolTip = "Номер команды, если текущая ячейка чёрная";
                    break;
                default:
                    break;
            }

            CommandSecondArg.Items.Clear();
            for (int j = -1; j <= _algorithm.commands.Count; j++)
            {
                CommandSecondArg.Items.Add(j);
            }

            CommandSecondArg.SelectedIndex = Convert.ToInt32(secondArg) + 1;

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
                    ComboBox CommandFirstArg = ((AlgList.Items[i] as StackPanel).Children[2] as ComboBox);
                    ComboBox CommandSecondArg = ((AlgList.Items[i] as StackPanel).Children[3] as ComboBox);
                    int c = CommandFirstArg.SelectedIndex;

                    switch (CommandName.Items[CommandName.SelectedIndex])
                    {
                        case "Движение":
                            CommandName.ToolTip = "Движение вперёд";
                            CommandFirstArg.ToolTip = "Количество клеток";

                            CommandFirstArg.Items.Clear();
                            for (int j = 0; j < 15; j++)
                            {
                                CommandFirstArg.Items.Add(j);
                            }
                            CommandFirstArg.SelectedIndex = c;

                            CommandSecondArg.ToolTip = "Номер следующей команды";
                            break;
                        case "Поворот":
                            CommandName.ToolTip = "Поворот";
                            CommandFirstArg.ToolTip = "Направление поворота";

                            CommandFirstArg.Items.Clear();
                            CommandFirstArg.Items.Add("Налево");
                            CommandFirstArg.Items.Add("Направо");
                            CommandFirstArg.SelectedIndex = c;

                            CommandSecondArg.ToolTip = "Номер следующей команды";
                            break;
                        case "Заливка":
                            CommandName.ToolTip = "Закрашивание ячейки";
                            CommandFirstArg.ToolTip = "Цвет ячейки";

                            CommandFirstArg.Items.Clear();
                            CommandFirstArg.Items.Add("Чёрный");
                            CommandFirstArg.Items.Add("Белый");
                            CommandFirstArg.SelectedIndex = c;

                            CommandSecondArg.ToolTip = "Номер следующей команды";
                            break;
                        case "Изучение":
                            CommandName.ToolTip = "Выбор действия в зависимости от цвета блока";
                            CommandFirstArg.ToolTip = "Номер команды, если текущая ячейка белая";

                            CommandFirstArg.Items.Clear();
                            for (int j = -1; j <= AlgList.Items.Count; j++)
                            {
                                CommandFirstArg.Items.Add(j);
                            }
                            CommandFirstArg.SelectedIndex = c;

                            CommandSecondArg.ToolTip = "Номер команды, если текущая ячейка чёрная";
                            break;
                        default:
                            break;
                    }

                    int k = CommandSecondArg.SelectedIndex;

                    CommandSecondArg.Items.Clear();
                    for (int j = -1; j <= AlgList.Items.Count; j++)
                    {
                        CommandSecondArg.Items.Add(j);
                    }

                    CommandSecondArg.SelectedIndex = k;
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
                        firstArg = ((AlgList.Items[i] as StackPanel).Children[2] as ComboBox).Text,
                        secondArg = ((AlgList.Items[i] as StackPanel).Children[3] as ComboBox).Text
                    });
                }

                _algorithm.algName = AlgName.Text;
                _algorithm.field.countGridX = Convert.ToInt32(FieldSizeX.Text);
                _algorithm.field.countGridY = Convert.ToInt32(FieldSizeY.Text);
                _algorithm.robot.column = Convert.ToInt32(RoborColumn.Text);
                _algorithm.robot.row = Convert.ToInt32(RoborRow.Text);
                _algorithm.robot.rotate = RoborRotate.Text;
                
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
                if (_algorithms.Any(x => x.algName == AlgName.Text) && _algorithm.algName != AlgName.Text)
                {
                    MessageBox.Show("Название уровня занято!", "Внимание!");
                    error = true;
                }
            }

            if (error == false)
            {
                _fileWork.WriteAlgorithm(_algorithm);
            }
            else
                _algorithm.commands.Clear();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
