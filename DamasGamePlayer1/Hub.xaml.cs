
using DamasGame.Util;
using DamasGamePlayer1.Host;
using log4net;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace DamasGamePlayer1
{
    /// <summary>
    /// Interaction logic for Hub.xaml
    /// </summary>
    public partial class Hub : IHubWindow
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DamasGamePlayer1Service));

        private Window _waitWindow;
        public Hub()
        {
            InitializeComponent();
            Log.Info(String.Format(CultureInfo.CurrentCulture, Messages.MSG_AGUARDANDO_JOGADOR, Constants.JOGADOR2));
            _waitWindow = new Window { Height = 100, Width = 400, WindowStartupLocation = WindowStartupLocation.CenterScreen, WindowStyle = WindowStyle.None };
            _waitWindow.Content = new TextBlock { Text = String.Format(CultureInfo.CurrentCulture, Messages.MSG_AGUARDANDO_JOGADOR, Constants.JOGADOR2), FontSize = 30, FontWeight = FontWeights.Bold, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
            _waitWindow.Show();
        }

        public void End()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                _waitWindow.Close();
                this.Close();
            })); 
        }

        private void CenterWindowOnScreen()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }

        public void CloseHubAndStartGame()
        {
    
                IGameWindow window = InjectionContainer.Container.Resolve<IGameWindow>();
                End();
                window.Show();
        
        }
    }
}
