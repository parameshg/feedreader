using EnsureThat;
using FeedReader.Persistence;
using FeedReader.Redux;
using Redux;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace FeedReader
{
    public partial class Home : Window
    {
        private IStore<State> Store { get; }

        private DispatcherTimer Timer { get; } = new DispatcherTimer();

        public Home(IStore<State> store)
        {
            InitializeComponent();

            Store = EnsureArg.IsNotNull(store);

            Timer.Interval = new TimeSpan(1, 0, 0);

            Timer.Tick += OnTimer;
        }

        private void OnTimer(object? sender, EventArgs e)
        {
            Store.Dispatch(new RefreshAction());
        }

        private void OnLoad(object sender, RoutedEventArgs args)
        {
            Top = SystemParameters.WorkArea.Bottom - Height;

            Left = SystemParameters.WorkArea.Right - Width;

            Store.Subscribe(state => OnMessage(state));

            Timer.Start();

            Store.Dispatch(new LoadAction());
        }

        private void OnTray(object sender, MouseButtonEventArgs args)
        {
            Show();

            Activate();
        }

        private void OnMessage(State state)
        {
            News.ItemsSource = state.Stories;
        }

        private void OnHide(object sender, RoutedEventArgs e)
        {
            Store.Dispatch(new HideAction { Id = ((Button)sender).Tag.ToString() });
        }

        private void OnLike(object sender, RoutedEventArgs e)
        {
        }

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            var control = sender as TextBlock;

            if (control != null)
            {
                control.Background = Brushes.WhiteSmoke;
            }
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            var control = sender as TextBlock;

            if (control != null)
            {
                control.Background = Brushes.White;
            }
        }

        private void OnMouseClick(object sender, MouseButtonEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = ((TextBlock)sender).Tag.ToString(),
                UseShellExecute = true
            });
        }

        private void OnMinimize(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}