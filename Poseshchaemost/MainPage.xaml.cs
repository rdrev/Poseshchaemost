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
            dataGrid.ItemsSource = PoseshchaemostEntities1.GetPoseshchaemostEntities1es().Sotrudnik.ToList();
            Update();
        }
        public void Update()
        {
            Update1();
        }

        private void Update1()
        {
            var sotrudnikList = PoseshchaemostEntities1.GetPoseshchaemostEntities1es().Sotrudnik.ToList();
            var kuratorList = sotrudnikList.Where(x => x.id_sotrudnik == x.kurator).ToList();

            kuratorBox.ItemsSource = kuratorList;

            dataGrid.ItemsSource = sotrudnikList;
        }

        private void kuratorBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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
    }
}
