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
using System.Text.Json;

namespace kursach
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<string, List<memClass>> memesSorted = new Dictionary<string, List<memClass>>();
        public MainWindow()
        {
            InitializeComponent();
        }

        public void AddMeme(memClass mem)
        {
            if (memesSorted.Keys.Contains(mem.Category))
            {
                memesSorted[mem.Category].Add(mem);
            }
            else
            {
                CtgrList.Items.Add(mem.Category);
                memesSorted.Add(mem.Category, new List<memClass>() { mem });
            }

            UpdateMemList();
        }

        private void UpdateMemList()
        {
            List<memClass> memesSortedByCategory = new List<memClass>();
            List<memClass> memesSortedByName = new List<memClass>();

            
            if (CtgrList.SelectedItem == null)
            {
                foreach (List<memClass> memes in memesSorted.Values)
                {
                    memesSortedByCategory.AddRange(memes);
                }
            }
            else
            {
                memesSortedByCategory.AddRange(memesSorted[(string)CtgrList.SelectedItem]);
            }

            
            if (Poisk.Text != "")
            {
                foreach (memClass meme in memesSortedByCategory)
                {
                    if (meme.Name.Contains(Poisk.Text))
                    {
                        memesSortedByName.Add(meme);
                    }
                }
            }
            else
            {
                memesSortedByName.AddRange(memesSortedByCategory);
            }

            MemList.Items.Clear();

            foreach (memClass meme in memesSortedByName)
            {
                MemList.Items.Add(meme);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddMemWnd addWnd = new AddMemWnd();
            addWnd.Show();
            addWnd.MainWindow = this;
        }

        private void CtgrList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateMemList();
        }

        private void Poisk_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateMemList();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MemList.SelectedItem != null)
            {
                foreach (List<memClass> memes in memesSorted.Values)
                {
                    if (memes.Contains(MemList.SelectedItem))
                    {
                        memes.Remove(MemList.SelectedItem as memClass);
                    }
                }
                UpdateMemList();
            }

        }

        private void MemList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MemList.SelectedItem != null)
            {
                BitmapImage memeImage = new BitmapImage(); 
                memeImage.BeginInit();
                memeImage.UriSource = new Uri((MemList.SelectedItem as memClass).ImagePath);
                memeImage.EndInit();
                MemeIMG.Source = memeImage;
            }
        }

        private void SaveFileBttn_Click(object sender, RoutedEventArgs e)
        {
            string save = "";

            foreach (List<memClass> memes in memesSorted.Values)
            {
                foreach (memClass meme in memes)
                {
                    save += JsonSerializer.Serialize<memClass>(meme) + "|";
                }
            }


            StreamWriter fileStream = new StreamWriter(Environment.CurrentDirectory + "/save.txt");
            fileStream.Write(save);
            fileStream.Close();
        }

        private void DownldFile_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Environment.CurrentDirectory + "/save.txt")) //Чек, существует ли файлик сохранения
            {
                //Сброс всех мемов
                memesSorted.Clear();
                CtgrList.Items.Clear();
                MemList.Items.Clear();
                MemeIMG.Source = null;

                StreamReader streamReader = new StreamReader(Environment.CurrentDirectory + "/save.txt");
                string saveFile = streamReader.ReadToEnd(); //Читаем
                streamReader.Close();

                string[] saves;
                saves = saveFile.Split('|'); //Делим строчку на разные мемасы

                foreach (string save in saves)
                {
                    if (save == "") //Прервать цикл, если дошли до пустого сохранения
                    {
                        break;
                    }

                    memClass meme = JsonSerializer.Deserialize<memClass>(save);
                    AddMeme(meme);
                }
            }
        }
    }
}
