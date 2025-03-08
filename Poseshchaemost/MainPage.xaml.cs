using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.IO.Ports;
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
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private SerialPort serialPort;
        private String data1 = "";

        public MainPage()
        {
            InitializeComponent();
            InitializeSerialPort();
            
        }
        public void Update()
        {
            Update1();
        }

        private void Update1()
        {
            var sotrudnikList = BD.GetBD().Sotrudniks.OrderBy(x => x.familiya).ToList();
            var kuratorList = sotrudnikList.Where(x => x.id_sotrudnik == x.kurator).ToList();
            kuratorList.Insert(0, new Sotrudnik {  familiya = "Все" });

            if (kuratorBox.SelectedIndex > 0)
            {
                var kurator = (Sotrudnik)kuratorBox.SelectedItem;
                sotrudnikList = sotrudnikList.Where(x => x.kurator == kurator.id_sotrudnik).ToList();
            }

            kuratorBox.ItemsSource = kuratorList;

            dataGrid.ItemsSource = sotrudnikList;
        }

        private void kuratorBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update1();
        }

        private void InitializeSerialPort()
        {
            //serialPort = new SerialPort
            //{
            //    PortName = Meneger.comPort,  // Укажите правильный порт
            //    BaudRate = 9600,
            //    Parity = Parity.None,
            //    DataBits = 8,
            //    StopBits = StopBits.One,
            //    Handshake = Handshake.None
            //};

            //serialPort.DataReceived += SerialPort_DataReceived;

            //try
            //{
            //    serialPort.Open();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Ошибка открытия порта: {ex.Message}");
            //}
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = serialPort.ReadExisting();

            //Dispatcher.Invoke(() => ReceivedDataTextBox.Text = data);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            Meneger.Frame.Navigate(new SotrudnikPage(new Sotrudnik()));
        }

        private void UpBtn_Click(object sender, RoutedEventArgs e)
        {
            Meneger.Frame.Navigate(new SotrudnikPage((sender as Button).DataContext as Sotrudnik));
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы дельствительно хотете уволить сотрудника", "Подверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var sotrudnik = (sender as Button).DataContext as Sotrudnik;
                BD.GetBD().Sotrudniks.Remove(sotrudnik);
                BD.GetBD().SaveChanges();
                Update1();
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Update();
        }
    }
}
