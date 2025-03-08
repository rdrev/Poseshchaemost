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
using System.Security.Policy;
using System.Collections;
using System.IO;
using System.Globalization;

namespace Poseshchaemost
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private SerialPort serialPort;
        private String cart_ = "";
        private bool naprovlenie = false;
        private SotrudnikPage sotrudnikPage = new SotrudnikPage(new Sotrudnik());
        public MainPage()
        {
            InitializeComponent();
            InitializeSerialPort();
            
        }
        public void Update()
        {
            Update1();
            Update2();
        }

        private void Update1()
        {
            var sotrudnikList = DB.GetBD().Sotrudniks.OrderBy(x => x.familiya).ToList();
            var kuratorList = sotrudnikList.Where(x => x.id_sotrudnik == x.kurator).ToList();

            kuratorList.Insert(0, new Sotrudnik {  familiya = "Все" });

            sotrudnikList = sotrudnikList.Where(x => x.familiya.ToLower().Contains(PoiskBT.Text.ToLower()) ||
                                                     x.imya.ToLower().Contains(PoiskBT.Text.ToLower()) ||
                                                     x.otchestvo.ToLower().Contains(PoiskBT.Text.ToLower())).ToList();

            if (kuratorBox.SelectedIndex > 0)
            {
                var kurator = (Sotrudnik)kuratorBox.SelectedItem;
                sotrudnikList = sotrudnikList.Where(x => x.kurator == kurator.id_sotrudnik).ToList();
            }

            kuratorBox.ItemsSource = kuratorList;

            dataGrid.ItemsSource = sotrudnikList;
        }

        private void Update2()
        {
            var prokhodList = DB.GetBD().Prokhods.OrderByDescending(x => x.data).ToList();
            prokhodList = prokhodList.Where(x => x.Sotrudnik1.familiya.ToLower().Contains(PoiskBT2.Text.ToLower()) ||
                                                 x.Sotrudnik1.imya.ToLower().Contains(PoiskBT2.Text.ToLower()) ||
                                                 x.Sotrudnik1.otchestvo.ToLower().Contains(PoiskBT2.Text.ToLower())).ToList();
            dataGrid2.ItemsSource = prokhodList;
        }

        private void kuratorBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update1();
        }

        private void InitializeSerialPort()
        {
            serialPort = new SerialPort
            {
                PortName = Meneger.comPort,  // Укажите правильный порт
                BaudRate = 9600,
                Parity = Parity.None,
                DataBits = 8,
                StopBits = StopBits.One,
                Handshake = Handshake.None
            };

            serialPort.DataReceived += SerialPort_DataReceived;

            try
            {
                serialPort.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка открытия порта: {ex.Message}");
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = serialPort.ReadExisting();

            foreach (var item in data)
            {
                switch (item)
                {
                    case '+':
                        naprovlenie = true;
                        cart_ = "";
                        break;
                    case '-':
                        naprovlenie = false;
                        cart_ = "";
                        break;
                    case ';':
                        string cart = cart_;
                        bool napr = naprovlenie;
                        Dispatcher.Invoke(() => card_db(cart, napr));
                        break;
                    default:
                        cart_ += item;
                        break;
                }
            }
            
        }

        private void card_db(string catr, bool napr)
        {
            sotrudnikPage.card_up(catr);
            var pro = DB.GetBD().Sotrudniks.Where(x => x.cart == catr).FirstOrDefault();
            if (pro != null)
            {
                serialPort.Write("@");

                PosItem.DataContext = pro;

                if (napr)
                {
                    TB_naprovlenie.Text = "Пришёл";
                    TB_naprovlenie.Foreground = Brushes.LawnGreen;
                }
                else
                {
                    TB_naprovlenie.Text = "Ушёл";
                    TB_naprovlenie.Foreground = Brushes.Red;
                }

                var prokhod = new Prokhod
                {
                    data = DateTime.Now,
                    naprovelenie = napr,
                    sotrudnik = pro.id_sotrudnik
                };
                DB.GetBD().Prokhods.Add(prokhod);
                DB.GetBD().SaveChanges();

                Update2();
            }
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
            sotrudnikPage = new SotrudnikPage(new Sotrudnik());
            Meneger.Frame.Navigate(sotrudnikPage);
        }

        private void UpBtn_Click(object sender, RoutedEventArgs e)
        {
            sotrudnikPage = new SotrudnikPage((sender as Button).DataContext as Sotrudnik);
            Meneger.Frame.Navigate(sotrudnikPage);
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы дельствительно хотете уволить сотрудника", "Подверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var sotrudnik = (sender as Button).DataContext as Sotrudnik;
                DB.GetBD().Sotrudniks.Remove(sotrudnik);
                DB.GetBD().SaveChanges();
                Update1();
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        private void PoiskBT_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update1();
        }

        private void PoiskBT2_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update2();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Meneger.Frame.Navigate(new PasswordPage());
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Meneger.Frame.GoBack(); 
            
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
            }

        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            ReferenceWindow referenceWindow = new ReferenceWindow();
            referenceWindow.Show();
        }
    }
}
