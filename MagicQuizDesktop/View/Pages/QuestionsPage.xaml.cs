using MagicQuizDesktop.Models;
using MagicQuizDesktop.ViewModels;
using Microsoft.Extensions.DependencyInjection;
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

namespace MagicQuizDesktop.View.Pages
{
    /// <summary>
    /// Interaction logic for QuestionsPage.xaml
    /// </summary>
    public partial class QuestionsPage : Page
    {
        public QuestionsPage()
        {
            InitializeComponent();
            DataContext = App.ServiceProvider.GetService<QuestionViewModel>();
        }
    }
}
