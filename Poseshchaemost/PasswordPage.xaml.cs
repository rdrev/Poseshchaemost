using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace Poseshchaemost
{
    /// <summary>
    /// Логика взаимодействия для PasswordPage.xaml
    /// </summary>
    public partial class PasswordPage : Page
    {
        public PasswordPage()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Meneger.Frame.GoBack();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordBox1.Password == PasswordBox2.Password)
            {
                ConfigurationManager.AppSettings["Password"] = PasswordBox1.Password;
                Meneger.Frame.GoBack();
            }
            else 
            {
                MessageBox.Show("Ошибка");
            }
        }
    }
}
