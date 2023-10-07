using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Controls.Primitives;

namespace _1_utkast_SKALA_GENERATOR
{
    /// <summary>
    /// Interaction logic for LoadScaleWindow.xaml
    /// </summary>
    public partial class LoadScaleWindow : Window
    {
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        List<Positions> savedPositions;
        public LoadScaleWindow()
        {
            InitializeComponent();

             savedPositions = JsonSerializer.Deserialize<List<Positions>>(File.ReadAllText(mainWindow.savedPositionsFileName));
            if (savedPositions.Count >= 0)
            {
                foreach(var position in savedPositions )
                {
                    string positions = "";
                    for (int i = 0; i < position.NotePositions.Length; i++)//✔ X
                    {
                        positions += position.NotePositions[i] ? "O " : "X ";
                    }
                    var pvm = new PositionsViewModel()
                    {
                        Name = position.Name,
                        Positions = positions
                    };
                    
                    SavedPositionsListBox.Items.Add(pvm);
                }
            }
            // 
        }
        public class PositionsViewModel
        {
            public PositionsViewModel()
            {
            }
            public string Name { get; set; }
            public string Positions { get; set; }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).IsEnabled = true;
        }

        private void LoadPosition_Click(object sender, RoutedEventArgs e)
        {
            var selectedPosition = ((PositionsViewModel)SavedPositionsListBox.SelectedItem);

            var positionsList = JsonSerializer.Deserialize<List<Positions>>(File.ReadAllText(mainWindow.savedPositionsFileName));

            foreach(var position in positionsList)
            {
                if(selectedPosition.Name == position.Name)
                {
                    mainWindow.positions = position.NotePositions;

                    for (int i = 1; i < mainWindow.positions.Length; i++)
                    {
                        ((ToggleButton)mainWindow.toggleButtonStackPanel.Children[i]).IsChecked = mainWindow.positions[i];
                    }

                    mainWindow.generateScalePattern();
                    break;
                }
            }

            Close();
            
        }
    }
}
