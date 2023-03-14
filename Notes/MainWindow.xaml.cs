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
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System.Windows.Media;
using System.Reflection;

namespace Notes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        bool titleChanged= false;
        string oldTitle = "";

        private void Window_Size_Changed(object sender, SizeChangedEventArgs e)
        {
            // Get the new window size.
            double width = e.NewSize.Width;
            double height = e.NewSize.Height;
            
            size_TextBlock.Text = width.ToString() + " x " + height.ToString();
            
        }

        public MainWindow()
        {
            InitializeComponent();


            // combobox set a list of colors
            List<string> colorList = new List<string>();
            var color = typeof(System.Drawing.KnownColor);
            var colors = Enum.GetValues(color);
            var from = 27;
            var to = colors.Length - 7;
            

            for (int i = from; i < to; i++)
            {
                colorList.Add(colors.GetValue(i).ToString());
            }
            Color_ComboBox.ItemsSource = colorList.ToArray();

            // combobox set a list of fonts
            foreach (FontFamily fontFamily in Fonts.SystemFontFamilies)
            {
                FontsComboBox.Items.Add(fontFamily);
            }

            // combobox set a list of font sizes
            for(int i = 10; i < 100;)
            {
                FontSize_ComboBox.Items.Add(i);
                i += 2;
            }
        }


        private void addNote(object sender, RoutedEventArgs e)
        {
            string title = "SET A TITLE";
            int addition = 0;
            string path = Directory.GetCurrentDirectory() + "/Notes/" + title + addition.ToString() + ".txt";
            

            while (true)
            {
                if (File.Exists(path))
                {
                    addition++;
                    path = Directory.GetCurrentDirectory() + "/Notes/" + title + addition.ToString() + ".txt";
                }
                else
                {
                    File.WriteAllText(path, "Input your text here...");
                    UpdateList(sender, e);
                    File_ListBox.SelectedItem = File_ListBox.Items[File_ListBox.Items.IndexOf(title + addition.ToString())];
                    OpenNote(sender, e);
                    break;
                }
                
            }
        }

        private void deleteNote(object sender, RoutedEventArgs e)
        {
            string selectedName = File_ListBox.SelectedItem.ToString();
            string path = Directory.GetCurrentDirectory() + "/Notes/" + selectedName + ".txt";
            if (File.Exists(path))
            {
                File.Delete(path);
                
            }
            UpdateList(sender, e);
            File_ListBox.SelectedItem = File_ListBox.Items[0];
            OpenNote(sender, e);
        }

        private void UpdateList(object sender, RoutedEventArgs e)
        {
            string path = Directory.GetCurrentDirectory() + "/Notes/";
            string[] files = Directory.GetFiles(path);
            string name;
            File_ListBox.Items.Clear();
            foreach (string file in files)
            {
                name = System.IO.Path.GetFileName(file);
                File_ListBox.Items.Add(name.Substring(0, name.Length - 4));
            }

            if(File_ListBox.Items.Count < 1)
            {
                addNote(sender, e);

            }

        }


        private void GetNoteContent(string path, string title)
        {
            title_TextBox.Text = title;
            title_TextBox.Opacity = 1.0;
            text_TextBox.Text = File.ReadAllText(path);
            text_TextBox.Opacity = 1.0;
        }

        private void OpenNote(object sender, RoutedEventArgs e)
        {
            var selectedItem = File_ListBox.SelectedItem;
            string path = Directory.GetCurrentDirectory() + "/Notes/" + selectedItem + ".txt";

            // Проверить, что элемент был выбран
            if (selectedItem != null)
            {
                if (File.Exists(path))
                {
                    GetNoteContent(path, selectedItem.ToString());
                    SaveNote_Button.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show($"ЭЛЕМЕНТА: {selectedItem} нет");
                    File_ListBox.Items.Remove(selectedItem);
                    UpdateList(sender, e);
                }
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string path = Directory.GetCurrentDirectory() + "/Notes";
            SaveNote_Button.IsEnabled = false;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            UpdateList(sender, e);
            
            if (File_ListBox.Items.Count> 0)
            {
                File_ListBox.SelectedItem = File_ListBox.Items[0];
                OpenNote(sender, e);
            }
            

            
        }



        private void Color_Click(object sender, RoutedEventArgs e)
        {
            // Change the background color of a panel (assuming it has the name "panel1")
            // panel1.Background = Brushes.Red;
        }

        private void Font_Click(object sender, RoutedEventArgs e)
        {
            // Change the font family and size for the entire project
            FontFamily newFont = new FontFamily("Arial");
            double newSize = 14.0;
            // Apply the new font and size to the entire project (assuming the name of the top-level grid is "grid1")
            // grid1.FontFamily = newFont;
            // grid1.FontSize = newSize;
        }

        private void Title_GotFocus(object sender, RoutedEventArgs e)
        {
            // Clear the title when the user clicks in it
            if (title_TextBox.Text == "" || title_TextBox.Text == "SET A TITLE" || title_TextBox.Opacity == 0.2)
            {
                title_TextBox.Text = "";
            }
            title_TextBox.Opacity = 1.0;
        }
        private void Title_LostFocus(object sender, RoutedEventArgs e)
        {
            // Clear the title when the user clicks in it
            if (title_TextBox.Text.Length > 15)
            {
                title_TextBox.Text = "MAX 15 SYMB";
                title_TextBox.Opacity = 0.2;
            }
            else
            {
                if (title_TextBox.Text == "")
                {
                    title_TextBox.Text = "SET A TITLE";
                    title_TextBox.Opacity = 0.2;
                }
                else
                {
                    title_TextBox.Text = title_TextBox.Text.ToUpper();
                    SaveNote_Button.IsEnabled = true;
                }
            }
            
        }

        private void Text_GotFocus(object sender, RoutedEventArgs e)
        {
            // Clear the text when the user clicks in it
            if (text_TextBox.Text == "" || text_TextBox.Text == "Input your text here...")
            {
                text_TextBox.Text = "";
            }
            text_TextBox.Opacity = 1.0;
        }

        private void Text_LostFocus(object sender, RoutedEventArgs e)
        {
            if (text_TextBox.Text.Replace(" ", "") == "") {
                text_TextBox.Text = "Input your text here...";
                text_TextBox.Opacity = 0.2;
            }
        }

        public bool IsValidFileName(string fileName)
        {
            if (fileName.Length == 0) { return false; }
            string invalidChars = new string(System.IO.Path.GetInvalidFileNameChars());
            string regexSearch = new string(System.IO.Path.GetInvalidPathChars()) + new string(System.IO.Path.GetInvalidFileNameChars());
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(string.Format("[{0}]", System.Text.RegularExpressions.Regex.Escape(regexSearch)));

            if (string.IsNullOrEmpty(fileName))
            {
                return false;
            }

            if (fileName.IndexOfAny(invalidChars.ToCharArray()) >= 0)
            {
                return false;
            }

            if (regex.IsMatch(fileName))
            {
                return false;
            }

            return true;
        }


        public void save_close(object senderr, System.ComponentModel.CancelEventArgs ee)
        {
            string title = title_TextBox.Text;
            string text = text_TextBox.Text;
            if (IsValidFileName(title))
            {
                object sender = new Button();
                RoutedEventArgs e = new RoutedEventArgs();
                string path = Directory.GetCurrentDirectory() + "/Notes/" + title + ".txt";
                if (!File.Exists(path))
                {
                    if (File.Exists(Directory.GetCurrentDirectory() + "/Notes/" + File_ListBox.SelectedItem.ToString() + ".txt"))
                        System.IO.File.Move(Directory.GetCurrentDirectory() + "/Notes/" + File_ListBox.SelectedItem.ToString() + ".txt", path);
                    File.WriteAllText(path, text);
                }
                else
                {
                    File.WriteAllText(path, text);
                }
                
            }
            else
            {
                MessageBox.Show("Invalid file name!");
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Save the text to a file
            string title = title_TextBox.Text;
            string text = text_TextBox.Text;

            if (IsValidFileName(title))
            {
                string path = Directory.GetCurrentDirectory() + "/Notes/" + title + ".txt";
                if (!File.Exists(path))
                {
                    if(File.Exists(Directory.GetCurrentDirectory() + "/Notes/" + File_ListBox.SelectedItem.ToString() + ".txt"))
                        System.IO.File.Move(Directory.GetCurrentDirectory() + "/Notes/" + File_ListBox.SelectedItem.ToString() + ".txt", path);
                    File.WriteAllText(path, text);
                    UpdateList(sender, e);
                    int index = File_ListBox.Items.IndexOf(title);
                    File_ListBox.SelectedItem = File_ListBox.Items[index];
                    OpenNote(sender, e);

                }
                else
                {
                    File.WriteAllText(path, text);
                }
            }
            else
            {
                MessageBox.Show("Invalid file name!");
            }
        }

        private void FontsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FontFamily selectedFont = (FontFamily)FontsComboBox.SelectedItem;
            if (selectedFont != null)
            {
                text_TextBox.FontFamily = selectedFont;
                title_TextBox.FontFamily= selectedFont;
            }
        }


        private void FontsComboBoxSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int selectedSize = (int)FontSize_ComboBox.SelectedItem;
                if (selectedSize > 0)
                {
                    text_TextBox.FontSize = selectedSize;
                }
            }
            catch (Exception ex)
            {
                //Handle the exception here
            }
        }


        private bool IsNumeric(string text)
        {
            foreach (char c in text)
            {
                if (!Char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        private void FontOwn_TextChanged(object sender, EventArgs e)
        {
            if (FontSizeOwn_TextBox.Text != "" &&  IsNumeric(FontSizeOwn_TextBox.Text))
            {
                int size = int.Parse(FontSizeOwn_TextBox.Text);
                if (size > 0 && size < 110)
                    text_TextBox.FontSize = size;
                else
                    FontSizeOwn_TextBox.Text = "1";

            }
            else
            {
                FontSizeOwn_TextBox.Text = "1";
            }
        }
        


        private void ColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Color_ComboBox.SelectedItem != null)
            {
                string colorName = Color_ComboBox.SelectedItem.ToString();
                System.Drawing.Color selectedColor = System.Drawing.Color.FromName(colorName);
                SolidColorBrush brush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(selectedColor.A, selectedColor.R, selectedColor.G, selectedColor.B));
                Background = brush;
                Menu_1.Background = brush;

                SolidColorBrush brush2 = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#272537"));

                // Set the window background color to the SolidColorBrush
                this.Background = brush2;
            }
        }

        private void Wtf(object sender, RoutedEventArgs e)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            MessageBox.Show(currentDirectory);
        }

    }
}
