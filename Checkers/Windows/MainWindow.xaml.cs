using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace Checkers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /*Всё находящееся здесь сделано Золотаревым Алексеем группы ИП-202
        Названия переменных соответсвуют их назначению*/
        int countx = 0, county = 0, countb = 0, countw = 0;
        bool racism = true, continueturn = true;

        public MainWindow()
        {
            InitializeComponent();

            CreateMap();

            PlaceFigures();
        }

        /// <summary>
        /// Генерация
        /// </summary>
        public void CreateMap()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Rectangle rectangle = new Rectangle();
                    rectangle.Name = "Field" + i + j;
                    rectangle.Height = 100;
                    rectangle.Width = 100;

                    if (countx % 2 == 0)
                    {
                        rectangle.Fill = new SolidColorBrush(Colors.LightGray);
                        rectangle.Stroke = new SolidColorBrush(Colors.LightGray);
                    }

                    BackGrid.Children.Add(rectangle);

                    if (county % 2 == 0)
                    {
                        Grid.SetColumn(rectangle, j + 1);
                        Grid.SetRow(rectangle, i);
                    }
                    else
                    {
                        Grid.SetColumn(rectangle, j);
                        Grid.SetRow(rectangle, i);
                    }
                    rectangle.Drop += Rectangle_Drop;
                    rectangle.AllowDrop = true;
                    countx++;
                }
                county++;
            }
        }
        public void PlaceFigures()
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Ellipse ellipse = new Ellipse();
                    ellipse.Width = 90;
                    ellipse.Height = 90;

                    if (countb < 12)
                    {
                        ellipse.Fill = new SolidColorBrush(Colors.Black);
                        ellipse.Stroke = new SolidColorBrush(Colors.Black);
                        ellipse.Name = "Piece" + countb + "B";
                    }
                    else
                    {
                        ellipse.Fill = new SolidColorBrush(Colors.Wheat);
                        ellipse.Stroke = new SolidColorBrush(Colors.Wheat);
                        ellipse.Name = "Piece" + countw + "W";
                    }

                    ellipse.MouseMove += Ellipse_MouseMove;

                    GameGrid.Children.Add(ellipse);

                    if (countb < 12)
                    {
                        if (i % 2 == 0)
                        {
                            Grid.SetColumn(ellipse, j + j + 1);
                            Grid.SetRow(ellipse, i);
                        }
                        else
                        {
                            Grid.SetColumn(ellipse, j + j);
                            Grid.SetRow(ellipse, i);
                        }
                        countb++;
                    }
                    else
                    {
                        if (i % 2 == 0)
                        {
                            Grid.SetColumn(ellipse, j + j + 1);
                            Grid.SetRow(ellipse, i + 2);
                        }
                        else
                        {
                            Grid.SetColumn(ellipse, j + j);
                            Grid.SetRow(ellipse, i + 2);
                        }
                        countw++;
                    }
                }
            }
        }

        /// <summary>
        /// Рестарт
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            countx = 0; county = 0; countb = 0; countw = 0;
            GameGrid.Children.Clear();
            BackGrid.Children.Clear();
            CreateMap();
            PlaceFigures();
            Announcer.Visibility = Visibility.Hidden;
            Announcer.Content = " ";
            racism = true;
            TurnColor.Fill = new SolidColorBrush(Colors.Wheat);
            WinnerColor.Visibility = Visibility.Hidden;
            SkillIssue.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Векторы какие-то
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        public char GetVectorX(int num1, int num2)
        {
            switch (num1 - num2)
            {
                case 1:
                case 2:
                    return 'r';
                case -1:
                case -2:
                    return 'l';
                default:
                    return 'n';
            }
        }
        public char GetVectorY(int num1, int num2)
        {

            switch (num1 - num2)
            {
                case 1:
                case 2:
                    return 'd';
                case -1:
                case -2:
                    return 'u';
                default:
                    return 'n';
            }
        }

        /// <summary>
        /// Движка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_Drop(object sender, DragEventArgs e)
        {
            Ellipse ellipse = e.Data.GetData("CringeFormat") as Ellipse;

            MessageBox.Show(ellipse.Name.ToString());
        }
        private void Ellipse_MouseMove(object sender, MouseEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;

            if (ellipse != null && e.LeftButton == MouseButtonState.Pressed)
            {
                DataObject data = new DataObject("CringeFormat", ellipse);
                DragDrop.DoDragDrop(ellipse, data, DragDropEffects.Move);
            }
        }

        /*———————————No Switches?———————————
 ⠀           ⣞⢽⢪⢣⢣⢣⢫⡺⡵⣝⡮⣗⢷⢽⢽⢽⣮⡷⡽⣜⣜⢮⢺⣜⢷⢽⢝⡽⣝
            ⠸⡸⠜⠕⠕⠁⢁⢇⢏⢽⢺⣪⡳⡝⣎⣏⢯⢞⡿⣟⣷⣳⢯⡷⣽⢽⢯⣳⣫⠇
            ⠀⠀⢀⢀⢄⢬⢪⡪⡎⣆⡈⠚⠜⠕⠇⠗⠝⢕⢯⢫⣞⣯⣿⣻⡽⣏⢗⣗⠏⠀
            ⠀⠪⡪⡪⣪⢪⢺⢸⢢⢓⢆⢤⢀⠀⠀⠀⠀⠈⢊⢞⡾⣿⡯⣏⢮⠷⠁⠀⠀
            ⠀⠀⠀⠈⠊⠆⡃⠕⢕⢇⢇⢇⢇⢇⢏⢎⢎⢆⢄⠀⢑⣽⣿⢝⠲⠉⠀⠀⠀⠀
            ⠀⠀⠀⠀⠀⡿⠂⠠⠀⡇⢇⠕⢈⣀⠀⠁⠡⠣⡣⡫⣂⣿⠯⢪⠰⠂⠀⠀⠀⠀
            ⠀⠀⠀⠀⡦⡙⡂⢀⢤⢣⠣⡈⣾⡃⠠⠄⠀⡄⢱⣌⣶⢏⢊⠂⠀⠀⠀⠀⠀⠀
            ⠀⠀⠀⠀⢝⡲⣜⡮⡏⢎⢌⢂⠙⠢⠐⢀⢘⢵⣽⣿⡿⠁⠁⠀⠀⠀⠀⠀⠀⠀
            ⠀⠀⠀⠀⠨⣺⡺⡕⡕⡱⡑⡆⡕⡅⡕⡜⡼⢽⡻⠏⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
            ⠀⠀⠀⠀⣼⣳⣫⣾⣵⣗⡵⡱⡡⢣⢑⢕⢜⢕⡝⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
            ⠀⠀⠀⣴⣿⣾⣿⣿⣿⡿⡽⡑⢌⠪⡢⡣⣣⡟⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
            ⠀⠀⠀⡟⡾⣿⢿⢿⢵⣽⣾⣼⣘⢸⢸⣞⡟⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
            ⠀⠀⠀⠀⠁⠇⠡⠩⡫⢿⣝⡻⡮⣒⢽⠋⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
           —————————————————————————————*/

        /// <summary>
        /// Фундамент движки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rectangle_Drop(object sender, DragEventArgs e)
        {
            continueturn = true;
            Rectangle? rectangle = sender as Rectangle;
            Ellipse? ellipse = e.Data.GetData("CringeFormat") as Ellipse;

            int x1 = Grid.GetColumn(ellipse), y1 = Grid.GetRow(ellipse), x2 = Grid.GetColumn(rectangle), y2 = Grid.GetRow(rectangle);
            bool[] available = { true, true, true, true };

            char hora = GetVectorX(x2,x1), verta = GetVectorY(y2, y1);

            // Дамки
            if (ellipse.Stroke.ToString() == "#FFFF0000")
            {
                // Проверка белых
                if (ellipse.Fill.ToString() == "#FFF5DEB3" && racism == true)
                {
                    // Обычный ход
                    if ((x2 == x1 + 1 && y2 == y1 + 1) || (x2 == x1 - 1 && y2 == y1 - 1) || (x2 == x1 + 1 && y2 == y1 - 1) || (x2 == x1 - 1 && y2 == y1 + 1))
                    {
                        Grid.SetColumn(ellipse, x2); Grid.SetRow(ellipse, y2);

                        // Смена пола
                        if (y2 == 0)
                        {
                            ellipse.Stroke = new SolidColorBrush(Colors.Red);
                            ellipse.StrokeThickness = 8;
                        }

                        continueturn = false;
                    }
                    // Ход атаки
                    else if ((x2 == x1 + 2 && y2 == y1 - 2) || (x2 == x1 - 2 && y2 == y1 - 2) || (x2 == x1 + 2 && y2 == y1 + 2) || (x2 == x1 - 2 && y2 == y1 + 2))
                    {
                        bool trigger = false;
                        //Основная проверка атаки
                        for (int i = 0; i < countb; i++)
                        {
                            Ellipse enemy = GameGrid.Children[i] as Ellipse;

                            int x3 = Grid.GetColumn(enemy), y3 = Grid.GetRow(enemy);

                            if ((x3 == x1 + 1 && y3 == y1 - 1) || (x3 == x1 - 1 && y3 == y1 - 1) || (x3 == x1 + 1 && y3 == y1 + 1) || (x3 == x1 - 1 && y3 == y1 + 1))
                            {
                                char horb = GetVectorX(x3, x1), vertb = GetVectorY(y3, y1);

                                if (hora == horb && verta == vertb)
                                {
                                    Grid.SetColumn(ellipse, x2); Grid.SetRow(ellipse, y2);
                                    GameGrid.Children.Remove(enemy);
                                    countb--;
                                    continueturn = false;

                                    // Смена пола
                                    if (y2 == 0)
                                    {
                                        ellipse.Stroke = new SolidColorBrush(Colors.Red);
                                        ellipse.StrokeThickness = 8;
                                    }

                                    break;
                                }
                            }
                        }

                        //Прошлое направление
                        if (hora == 'r' && verta == 'u')
                        {
                            available[2] = false;
                        }
                        else if (hora == 'r' && verta == 'd')
                        {
                            available[3] = false;
                        }
                        else if (hora == 'l' && verta == 'u')
                        {
                            available[1] = false;
                        }
                        else if (hora == 'l' && verta == 'd')
                        {
                            available[0] = false;
                        }

                        //Вторичка
                        for (int i = 0; i < 4; i++)
                        {
                            int[] checkx1 = { x2 + 1, x2 + 1, x2 - 1, x2 - 1 }, checky1 = { y2 - 1, y2 + 1, y2 + 1, y2 - 1 };

                            //Направление доступно?
                            if (available[i] != false)
                            {
                                //Проверка ВСЕХ фигур
                                for (int j = 0; j <= countb + countw; j++)
                                {
                                    // За противником никого нет
                                    if (trigger)
                                    {
                                        break;
                                    }
                                    // Истёк срок проверки или направление заблокировано?
                                    if ((j == countb + countw || available[i] == false))
                                    {
                                        available[i] = false; break;
                                    }

                                    Ellipse? target = GameGrid.Children[j] as Ellipse;

                                    int x3 = Grid.GetColumn(target), y3 = Grid.GetRow(target);
                                    //Тут кто-то есть?
                                    if (x3 == checkx1[i] && y3 == checky1[i])
                                    {
                                        int[] checkx2 = { x2 + 2, x2 + 2, x2 - 2, x2 - 2 }, checky2 = { y2 - 2, y2 + 2, y2 + 2, y2 - 2 };

                                        // За пределами поля?
                                        if (checkx2[i] > 7 || checky2[i] > 7 || checkx2[i] < 0 || checky2[i] < 0)
                                        {
                                            available[i] = false; break;
                                        }

                                        //Он враг
                                        if (target.Fill.ToString() == "#FF000000")
                                        {
                                            //Проверка фигур ПРОТИВНИКА
                                            for (int n = 0; n <= countb; n++)
                                            {
                                                // Никого там нет
                                                if (n == countb)
                                                {
                                                    trigger = true;
                                                    break;
                                                }

                                                Ellipse? behindtarget = GameGrid.Children[n] as Ellipse;

                                                int x4 = Grid.GetColumn(behindtarget), y4 = Grid.GetRow(behindtarget);

                                                //Он за целью?
                                                if (x4 == checkx2[i] && y4 == checky2[i])
                                                {
                                                    available[i] = false; break;
                                                }
                                            }
                                        }
                                        //Он свой
                                        else if (target.Fill.ToString() == "#FFF5DEB3")
                                        {
                                            available[i] = false; break;
                                        }
                                    }
                                }
                            }
                        }

                        //Боже царя храни
                        for (int i = 0; i < 4; i++)
                        {
                            if (available[i] == true)
                            {
                                continueturn = true; break;
                            }
                        }
                    }
                    //Завершение
                    if (continueturn == false)
                    {
                        racism = false;
                        TurnColor.Fill = new SolidColorBrush(Colors.Black);
                    }
                }
                // Проверка чёрных
                else if (ellipse.Fill.ToString() == "#FF000000" && racism == false)
                {
                    // Обычный ход
                    if ((x2 == x1 + 1 && y2 == y1 + 1) || (x2 == x1 - 1 && y2 == y1 - 1) || (x2 == x1 + 1 && y2 == y1 - 1) || (x2 == x1 - 1 && y2 == y1 + 1))
                    {
                        Grid.SetColumn(ellipse, x2); Grid.SetRow(ellipse, y2);

                        // Смена пола
                        if (y2 == 7)
                        {
                            ellipse.Stroke = new SolidColorBrush(Colors.Red);
                            ellipse.StrokeThickness = 8;
                        }

                        continueturn = false;
                    }
                    // Ход атаки
                    else if ((x2 == x1 + 2 && y2 == y1 - 2) || (x2 == x1 - 2 && y2 == y1 - 2) || (x2 == x1 + 2 && y2 == y1 + 2) || (x2 == x1 - 2 && y2 == y1 + 2))
                    {
                        bool trigger = false;
                        //Основная проверка атаки
                        for (int i = 0; i < countw; i++)
                        {
                            Ellipse enemy = GameGrid.Children[i + countb] as Ellipse;

                            int x3 = Grid.GetColumn(enemy), y3 = Grid.GetRow(enemy);

                            if ((x3 == x1 + 1 && y3 == y1 - 1) || (x3 == x1 - 1 && y3 == y1 - 1) || (x3 == x1 + 1 && y3 == y1 + 1) || (x3 == x1 - 1 && y3 == y1 + 1))
                            {
                                char horb = GetVectorX(x3, x1), vertb = GetVectorY(y3, y1);

                                if (hora == horb && verta == vertb)
                                {
                                    Grid.SetColumn(ellipse, x2); Grid.SetRow(ellipse, y2);
                                    GameGrid.Children.Remove(enemy);
                                    countw--;
                                    continueturn = false;

                                    // Смена пола
                                    if (y2 == 7)
                                    {
                                        ellipse.Stroke = new SolidColorBrush(Colors.Red);
                                        ellipse.StrokeThickness = 8;
                                    }

                                    break;
                                }
                            }
                        }

                        //Прошлое направление
                        if (hora == 'r' && verta == 'u')
                        {
                            available[2] = false;
                        }
                        else if (hora == 'r' && verta == 'd')
                        {
                            available[3] = false;
                        }
                        else if (hora == 'l' && verta == 'u')
                        {
                            available[1] = false;
                        }
                        else if (hora == 'l' && verta == 'd')
                        {
                            available[0] = false;
                        }

                        //Вторичка
                        for (int i = 0; i < 4; i++)
                        {
                            int[] checkx1 = { x2 + 1, x2 + 1, x2 - 1, x2 - 1 }, checky1 = { y2 - 1, y2 + 1, y2 + 1, y2 - 1 };

                            //Направление доступно?
                            if (available[i] != false)
                            {
                                //Проверка ВСЕХ фигур
                                for (int j = 0; j <= countb + countw; j++)
                                {
                                    // За противником никого нет
                                    if (trigger)
                                    {
                                        break;
                                    }
                                    // Истёк срок проверки или направление заблокировано?
                                    if ((j == countb + countw || available[i] == false))
                                    {
                                        available[i] = false; break;
                                    }

                                    Ellipse? target = GameGrid.Children[j] as Ellipse;

                                    int x3 = Grid.GetColumn(target), y3 = Grid.GetRow(target);
                                    //Тут кто-то есть?
                                    if (x3 == checkx1[i] && y3 == checky1[i])
                                    {
                                        int[] checkx2 = { x2 + 2, x2 + 2, x2 - 2, x2 - 2 }, checky2 = { y2 - 2, y2 + 2, y2 + 2, y2 - 2 };

                                        // За пределами поля?
                                        if (checkx2[i] > 7 || checky2[i] > 7 || checkx2[i] < 0 || checky2[i] < 0)
                                        {
                                            available[i] = false; break;
                                        }

                                        //Он враг
                                        if (target.Fill.ToString() == "#FFF5DEB3")
                                        {
                                            //Проверка фигур ПРОТИВНИКА
                                            for (int n = 0; n <= countw; n++)
                                            {
                                                // Никого там нет
                                                if (n == countw)
                                                {
                                                    trigger = true;
                                                    break;
                                                }

                                                Ellipse? behindtarget = GameGrid.Children[n + countb] as Ellipse;

                                                int x4 = Grid.GetColumn(behindtarget), y4 = Grid.GetRow(behindtarget);

                                                //Он за целью?
                                                if (x4 == checkx2[i] && y4 == checky2[i])
                                                {
                                                    available[i] = false; break;
                                                }
                                            }
                                        }
                                        //Он свой
                                        else if (target.Fill.ToString() == "#FF000000")
                                        {
                                            available[i] = false; break;
                                        }
                                    }
                                }
                            }
                        }

                        //Боже царя храни
                        for (int i = 0; i < 4; i++)
                        {
                            if (available[i] == true)
                            {
                                continueturn = true; break;
                            }
                        }
                    }

                    //Завершение атаки
                    if (continueturn == false)
                    {
                        racism = true;
                        TurnColor.Fill = new SolidColorBrush(Colors.Wheat);
                    }
                }
            }
            // Обычные пчелики
            else if (ellipse.Stroke.ToString() == "#FFF5DEB3" || ellipse.Stroke.ToString() == "#FF000000")
            {
                // Проверка белых
                if (ellipse.Fill.ToString() == "#FFF5DEB3" && racism == true)
                {
                    // Обычный ход
                    if ((x2 == x1 + 1 && y2 == y1 - 1) || (x2 == x1 - 1 && y2 == y1 - 1))
                    {
                        Grid.SetColumn(ellipse, x2); Grid.SetRow(ellipse, y2);

                        // Смена пола
                        if (y2 == 0)
                        {
                            ellipse.Stroke = new SolidColorBrush(Colors.Red);
                            ellipse.StrokeThickness = 8;
                        }

                        continueturn = false;
                    }
                    // Ход атаки
                    else if ((x2 == x1 + 2 && y2 == y1 - 2) || (x2 == x1 - 2 && y2 == y1 - 2) || (x2 == x1 + 2 && y2 == y1 + 2) || (x2 == x1 - 2 && y2 == y1 + 2))
                    {
                        bool trigger = false;
                        //Основная проверка атаки
                        for (int i = 0; i < countb; i++)
                        {
                            Ellipse enemy = GameGrid.Children[i] as Ellipse;

                            int x3 = Grid.GetColumn(enemy), y3 = Grid.GetRow(enemy);

                            if ((x3 == x1 + 1 && y3 == y1 - 1) || (x3 == x1 - 1 && y3 == y1 - 1) || (x3 == x1 + 1 && y3 == y1 + 1) || (x3 == x1 - 1 && y3 == y1 + 1))
                            {
                                char horb = GetVectorX(x3, x1), vertb = GetVectorY(y3, y1);

                                if (hora == horb && verta == vertb)
                                {
                                    Grid.SetColumn(ellipse, x2); Grid.SetRow(ellipse, y2);
                                    GameGrid.Children.Remove(enemy);
                                    countb--;
                                    continueturn = false;

                                    // Смена пола
                                    if (y2 == 0)
                                    {
                                        ellipse.Stroke = new SolidColorBrush(Colors.Red);
                                        ellipse.StrokeThickness = 8;
                                    }

                                    break;
                                }
                            }
                        }

                        //Прошлое направление
                        if (hora == 'r' && verta == 'u')
                        {
                            available[2] = false;
                        }
                        else if (hora == 'r' && verta == 'd')
                        {
                            available[3] = false;
                        }
                        else if (hora == 'l' && verta == 'u')
                        {
                            available[1] = false;
                        }
                        else if (hora == 'l' && verta == 'd')
                        {
                            available[0] = false;
                        }

                        //Вторичка
                        for (int i = 0; i < 4; i++)
                        {
                            int[] checkx1 = { x2 + 1, x2 + 1, x2 - 1, x2 - 1 }, checky1 = { y2 - 1, y2 + 1, y2 + 1, y2 - 1 };

                            //Направление доступно?
                            if (available[i] != false)
                            {
                                //Проверка ВСЕХ фигур
                                for (int j = 0; j <= countb + countw; j++)
                                {
                                    // За противником никого нет
                                    if (trigger)
                                    {
                                        break;
                                    }
                                    // Истёк срок проверки или направление заблокировано?
                                    if ((j == countb + countw || available[i] == false))
                                    {
                                        available[i] = false; break;
                                    }

                                    Ellipse? target = GameGrid.Children[j] as Ellipse;

                                    int x3 = Grid.GetColumn(target), y3 = Grid.GetRow(target);
                                    //Тут кто-то есть?
                                    if (x3 == checkx1[i] && y3 == checky1[i])
                                    {
                                        int[] checkx2 = { x2 + 2, x2 + 2, x2 - 2, x2 - 2 }, checky2 = { y2 - 2, y2 + 2, y2 + 2, y2 - 2 };

                                        // За пределами поля?
                                        if (checkx2[i] > 7 || checky2[i] > 7 || checkx2[i] < 0 || checky2[i] < 0)
                                        {
                                            available[i] = false; break;
                                        }

                                        //Он враг
                                        if (target.Fill.ToString() == "#FF000000")
                                        {
                                            //Проверка фигур ПРОТИВНИКА
                                            for (int n = 0; n <= countb; n++)
                                            {
                                                // Никого там нет
                                                if (n == countb)
                                                {
                                                    trigger = true;
                                                    break;
                                                }

                                                Ellipse? behindtarget = GameGrid.Children[n] as Ellipse;

                                                int x4 = Grid.GetColumn(behindtarget), y4 = Grid.GetRow(behindtarget);

                                                //Он за целью?
                                                if (x4 == checkx2[i] && y4 == checky2[i])
                                                {
                                                    available[i] = false; break;
                                                }
                                            }
                                        }
                                        //Он свой
                                        else if (target.Fill.ToString() == "#FFF5DEB3")
                                        {
                                            available[i] = false; break;
                                        }
                                    }
                                }
                            }
                        }

                        //Боже царя храни
                        for (int i = 0; i < 4; i++)
                        {
                            if (available[i] == true)
                            {
                                continueturn = true; break;
                            }
                        }
                    }
                    //Завершение
                    if (continueturn == false)
                    {
                        racism = false;
                        TurnColor.Fill = new SolidColorBrush(Colors.Black);
                    }
                }
                // Проверка чёрных
                else if (ellipse.Fill.ToString() == "#FF000000" && racism == false)
                {
                    // Обычный ход
                    if ((x2 == x1 + 1 && y2 == y1 + 1) || (x2 == x1 - 1 && y2 == y1 + 1))
                    {
                        Grid.SetColumn(ellipse, x2); Grid.SetRow(ellipse, y2);

                        // Смена пола
                        if (y2 == 7)
                        {
                            ellipse.Stroke = new SolidColorBrush(Colors.Red);
                            ellipse.StrokeThickness = 8;
                        }

                        continueturn = false;
                    }
                    // Ход атаки
                    else if ((x2 == x1 + 2 && y2 == y1 - 2) || (x2 == x1 - 2 && y2 == y1 - 2) || (x2 == x1 + 2 && y2 == y1 + 2) || (x2 == x1 - 2 && y2 == y1 + 2))
                    {
                        bool trigger = false;
                        //Основная проверка атаки
                        for (int i = 0; i < countw; i++)
                        {
                            Ellipse enemy = GameGrid.Children[i + countb] as Ellipse;

                            int x3 = Grid.GetColumn(enemy), y3 = Grid.GetRow(enemy);

                            if ((x3 == x1 + 1 && y3 == y1 - 1) || (x3 == x1 - 1 && y3 == y1 - 1) || (x3 == x1 + 1 && y3 == y1 + 1) || (x3 == x1 - 1 && y3 == y1 + 1))
                            {
                                char horb = GetVectorX(x3, x1), vertb = GetVectorY(y3, y1);

                                if (hora == horb && verta == vertb)
                                {
                                    Grid.SetColumn(ellipse, x2); Grid.SetRow(ellipse, y2);
                                    GameGrid.Children.Remove(enemy);
                                    countw--;
                                    continueturn = false;

                                    // Смена пола
                                    if (y2 == 7)
                                    {
                                        ellipse.Stroke = new SolidColorBrush(Colors.Red);
                                        ellipse.StrokeThickness = 8;
                                    }

                                    break;
                                }
                            }
                        }

                        //Прошлое направление
                        if (hora == 'r' && verta == 'u')
                        {
                            available[2] = false;
                        }
                        else if (hora == 'r' && verta == 'd')
                        {
                            available[3] = false;
                        }
                        else if (hora == 'l' && verta == 'u')
                        {
                            available[1] = false;
                        }
                        else if (hora == 'l' && verta == 'd')
                        {
                            available[0] = false;
                        }

                        //Вторичка
                        for (int i = 0; i < 4; i++)
                        {
                            int[] checkx1 = { x2 + 1, x2 + 1, x2 - 1, x2 - 1 }, checky1 = { y2 - 1, y2 + 1, y2 + 1, y2 - 1 };

                            //Направление доступно?
                            if (available[i] != false)
                            {
                                //Проверка ВСЕХ фигур
                                for (int j = 0; j <= countb + countw; j++)
                                {
                                    // За противником никого нет
                                    if (trigger)
                                    {
                                        break;
                                    }
                                    // Истёк срок проверки или направление заблокировано?
                                    if ((j == countb + countw || available[i] == false))
                                    {
                                        available[i] = false; break;
                                    }

                                    Ellipse? target = GameGrid.Children[j] as Ellipse;

                                    int x3 = Grid.GetColumn(target), y3 = Grid.GetRow(target);
                                    //Тут кто-то есть?
                                    if (x3 == checkx1[i] && y3 == checky1[i])
                                    {
                                        int[] checkx2 = { x2 + 2, x2 + 2, x2 - 2, x2 - 2 }, checky2 = { y2 - 2, y2 + 2, y2 + 2, y2 - 2 };

                                        // За пределами поля?
                                        if (checkx2[i] > 7 || checky2[i] > 7 || checkx2[i] < 0 || checky2[i] < 0)
                                        {
                                            available[i] = false; break;
                                        }

                                        //Он враг
                                        if (target.Fill.ToString() == "#FFF5DEB3")
                                        {
                                            //Проверка фигур ПРОТИВНИКА
                                            for (int n = 0; n <= countw; n++)
                                            {
                                                // Никого там нет
                                                if (n == countw)
                                                {
                                                    trigger = true;
                                                    break;
                                                }

                                                Ellipse? behindtarget = GameGrid.Children[n + countb] as Ellipse;

                                                int x4 = Grid.GetColumn(behindtarget), y4 = Grid.GetRow(behindtarget);

                                                //Он за целью?
                                                if (x4 == checkx2[i] && y4 == checky2[i])
                                                {
                                                    available[i] = false; break;
                                                }
                                            }
                                        }
                                        //Он свой
                                        else if (target.Fill.ToString() == "#FF000000")
                                        {
                                            available[i] = false; break;
                                        }
                                    }
                                }
                            }
                        }

                        //Боже царя храни
                        for (int i = 0; i < 4; i++)
                        {
                            if (available[i] == true)
                            {
                                continueturn = true; break;
                            }
                        }
                    }

                    //Завершение атаки
                    if (continueturn == false)
                    {
                        racism = true;
                        TurnColor.Fill = new SolidColorBrush(Colors.Wheat);
                    }
                }
            }

            // Проверка победы
            if (countb == 0)
            {
                BackGrid.Children.Clear();
                GameGrid.Children.Clear();
                WinnerColor.Visibility = Visibility.Visible;
                WinnerColor.Fill = new SolidColorBrush(Colors.Wheat);
                Announcer.Visibility = Visibility.Visible;
                Announcer.Content = "Белые победили";
            }
            else if (countw == 0)
            {
                BackGrid.Children.Clear();
                GameGrid.Children.Clear();
                WinnerColor.Visibility = Visibility.Visible;
                WinnerColor.Fill = new SolidColorBrush(Colors.Black);
                Announcer.Visibility = Visibility.Visible;
                Announcer.Content = "Чёрные победили";
            }
        }

        /// <summary>
        /// Мы не говорим об этом
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key.ToString())
            {
                case "S":
                    BackGrid.Children.Clear();
                    GameGrid.Children.Clear();
                    SkillIssue.Visibility = Visibility.Visible;
                    break;
                case "W":
                    BackGrid.Children.Clear();
                    GameGrid.Children.Clear();
                    WinnerColor.Visibility = Visibility.Visible;
                    WinnerColor.Fill = new SolidColorBrush(Colors.Wheat);
                    Announcer.Visibility = Visibility.Visible;
                    Announcer.Content = "Белые победили";
                    break;
                case "B":
                    BackGrid.Children.Clear();
                    GameGrid.Children.Clear();
                    WinnerColor.Visibility = Visibility.Visible;
                    WinnerColor.Fill = new SolidColorBrush(Colors.Black);
                    Announcer.Visibility = Visibility.Visible;
                    Announcer.Content = "Чёрные победили";
                    break;
                default: 
                    break;
            }
        }
    }
}