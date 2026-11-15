using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ManagedShellTest
{
    /// <summary>
    /// Logika interakcji dla klasy NotifBalloon.xaml
    /// </summary>
    public partial class NotifBalloon : Window
    {
        private static readonly Duration FadeDuration = TimeSpan.FromMilliseconds(250);
        private readonly DispatcherTimer _closeTimer = new DispatcherTimer();
        private int _displayVersion;
        private Action? _clickAction;

        public NotifBalloon()
        {
            InitializeComponent();
            _closeTimer.Tick += CloseTimer_Tick;
            this.MouseDown += (s, e) =>
            {
                _clickAction?.Invoke();
                _clickAction = null;
                _closeTimer.Stop();
                Hide();
            };
            Opacity = 0;
        }

        private void notifWnd_Loaded(object sender, RoutedEventArgs e)
        {
            // Make this appear in the top left corner of the screen
            this.Left = 3;
            this.Top = 30;
        }

        public void ShowBalloon(string title, string info, ImageSource image, int timeout, Action? clickAction)
        {
            this.title.Text = title;
            this.info.Text = info;
            this.pic.Source = image;

            _clickAction = clickAction;

            this.title.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
            this.info.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));

            const double contentWidth = 82;
            double availableWidth = Math.Max(MinWidth, SystemParameters.WorkArea.Width - 16);
            double messageWidth = Math.Min(this.info.DesiredSize.Width + contentWidth, availableWidth);
            this.Width = Math.Max(MinWidth, Math.Max(this.title.DesiredSize.Width + contentWidth, messageWidth));

            _displayVersion++;
            _closeTimer.Stop();
            BeginAnimation(OpacityProperty, null);
            Opacity = 0;

            if (!IsVisible)
            {
                Show();
            }

            BeginAnimation(OpacityProperty, new DoubleAnimation(0, 1, FadeDuration)
            {
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            });

            _closeTimer.Interval = TimeSpan.FromMilliseconds(Math.Max(0, timeout));
            _closeTimer.Start();
        }

        private void CloseTimer_Tick(object sender, EventArgs e)
        {
            _closeTimer.Stop();
            int displayVersion = _displayVersion;

            DoubleAnimation fadeOut = new DoubleAnimation(0, FadeDuration)
            {
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
            };
            fadeOut.Completed += (s, args) =>
            {
                if (displayVersion == _displayVersion)
                {
                    Hide();
                    BeginAnimation(OpacityProperty, null);
                    Opacity = 0;
                }
            };

            BeginAnimation(OpacityProperty, fadeOut);
        }
    }
}
