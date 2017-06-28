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

namespace Robot.Form
{
    /// <summary>
    /// Логика взаимодействия для Field.xaml
    /// </summary>
    public partial class Field : Window
    {
        Color blackColor;
        Color WhiteColor;

        Button[,] image;
        Button btn;
        int startRow;
        int startColumn;

        public Field()
        {
            InitializeComponent();

            blackColor = new Color()
            {
                A = 0,
                R = 100,
                G = 100,
                B = 100
            };

            WhiteColor = new Color()
            {
                A = 100,
                R = 100,
                G = 100,
                B = 100
            };

            image = new Button[200,200];

            startRow = 0;
            startColumn = 0;

            for (int i = 0; i < 15; i++)
            {
                RowDefinition rd = new RowDefinition();
                ColumnDefinition cd = new ColumnDefinition();

                MainGrid.RowDefinitions.Add(rd);
                MainGrid.ColumnDefinitions.Add(cd);
            }

            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    image[i,j].Background = new SolidColorBrush(WhiteColor);

                    MainGrid.Children.Add(image[0, 0]);
                    MainGrid.Children.Add(image[i, j]);

                    Grid.SetRow(image[i, j], i);
                    Grid.SetColumn(image[i, j], j);
                }
            }
            
            btn = new Button();

            MainGrid.Children.Add(btn);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            image[startRow, startColumn].Background = new SolidColorBrush(blackColor);

            switch (e.Key)
            {
                case Key.Down:
                    if (startRow < 15)
                        startRow++;
                    break;
                case Key.Up:
                    if (startRow > 0)
                        startRow--;
                    break;
                case Key.Left:
                    if (startColumn > 0)
                        startColumn--;
                    break;
                case Key.Right:
                    if (startColumn < 15)
                        startColumn++;
                    break;
                default:
                    break;
            }
            Grid.SetRow(btn, startRow);
            Grid.SetColumn(btn, startColumn);
        }
    }
}
