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

namespace Poseshchaemost
{
    /// <summary>
    /// Логика взаимодействия для ReferenceWindow.xaml
    /// </summary>
    public partial class ReferenceWindow : Window
    {
        public ReferenceWindow()
        {
            InitializeComponent();
            textBox.Text = "\t\t\tPoseshchaemost\nЭто проект для сдачи курсовой работы. Он считывает данные \nEM карт из монитора порта сравнивает данные из базы \nи пропускает пользователя фиксируя проход в базе.\nТакже есть механизмы редактирование списков сотрудников.";
        }
    }
}
