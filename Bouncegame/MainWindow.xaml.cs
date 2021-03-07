using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Bouncegame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Model model = new Model();
        DispatcherTimer timer_ballMove = new DispatcherTimer();
        DispatcherTimer timer_ballAdjust = new DispatcherTimer();



        public MainWindow()
        {
            InitializeComponent();

            switchToDevMode(false);
            slid_speedControl.Value = 1;

            timer_ballMove.Tick += moveBall;
            timer_ballMove.Interval = TimeSpan.FromMilliseconds(5);

            timer_ballAdjust.Tick += adjustBall;
            timer_ballAdjust.Interval = TimeSpan.FromMilliseconds(100);
            timer_ballAdjust.Start();

            grid_window.Background = Brushes.Black;
            Console.WriteLine("test");
        }

        //
        //TimerTick Methdos
        //
        void moveBall(object sender, EventArgs e)
        {
            if ((bool)cb_hasGravity.IsChecked)
            {
                applyGravity();
                //updateBallPosition();
                fillDevInfo();
                showDevInfo();
            }
            else
            {
                if ((bool)cb_isDevMode.IsChecked)
                {
                    if ((bool)cb_y.IsChecked)
                    {
                        if (model.jumpDown)
                        {
                            Canvas.SetTop(ellip_ball, model.ballTop + model.ballJumpDist);
                            if (model.ballTop >= model.windowHeigth - (ellip_ball.Height * 1.5))
                            {
                                model.jumpDown = false;
                            }
                        }
                        else
                        {
                            Canvas.SetTop(ellip_ball, model.ballTop - model.ballJumpDist);
                            if (model.ballTop <= 0)
                            {
                                model.jumpDown = true;
                            }
                        }
                    }

                    if ((bool)cb_x.IsChecked)
                    {
                        if (model.jumpRight)
                        {
                            Canvas.SetLeft(ellip_ball, model.ballLeft + model.ballJumpDist);
                            if (model.ballLeft >= model.windowWidth - (1.2 * ellip_ball.Width))
                            {
                                model.jumpRight = false;
                            }
                        }
                        else
                        {
                            Canvas.SetLeft(ellip_ball, model.ballLeft - model.ballJumpDist);
                            if (model.ballLeft <= 0)
                            {
                                model.jumpRight = true;
                            }
                        }
                    }
                    updateBallPosition();
                    fillDevInfo();
                    showDevInfo();
                }
                else
                {
                    if (model.jumpDown)
                    {
                        Canvas.SetTop(ellip_ball, model.ballTop + model.ballJumpDist);
                        if (model.ballTop >= model.windowHeigth - (ellip_ball.Height * 1.5))
                        {
                            model.jumpDown = false;
                        }
                    }
                    else
                    {
                        Canvas.SetTop(ellip_ball, model.ballTop - model.ballJumpDist);
                        if (model.ballTop <= 0)
                        {
                            model.jumpDown = true;
                        }
                    }

                    if (model.jumpRight)
                    {
                        Canvas.SetLeft(ellip_ball, model.ballLeft + model.ballJumpDist);
                        if (model.ballLeft >= model.windowWidth - (1.2 * ellip_ball.Width))
                        {
                            model.jumpRight = false;
                        }
                    }
                    else
                    {
                        Canvas.SetLeft(ellip_ball, model.ballLeft - model.ballJumpDist);
                        if (model.ballLeft <= 0)
                        {
                            model.jumpRight = true;
                        }
                    }
                    updateBallPosition();
                    fillDevInfo();
                    showDevInfo();
                }
            }
        }
        void applyGravity()
        {
            if (model.ballTop >= model.windowHeigth - (ellip_ball.Height * 1.5))
            {
                model.velocity = (-model.velocity * model.friction);
            }
            else
            {
                model.velocity += model.gravity;
            }
            Canvas.SetTop(ellip_ball, model.ballTop + model.velocity);

            if (model.jumpRight)
            {
                Canvas.SetLeft(ellip_ball, model.ballLeft + model.ballJumpDist);
                if (model.ballLeft >= model.windowWidth - (1.2 * ellip_ball.Width))
                {
                    model.jumpRight = false;
                }
            }
            else
            {
                Canvas.SetLeft(ellip_ball, model.ballLeft - model.ballJumpDist);
                if (model.ballLeft <= 0)
                {
                    model.jumpRight = true;
                }
            }
        }
        void adjustBall(Object sender, EventArgs e)
        {
            checkModeHardcore();
        }

        //
        //adjustBallTimer Methods
        //
        void checkModeHardcore()
        {
            if ((bool)cb_isModeHardcore.IsChecked)
            {
                ellip_ball.Fill = getRandomColor();
                ellip_ball.Width = getRandomWidth();
                ellip_ball.Height = ellip_ball.Width;
                grid_window.Background = Brushes.Black;
                changeTextColor(Brushes.White);
                timer_ballAdjust.IsEnabled = true;
            }
            else
            {
                var converter = new BrushConverter();
                var brush = (Brush)converter.ConvertFromString("#FFFBE6B8");


                changeTextColor(Brushes.Black);
                grid_window.Background = brush;
            }
        }
        private void cb_isModeHardcore_Unchecked(object sender, RoutedEventArgs e)
        {
            getCheckedFill();
        }

        SolidColorBrush getRandomColor()
        {
            SolidColorBrush color;
            Random random = new Random();

            byte r;
            byte g;
            byte b;

            r = (byte)random.Next(0, byte.MaxValue);
            g = (byte)random.Next(0, byte.MaxValue);
            b = (byte)random.Next(0, byte.MaxValue);

            return color = new SolidColorBrush(Color.FromRgb(r, g, b));
        }

        int getRandomWidth()
        {
            int width;
            Random random = new Random();

            return width = random.Next(2, 100);
        }

        //
        //Game Methods
        //
        private void btn_go_Click(object sender, RoutedEventArgs e)
        {
            if (timer_ballMove.IsEnabled)
            {
                timer_ballMove.IsEnabled = false;
                btn_go.Content = "Go";
            }
            else if (!timer_ballMove.IsEnabled)
            {
                timer_ballMove.IsEnabled = true;
                btn_go.Content = "Pause";
            }
        }
        private void slid_speedControl_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lbl_difficulty.Content = ($"Difficulty(0-10): {Convert.ToInt32(slid_speedControl.Value)}");
            model.ballJumpDist = slid_speedControl.Value * 2;
        }
        private void slid_speedControl_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                slid_speedControl.Value += 1;
            }
            if (e.Delta < 0)
            {
                slid_speedControl.Value -= 1;
            }
        }
        private void cb_isDevMode_Click(object sender, RoutedEventArgs e)
        {
            checkForDevMode();
        }
        void checkForDevMode()
        {
            if ((bool)cb_isDevMode.IsChecked)
            {
                switchToDevMode(true);
            }
            else
            {
                switchToDevMode(false);
            }
        }
        void switchToDevMode(bool isDevMode)
        {
            if (!isDevMode)
            {
                lbl_jumpDown.Visibility = Visibility.Hidden;
                lbl_jumpRight.Visibility = Visibility.Hidden;
                lbl_windowHeigthAndBallTop.Visibility = Visibility.Hidden;
                lbl_windowWidthAndBallLeft.Visibility = Visibility.Hidden;
                lbl_status.Visibility = Visibility.Hidden;

                cb_x.Visibility = Visibility.Hidden;
                cb_y.Visibility = Visibility.Hidden;
            }
            else
            {
                lbl_jumpDown.Visibility = Visibility.Visible;
                lbl_jumpRight.Visibility = Visibility.Visible;
                lbl_windowHeigthAndBallTop.Visibility = Visibility.Visible;
                lbl_windowWidthAndBallLeft.Visibility = Visibility.Visible;
                lbl_status.Visibility = Visibility.Visible;

                cb_x.Visibility = Visibility.Visible;
                cb_y.Visibility = Visibility.Visible;
            }
        }

        void fillDevInfo()
        {
            model.windowWidth = window_window.Width;
            model.windowHeigth = window_window.Height;
            model.ballLeft = Canvas.GetLeft(ellip_ball);
            model.ballTop = Canvas.GetTop(ellip_ball);

            model.windowWidth = window_window.Width;
            model.windowHeigth = window_window.Height;
        }

        void showDevInfo()
        {
            lbl_jumpRight.Content = ($"Jump Right: {model.jumpRight}");
            lbl_jumpDown.Content = ($"Jump Down: {model.jumpDown}");
            lbl_windowWidthAndBallLeft.Content = ($"Window Width: {model.windowWidth} - Ball Left: {model.ballLeft}");
            lbl_windowHeigthAndBallTop.Content = ($"Window Heigth: {model.windowHeigth} - Ball Top: {model.ballTop}");
            lbl_velocity.Content = ($"Velocity: {model.velocity}");
            Console.WriteLine(model.velocity);
            lbl_friction.Content = ($"Friction: {model.friction}");
        }

        //
        //Ball Methods
        //
        private void ellip_ball_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            model.score++;
            lbl_score.Content = "Score: " + model.score.ToString();
            //ellip_ball.Fill = Brushes.Green;
        }

        private void ellip_ball_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //ellip_ball.Fill = Brushes.White;
        }

        private void ellip_ball_MouseLeave(object sender, MouseEventArgs e)
        {
            //ellip_ball.Fill = Brushes.White;
        }
        void updateBallPosition()
        {
            model.ballLeft = Canvas.GetLeft(ellip_ball);
            model.ballTop = Canvas.GetTop(ellip_ball);
        }

        private void rb_white_Checked(object sender, RoutedEventArgs e)
        {
            ellip_ball.Fill = Brushes.White;
        }

        private void rb_black_Checked(object sender, RoutedEventArgs e)
        {
            ellip_ball.Fill = Brushes.Black;
        }

        private void rb_green_Checked(object sender, RoutedEventArgs e)
        {
            ellip_ball.Fill = Brushes.Green;
        }

        private void rb_img_Checked(object sender, RoutedEventArgs e)
        {
            ImageBrush ib = new ImageBrush();

            ib.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/Download.png"));

            ellip_ball.Fill = ib;
        }

        //
        //Window Methdos
        //
        private void window_window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            fillDevInfo();
            showDevInfo();
        }

        //
        //HelperMethods
        //
        void changeTextColor(Brush brush)
        {
            lbl_difficulty.Foreground = brush;
            cb_isDevMode.Foreground = brush;
            cb_isModeHardcore.Foreground = brush;
            lbl_score.Foreground = brush;


            lbl_jumpDown.Foreground = brush;
            lbl_jumpRight.Foreground = brush;
            lbl_windowHeigthAndBallTop.Foreground = brush;
            lbl_windowWidthAndBallLeft.Foreground = brush;
            lbl_status.Foreground = brush;

            cb_x.Foreground = brush;
            cb_y.Foreground = brush;

            rb_white.Foreground = brush;
            rb_black.Foreground = brush;
            rb_green.Foreground = brush;
            rb_img.Foreground = brush;

        }
        void getCheckedFill()
        {
            if ((bool)rb_white.IsChecked)
            {
                ellip_ball.Fill = Brushes.White;
            }
            if ((bool)rb_black.IsChecked)
            {
                ellip_ball.Fill = Brushes.Black;
            }
            if ((bool)rb_green.IsChecked)
            {
                ellip_ball.Fill = Brushes.Green;
            }
            if ((bool)rb_img.IsChecked)
            {
                ellip_ball.Fill = Brushes.White;
            }
        }



    }
}
