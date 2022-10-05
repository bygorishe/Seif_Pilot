using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Seif_Pilot
{
    public partial class MainWindow : Window
    {
        int N;
        List<List<Button>> buttonList;
        List<List<bool>> buttonMap; 

        public MainWindow()
        {
            InitializeComponent();
            Start();
        }

        private void Restart(object sender, RoutedEventArgs e) => Start();
        private void Start()
        {
            MainGrid.ColumnDefinitions.Clear();
            MainGrid.RowDefinitions.Clear();
            MainGrid.Children.Clear();
            MainGrid.IsEnabled = true;

            N = int.Parse(NBox.Text);

            buttonList = new List<List<Button>>(N);
            buttonMap = new List<List<bool>>(N); 

            for (int i = 0; i < N; i++)
            {
                buttonMap.Add(new List<bool>(N));
                buttonList.Add(new List<Button>(N));
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition());
                MainGrid.RowDefinitions.Add(new RowDefinition());
            }

            Random random = new Random();

            if (N % 2 == 0)
            {
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        buttonMap[i].Add((random.Next(0, 2) < 1));

                        buttonList[i].Add(new Button()
                        {
                            Background = (buttonMap[i][j]) ? Brushes.Green : Brushes.Yellow,
                            BorderThickness = new Thickness(1),
                            Content = (buttonMap[i][j]) ? '|' : '—'
                        });
                        buttonList[i][j].Click += ReverseClick;
                        MainGrid.Children.Add(buttonList[i][j]);
                        Grid.SetColumn(buttonList[i][j], j);
                        Grid.SetRow(buttonList[i][j], i);
                    }
                }
            }
            else //для нечетных нельзя рандомно заполнять поле(
            {
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        buttonMap[i].Add(true);

                        buttonList[i].Add(new Button()
                        {
                            Background = Brushes.Green,
                            BorderThickness = new Thickness(1),
                            Content = '|' 
                        });
                        buttonList[i][j].Click += ReverseClick;
                        MainGrid.Children.Add(buttonList[i][j]);
                        Grid.SetColumn(buttonList[i][j], j);
                        Grid.SetRow(buttonList[i][j], i);
                    }
                }

                for (int i = 0; i < N; i++)
                    Reverse(random.Next(0, N), random.Next(0, N));
            }
            WinCheck();
        }

        private void SwapContent(Button button)
        {
            button.Content = button.Content.Equals('|') ? '—' : '|';
            button.Background = button.Background.Equals(Brushes.Yellow) ? Brushes.Green : Brushes.Yellow;
        }

        private void Reverse(int x, int y)
        {
            for (int i = 0; i < N; i++)
            {
                if (i == y) continue;
                SwapContent(buttonList[x][i]);
                buttonMap[x][i] = !buttonMap[x][i];
            }

            for (int j = 0; j < N; j++)
            {
                SwapContent(buttonList[j][y]);
                buttonMap[j][y] = !buttonMap[j][y];
            }
        }

        private void ReverseClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int x = 0, y = 0;
            for (; x < N; x++)
                if (buttonList[x].Contains(button))
                {
                    y = buttonList[x].IndexOf(button);
                    break;
                }

            Reverse(x,y);

            WinCheck();
        }

        private bool WinCheck()
        {
            bool flag = !buttonMap[0][0];
            foreach (var c in buttonMap)
                if (c.Contains(flag))
                    return false;

            MessageBox.Show("WIN");
            MainGrid.IsEnabled = false;
            return true;
        }
    }
}
