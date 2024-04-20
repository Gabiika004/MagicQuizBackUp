using MagicQuizDesktop.ViewModels;
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
using System.Windows.Shapes;

namespace MagicQuizDesktop.View.Windows
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        public GameWindow()
        {
            InitializeComponent();
            this.Activated += GameWindow_Activated;
            this.Deactivated += GameWindow_Deactivated;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void GameWindow_Activated(object sender, EventArgs e)
        {
            // Ellenőrzés, hogy a DataContext egy GameViewModel-e
            if (this.DataContext is GameViewModel viewModel)
            {
                viewModel.ResumeTimer();
            }
        }

        private void GameWindow_Deactivated(object sender, EventArgs e)
        {
            // Ellenőrzés, hogy a DataContext egy GameViewModel-e
            if (this.DataContext is GameViewModel viewModel)
            {
                viewModel.PauseTimer();
            }
        }
    }
}
