using ManagedShell.WindowsTray;
using System.Windows.Media.Imaging;

namespace ManagedShellTest
{
    public partial class Form1 : Form
    {
        NotifBalloon notif = new NotifBalloon();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ManagedShell.ShellManager shellManager = new ManagedShell.ShellManager();
            shellManager.NotificationArea.NotificationBalloonShown += NotificationArea_NotificationBalloonShown;
            this.Hide();
        }

        private void NotificationArea_NotificationBalloonShown(object? sender, NotificationBalloonEventArgs e)
        {
            notif.ShowBalloon(e.Balloon.Title, e.Balloon.Info, e.Balloon.Icon, e.Balloon.Timeout, e.Balloon.Click);
        }
    }
}
