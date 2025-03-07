using System;
using System.Collections.Generic;
using System.IO.Ports;
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
using System.Configuration;
using System.Security.Cryptography;
using System.Windows.Shapes;

namespace Poseshchaemost
{
    /// <summary>
    /// Логика взаимодействия для Vkhod.xaml
    /// </summary>
    public partial class Vkhod : Page
    {

        public Vkhod()
        {
            InitializeComponent();
            //чтение портов доступных в системе
            string[] ports = SerialPort.GetPortNames();
            //Очистка содержимого бокса
            PortsBox.Items.Clear();
            //Добавление найденных портов в бокс
            PortsBox.ItemsSource = ports;

        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            string Pass = ConfigurationManager.AppSettings["Password"];
            Pass = SecureConfig.Decrypt(Pass);

            if (Pass == PasswordBox.Password)
            {
                if (PortsBox.SelectedItem != null)
                {
                    Meneger.comPort = PortsBox.SelectedItem.ToString();
                    Meneger.Frame.Navigate(new MainPage());
                }
                else
                {
                    MessageBox.Show("Выберите порт");
                }
            }
            else
            {
                MessageBox.Show("No");
            }
        }
    }
}
