using System;
using System.Drawing;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Color = Windows.UI.Color;

// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x412에 나와 있습니다.

namespace PSTimeUWP
{
    public sealed partial class MainPage : Page
    {
        private ApplicationViewTitleBar titleBar;
        private bool isLastKnownFullScreen = ApplicationView.GetForCurrentView().IsFullScreenMode;

        DispatcherTimer timer = new DispatcherTimer();
        DispatcherTimer analogtimer = new DispatcherTimer();

        public MainPage()
        {
            this.InitializeComponent();

            initTitle();

            aClock_Setting();
            initTimer();
        }

        private void initTitle()
        {
            titleBar = ApplicationView.GetForCurrentView().TitleBar;

            titleBar.BackgroundColor = Colors.Black;
            titleBar.ButtonBackgroundColor = Colors.Black;
            titleBar.ForegroundColor = Colors.White;
            titleBar.ButtonForegroundColor = Colors.White;
        }

        #region
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Window.Current.SizeChanged += OnWindowResize;
            UpdateContent();
        }

        void OnWindowResize(object sender, WindowSizeChangedEventArgs e)
        {
            UpdateContent();
        }

        void UpdateContent()
        {
            var view = ApplicationView.GetForCurrentView();
            var isFullScreenMode = view.IsFullScreenMode;
            ToggleFullScreenModeSymbol.Symbol = isFullScreenMode ? Symbol.BackToWindow : Symbol.FullScreen;

            // Did the system force a change in full screen mode?
            if (isLastKnownFullScreen != isFullScreenMode)
            {
                isLastKnownFullScreen = isFullScreenMode;
            }
        }

        #endregion



        void initTimer()
        {
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;
            timer.Start();

            analogtimer.Interval = new TimeSpan(0, 0, 1);
            analogtimer.Tick += Analogtimer_Tick;
            analogtimer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            DateTime now = DateTime.Now;
            tbTime.Text = now.ToString("hh:mm");
        }


        #region 아날로그시계
        //https://m.post.naver.com/viewer/postView.nhn?volumeNo=7414345&memberNo=11439725&vType=VERTICAL
        private bool aClock_Flag = false;
        private System.Drawing.Point Center;
        private double radius;
        private int hourHand;
        private int minHand;
        private int secHand;
        // 아날로그 시계에서 사용하는 변수를 설정
        // Center는 중심점
        // hourHand, minHand, secHand는 각각 시침, 분침, 초침의 길이
        private void aClock_Setting()
        {
            Center = new System.Drawing.Point((int)Canvas1.Width / 2, (int)Canvas1.Height / 2);
            radius = Canvas1.Width / 2;
            hourHand = (int)(radius * 0.45);
            minHand = (int)(radius * 0.55);
            secHand = (int)(radius * 0.65);
        }

        private void Analogtimer_Tick(object sender, object e)
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
                0, 0, new SolidColorBrush(Colors.RoyalBlue), 8, new Thickness(Center.X, Center.Y, 0, 0));
            //분침
            DrawLine(minHand * Math.Sin(radMin), -minHand * Math.Cos(radMin),
                0, 0, new SolidColorBrush(Colors.SkyBlue), 6, new Thickness(Center.X, Center.Y, 0, 0));
            // 초침
            DrawLine(secHand * Math.Sin(radSec), -secHand * Math.Cos(radSec),
                0, 0, new SolidColorBrush(Colors.OrangeRed), 3, new Thickness(Center.X, Center.Y, 0, 0));

            Ellipse core = new Ellipse();
            core.Margin = new Thickness(Canvas1.Width / 2 - 5, Canvas1.Height / 2 - 5, 0, 0);
            //core.Stroke = Brushes.SteelBlue;
            //core.Fill = Brush.LightSteelBlue;
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


        private void closeImage_KeyDown(object sender, KeyRoutedEventArgs e)
        {

        }

        private void ToggleFullScreenModeButton_Click(object sender, RoutedEventArgs e)
        {
            var view = ApplicationView.GetForCurrentView();
            if (view.IsFullScreenMode)
            {
                view.ExitFullScreenMode();
            }
            else
            {
                view.TryEnterFullScreenMode();
            }
        }
    }
}
