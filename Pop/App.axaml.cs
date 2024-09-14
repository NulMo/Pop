using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia.ViewModels.Commands;
using System;

namespace Pop
{
    public partial class App : Application
    {
        private TrayIcon _trayIcon;
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
                _trayIcon = new TrayIcon()
                {
                    Icon = new WindowIcon("../../../Assets/tray-icon.ico"),
                    ToolTipText = "Pop!",
                    IsVisible = true,
                };
                var trayMenu = new NativeMenu();

                var exitItem = new NativeMenuItem("Exit");
                trayMenu.Items.Add(exitItem);

                exitItem.Click += (s, e) =>
                {
                    Environment.Exit(0);
                };

                _trayIcon.Menu = trayMenu;
                _trayIcon.Clicked += (s, e) =>
                {
                    var window = desktop.MainWindow;
                    if (window.IsVisible)
                        window.Hide();
                    else
                        window.Show();
                };

                desktop.Exit += (s, e) => _trayIcon.Dispose();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}