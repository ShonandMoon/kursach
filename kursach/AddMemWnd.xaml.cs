using Microsoft.Win32;
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

namespace kursach
{
    /// <summary>
    /// Логика взаимодействия для AddMemWnd.xaml
    /// </summary>
    public partial class AddMemWnd : Window
    {
        public MainWindow MainWindow;

        public AddMemWnd()
        {
            InitializeComponent();
        }

        private void OpenFD_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.ShowDialog();

            
            string filePath = openFileDialog.FileName;
            MemPathTXT.Text = filePath;
        }

        private void AddNewMem_Click(object sender, RoutedEventArgs e)
        {
            if (MemPathTXT.Text == "" || CtgrNameTXT.Text == "" || MemNameTXT.Text == "")
            {
                return;
            }

            memClass meme = new memClass(MemPathTXT.Text, CtgrNameTXT.Text, MemNameTXT.Text);
            MainWindow.AddMeme(meme);

            Close();
        }
    }
}
