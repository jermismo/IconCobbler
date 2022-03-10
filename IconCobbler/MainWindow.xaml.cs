using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace IconCobbler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<string> Files { get; } = new ObservableCollection<string>();        

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainList_PreviewDrop(object sender, DragEventArgs e)
        {
            if (e.Data is DataObject data)
            {
                e.Effects = data.ContainsFileDropList() ? DragDropEffects.Copy : DragDropEffects.None;
            }
            else e.Effects = DragDropEffects.None;
        }

        private void MainList_Drop(object sender, DragEventArgs e)
        {
            if (e.Data is DataObject data)
            {
                bool hadBadFile = false;
                var files = data.GetFileDropList();
                foreach (var file in files)
                {
                    if (file is not null)
                    {
                        var ext = System.IO.Path.GetExtension(file);
                        if (!string.Equals(ext, ".png", StringComparison.OrdinalIgnoreCase))
                        {
                            hadBadFile = true;
                        }
                        else
                        {
                            Files.Add(file);
                        }
                    }
                }
                if (hadBadFile)
                {
                    MessageBox.Show("Hey, only PNG files please!");
                }
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                if (btn.DataContext is not null)
                {
                    _ = Files.Remove(btn.DataContext.ToString());
                }          
            }
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            if (Files.Count == 0)
            {
                MessageBox.Show("Please add some image files.");
                return;
            }
            // select a save file location
            SaveFileDialog dlg = new();
            dlg.Filter = "Icon File (*.ico)|*.ico";
            dlg.Title = "Save Icon File";
            dlg.InitialDirectory = System.IO.Path.GetDirectoryName(Files[0]);
            dlg.OverwritePrompt = true;
            dlg.DefaultExt = ".ico";
            dlg.FileName = System.IO.Path.GetFileNameWithoutExtension(Files[0]) + ".ico";
            if (dlg.ShowDialog() != true)
            {
                return;
            }

            string saveFile = dlg.FileName;

            IconMaker.MakeIconFile(saveFile, Files);
        }
    }
}
