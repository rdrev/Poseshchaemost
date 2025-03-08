using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для SotrudnikPage.xaml
    /// </summary>
    public partial class SotrudnikPage : Page
    {
        private Sotrudnik sotrudnik = new Sotrudnik();

        public SotrudnikPage(Sotrudnik sotrudnik)
        {
            InitializeComponent();
            this.sotrudnik = sotrudnik;

            DataContext = this.sotrudnik;
            var kuratorList = DB.GetBD().Sotrudniks.Where(x => x.id_sotrudnik == x.kurator).ToList();
            CBB.ItemsSource = kuratorList;

            if (sotrudnik.id_sotrudnik == 0)
            {
                CBB.SelectedIndex = 0;
            }
            else
            {
                CBB.SelectedItem = kuratorList.Where(x => x.id_sotrudnik == sotrudnik.kurator).ToList()[0];
            }
        }

        private void ObzBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";

            Nullable<bool> result = openFileDialog.ShowDialog();


            if (result == true)
            {
                string filename = openFileDialog.FileName;
                var img = File.ReadAllBytes(filename);
                var path = System.IO.Path.Combine(Environment.CurrentDirectory, "Bilder", filename);
                var uri = new Uri(path);
                var bitmap = new BitmapImage(uri);

                sotrudnik.foto = img;

                ipmegePoto.Source = bitmap;
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sotrudnik.id_sotrudnik == 0)
                {
                    sotrudnik.kurator = ((Sotrudnik)CBB.SelectedItem).id_sotrudnik;

                    DB.GetBD().Sotrudniks.Add(sotrudnik);
                    DB.GetBD().SaveChanges();
                }
                else
                {
                    sotrudnik.kurator = ((Sotrudnik)CBB.SelectedItem).id_sotrudnik;

                    var task = DB.GetBD().Sotrudniks.Find(sotrudnik.id_sotrudnik);
                    task = sotrudnik;

                    DB.GetBD().SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Meneger.Frame.GoBack();
        }

        private void CleBtn_Click(object sender, RoutedEventArgs e)
        {
            Meneger.Frame.GoBack();
        }
    }
}
