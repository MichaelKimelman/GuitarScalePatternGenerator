using System;
using System.Collections.Generic;
using System.IO;
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

namespace _1_utkast_SKALA_GENERATOR
{
    /// <summary>
    /// Interaction logic for AddScaleWindow.xaml
    /// </summary>
    public partial class AddScaleWindow : Window
    {
        bool[] currentPositions;
        public AddScaleWindow()
        {
            InitializeComponent();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //var owner = (MainWindow)((Window)sender).Owner;
            //var mainWindow = ((MainWindow)Application.Current.MainWindow);
            //var currentPosition 
            
            
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            currentPositions = mainWindow.positions;
            List<Positions> deserializeList = JsonSerializer.Deserialize<List<Positions>>(File.ReadAllText(mainWindow.savedPositionsFileName));

            bool positionCondition = !deserializeList.Exists(x => x.NotePositions.SequenceEqual(currentPositions));
            bool nameCondition = !deserializeList.Exists(x => x.Name == scaleNameTextBox.Text);

            if (scaleNameTextBox.Text.Trim().Length > 0 && positionCondition && nameCondition/*&&*/ /* &&  INSERT CHECK FOR CURRENTPOSITION IN DESERIALIZELIST*/)
            {
                ////serializeList.AddRange(mainWindow.savedPositions);
                ////var list = mainWindow.savedPositions;

                var newPosition = new Positions()
                {
                    Name = scaleNameTextBox.Text,
                    NotePositions = currentPositions,
                };

                deserializeList.Add(newPosition);

                ////using FileStream createStream = File.Create(mainWindow.savedPositionsFileName);
                var json = JsonSerializer.Serialize( deserializeList);
                File.WriteAllText(mainWindow.savedPositionsFileName, json );
                ////createStream.Dispose();

                ////mainWindow.savedPositions = JsonSerializer.Deserialize<List<Positions>>(File.ReadAllText(mainWindow.savedPositionsFileName));

                //////savedPositions = JsonSerializer.Deserialize<List<Positions>>(File.ReadAllText(savedPositionsFileName));
                ////mainWindow.SavedPositionsComboBox.Items.Clear();

                ////foreach (var position in mainWindow.savedPositions)
                ////{
                ////    mainWindow.SavedPositionsComboBox.Items.Add(position);
                ////}

                ////mainWindow.SavedPositionsComboBox.SelectedIndex = mainWindow.SavedPositionsComboBox.Items.Count -1;

                Close();
            }
            else if(!positionCondition)
            {
                MessageBox.Show("Position Already Exists, Enter New One", "Error", MessageBoxButton.OK);
                Close();
            }
            else if(!nameCondition)
            {
                MessageBox.Show("Name already Taken, Enter New Name", "Error", MessageBoxButton.OK);

            }
            //else if()



            

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).IsEnabled = true;
        }
    }
}
