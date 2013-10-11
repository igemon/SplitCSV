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

namespace FileCut
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        FileStr f; 
        private void bt_browse_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "*.csv"; // Default file name
            dlg.DefaultExt = ".text"; // Default file extension
            dlg.Filter = "CSV файлы(*.csv)|*.csv|TXT файлы(*.txt)|*.txt|Все файлы (*.*)|*.*"; // Filter files by extension
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                tb_file.Text = filename;
                bt_open_Click(this, null);
            }
        }

        private void tb_file_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (this.tb_file.Text != string.Empty)
                {
                    bt_open_Click(this, null);

                }
            }
        }

        private void bt_open_Click(object sender, RoutedEventArgs e)
        {
            if (this.tb_file.Text != string.Empty)
            { 
            f = new FileStr();
            
            bt_convert.IsEnabled = true;
         
            

        }
        }
        private void cb_mode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tb_strok != null && tb_chast != null)
            {
                if (cb_mode.SelectedIndex == 0)
                {
                    tb_strok.IsEnabled = false;
                    tb_chast.IsEnabled = true;
                }
                else
                {
                    tb_strok.IsEnabled = true;
                    tb_chast.IsEnabled = false;
                }
            }
        }
        private void bt_convert_Click(object sender, RoutedEventArgs e)
        {
            f.OpenFile(tb_file.Text);
            if (cb_mode.SelectedIndex == 0)
            {
                f.Cut_part(tb_chast.Text);
            }
            else
            {
                f.Cut_count_line(tb_strok.Text);
            }
            f.WriteFiles(tb_name_out.Text);
            
        }
        private class FileStr
        {
            //prop
            private string FileName { get; set; }
            private string MyProperty { get; set; }
            private  StreamReader sr  { get; set; }
            private int CountFile { get; set; }
            private List<string> listOfPart { get; set; }
            //method
            public FileStr()
            {
                listOfPart = new List<string>();
            }
            public void OpenFile(string fpath)
            {
                this.FileName = fpath;

                sr = new StreamReader(FileName); 
             
            }
            public void Cut_part(string k)
            {
                int num_part = int.Parse(k);
                int size = System.IO.File.ReadLines(FileName).Count();
                int offset=(size%num_part==0?0:1);
                int kol = size / num_part + offset;
                Cut_count_line(kol.ToString());
                
            }
            public void Cut_count_line(string c)
            {
                int num=int.Parse(c);
                 while (!sr.EndOfStream) 
                {
                    string str = "";
                    for (int i=0;i<num;i++)
                    {
                        if (sr.EndOfStream != true)
                        {
                            str += sr.ReadLine();
                            str += "\n";
                        }
                        else continue;
                    }

                    listOfPart.Add(str); 
                }
                 sr.Close();
                 

            }
            public void WriteFiles(string fname)
            {
                string[] fullpath=FileName.Split('.');
                string path = fullpath[0];
                Directory.CreateDirectory(path);
                for (int i = 0; i < listOfPart.Count; i++)
                {
                    using (StreamWriter sw = new StreamWriter(path+@"\"+fname+"_part"+i+".csv"))
                    {

                        sw.WriteLine(listOfPart[i]);
                        
                    }
                }
                listOfPart.Clear();
            }
        }

     

      
      
    }
}
