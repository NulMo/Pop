using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pop
{
    public partial class PopUp : Window
    {
        private List<Window> activeNotifications = new List<Window>();

        public Task ShowNotificationAsync(string title, string message)
        {
            return Dispatcher.UIThread.InvokeAsync(async () =>
            {
                var notification = new Window
                {
                    WindowStartupLocation = WindowStartupLocation.Manual,
                    SystemDecorations = SystemDecorations.None,
                    Background = new SolidColorBrush(Colors.SlateGray),
                    Opacity = 0,
                    Width = 300,
                    Height = 100,
                    Topmost = true,
                    ShowInTaskbar = false,
                    ShowActivated = false,  // Prevent the window from being activated
                    CanResize = false,
                };
                var content = new StackPanel
                {
                    Margin = new Thickness(10),
                };

                content.Children.Add(new TextBlock { Text = title, FontWeight = FontWeight.Light });
                content.Children.Add(new TextBlock { Text = message, TextWrapping = TextWrapping.Wrap, FontWeight = FontWeight.Bold });

                notification.Content = content;

                var workingArea = Screens.Primary.WorkingArea;
                notification.Position = new PixelPoint(
                    workingArea.Right - (int)notification.Width - 20,
                    workingArea.Bottom - (int)notification.Height - 20 - (activeNotifications.Count * 110)
                );

                notification.Show();
                activeNotifications.Add(notification);

                for (double i = 0; i <= 1; i += 0.1)
                {
                    notification.Opacity = i;
                    await Task.Delay(20);
                }

                await Task.Delay(5000);

                for (double i = 1; i >= 0; i -= 0.1)
                {
                    notification.Opacity = i;
                    await Task.Delay(20);
                }

                notification.Close();
                activeNotifications.Remove(notification);
            });
        }
    }
}