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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PSTime
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        DispatcherTimer analogtimer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();

            aClock_Setting();
            initTimer();
        }

        void initTimer()
        {
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;
            timer.Start();

            analogtimer.Interval = new TimeSpan(0, 0, 1);
            analogtimer.Tick += Analogtimer_Tick;
            analogtimer.Start();
        }



        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            tbTime.Text = now.ToString("hh:mm");


#if false
            double secondRadian = 2 * Math.PI / 60 * now.Second;
            this.secondLine.X2 = Math.Sin(secondRadian) * 10;
            this.secondLine.Y2 = Math.Cos(secondRadian) * 10;

            double minuteRadian = 2 * Math.PI / 60 * now.Minute;
            this.minuteLine.X2 = Math.Sin(minuteRadian) * 10;
            this.minuteLine.Y2 = Math.Cos(minuteRadian) * 10;

            double hourRadian = 2 * Math.PI / 720 * ((now.Hour % 12) * 60 + now.Minute);
            this.hourLine.X2 = Math.Sin(hourRadian) * 7;
            this.hourLine.Y2 = Math.Cos(hourRadian) * 7;
#endif
        }

        #region 아날로그시계
        //https://m.post.naver.com/viewer/postView.nhn?volumeNo=7414345&memberNo=11439725&vType=VERTICAL
        private bool aClock_Flag = false;
        private Point Center;
        private double radius;
        private int hourHand;
        private int minHand;
        private int secHand;
        // 아날로그 시계에서 사용하는 변수를 설정
        // Center는 중심점
        // hourHand, minHand, secHand는 각각 시침, 분침, 초침의 길이
        private void aClock_Setting()
        {
            Center = new Point(Canvas1.Width / 2, Canvas1.Height / 2);
            radius = Canvas1.Width / 2;
            hourHand = (int)(radius * 0.45);
            minHand = (int)(radius * 0.55);
            secHand = (int)(radius * 0.65);
        }

        private void Analogtimer_Tick(object sender, EventArgs e)
        {
            DateTime c = DateTime.Now;

            Canvas1.Children.Clear();  // 현재 화면 지우기
            //DrawClockFace(); //시계판 그리기
            double radHr = (c.Hour % 12 + c.Minute / 60.0) * 30 * Math.PI / 180;
            double radMin = (c.Minute + c.Second / 60.0) * 6 * Math.PI / 180;
            double radSec = (c.Second + c.Millisecond / 1000.0) * 6 * Math.PI / 180;
            DrawHands(radHr, radMin, radSec); // 바늘 그리기
        }

        //private void DrawClockFace()
        //{
        //    aClock.Stroke = Brushes.LightSteelBlue;
        //    aClock.StrokeThickness = 30;
        //    Canvas1.Children.Add(aClock);
        //}

        // 시계 바늘 그리기
        private void DrawHands(double radHr, double radMin, double radSec)
        {
            // 시침
            DrawLine(hourHand * Math.Sin(radHr), -hourHand * Math.Cos(radHr),
                0, 0, Brushes.RoyalBlue, 8, new Thickness(Center.X, Center.Y, 0, 0));
            //분침
            DrawLine(minHand * Math.Sin(radMin), -minHand * Math.Cos(radMin),
                0, 0, Brushes.SkyBlue, 6, new Thickness(Center.X, Center.Y, 0, 0));
            // 초침
            DrawLine(secHand * Math.Sin(radSec), -secHand * Math.Cos(radSec),
                0, 0, Brushes.OrangeRed, 3, new Thickness(Center.X, Center.Y, 0, 0));

            Ellipse core = new Ellipse();
            core.Margin = new Thickness(Canvas1.Width / 2 - 5, Canvas1.Height / 2 - 5, 0, 0);
            //core.Stroke = Brushes.SteelBlue;
            core.Fill = Brushes.LightSteelBlue;
            core.Width = 10;
            core.Height = 10;
            Canvas1.Children.Add(core);
        }

        private void DrawLine(double x1, double y1, int x2, int y2,
            SolidColorBrush color, int thick, Thickness margin)
        {
            Line line = new Line();
            line.X1 = x1; line.Y1 = y1; line.X2 = x2; line.Y2 = y2;
            line.Stroke = color;
            line.StrokeThickness = thick;
            line.Margin = margin;
            line.StrokeStartLineCap = PenLineCap.Round;
            Canvas1.Children.Add(line);
        }
        #endregion

        private void closeImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
