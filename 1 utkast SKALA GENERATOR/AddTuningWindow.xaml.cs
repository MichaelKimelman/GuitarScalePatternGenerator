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

namespace _1_utkast_SKALA_GENERATOR
{
    /// <summary>
    /// Interaction logic for AddTuningWindow.xaml
    /// </summary>
    public partial class AddTuningWindow : Window
    {
        MainWindow mainwindow = (MainWindow)Application.Current.MainWindow;
        public AddTuningWindow()
        {
            InitializeComponent();

            foreach (var child in NoteSelectStackpanel.Children)
            {
                foreach (var note in Enum.GetValues(typeof(MusicNotes)))
                {
                    ((ComboBox)child).Items.Add(note);
                }
                ((ComboBox)child).SelectedIndex = 1;
            }
        }

        private void AddTuning_Click(object sender, RoutedEventArgs e)
        {
            Tuning newtuning = new Tuning()
            {
                Name = TuningNameTextbox.Text,
                Notes = new MusicNotes[]
                    {
                        (MusicNotes)((ComboBox)sixthString).SelectedItem,
                        (MusicNotes)((ComboBox)fifthString).SelectedItem,
                        (MusicNotes)((ComboBox)fourthString).SelectedItem,
                        (MusicNotes)((ComboBox)thirdString).SelectedItem,
                        (MusicNotes)((ComboBox)secondString).SelectedItem,
                        (MusicNotes)((ComboBox)firstString).SelectedItem,
                    }
            };
            var list = JsonSerializer.Deserialize<List<Tuning>>(File.ReadAllText(mainwindow.savedTuningsFileName));

            bool nameCondition = !list.Exists(x => x.Name == TuningNameTextbox.Text);
            bool notesCondition = !list.Exists(x => x.Notes.SequenceEqual(newtuning.Notes));

            if (TuningNameTextbox.Text.Length > 0 && nameCondition && notesCondition)
            {
                list.Add(newtuning);


                var json = JsonSerializer.Serialize(list);
                File.WriteAllText(mainwindow.savedTuningsFileName, json);

                var newTuningList = JsonSerializer.Deserialize<List<Tuning>>(File.ReadAllText(mainwindow.savedTuningsFileName));
                mainwindow.tuningsComboBox.Items.Clear();
                foreach(var item in newTuningList)
                {
                    mainwindow.tuningsComboBox.Items.Add(item);
                }
                mainwindow.tuningsComboBox.SelectedIndex = mainwindow.tuningsComboBox.Items.Count - 1;
                mainwindow.generateScalePattern();

                Close();
            }
            else if(!notesCondition)
            {
                MessageBox.Show("Tuning already exists, enter new tuning please.", "Error", MessageBoxButton.OK);
            }
            else if (!nameCondition)
            {
                MessageBox.Show("Name already Taken, Enter New Name", "Error", MessageBoxButton.OK);
            }            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).IsEnabled = true;
        }
    }
}
