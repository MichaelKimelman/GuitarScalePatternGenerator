using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.IO;
using System.Text.Json;

namespace _1_utkast_SKALA_GENERATOR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool[] positions = new bool[12] //POSITIONER FÖR SKALA
        {
            true, false, false, 
            false, false, false, 
            false, false, false, 
            false, false, false,
        };

        public readonly string savedPositionsFileName = "savedPositions.json";

        public List<Positions> savedPositions = new List<Positions>(); //SPARADE POSITIONER

        public readonly string savedTuningsFileName = "savedTunings.json";

        bool hasLoaded = false; //FIXAR ATT APPEN INTE KRASHAR FÖR ATT STÄMNINGDROPDOWN INTE KAN HITTA KEYNOT
        bool positionButtonsShowAsNotes = false; //OM TRUE VISA NOTER PÅ POSITIONTOGGLEBUTTONS

        List<Tuning> tunings = new List<Tuning> //STÄMNINGAR
        {
            
            new Tuning()
            {
                Name = "E Standard",
                Notes = new MusicNotes[] { MusicNotes.E, MusicNotes.A, MusicNotes.D, MusicNotes.G, MusicNotes.B, MusicNotes.E }
            },
            new Tuning()
            {
                Name = "Drop D",
                Notes = new MusicNotes[] { MusicNotes.D, MusicNotes.A, MusicNotes.D, MusicNotes.G, MusicNotes.B, MusicNotes.E, }
            },
            new Tuning()
            {
                Name = "D Standard",
                Notes = new MusicNotes[] { MusicNotes.D, MusicNotes.G,MusicNotes.C,MusicNotes.F,MusicNotes.A,MusicNotes.D, }
            },

        };

        

        Dictionary<int, Dictionary<int, MusicNotes?>> strings = new Dictionary<int, Dictionary<int, MusicNotes?>>() //NOTER PÅ GITARREN
        {
                {0, new Dictionary<int,MusicNotes?>
                    {
                        {0, null },{1, null}, {2, null},
                        {3, null}, {4, null}, {5, null},
                        {6, null}, {7, null}, {8, null},
                        {9, null}, {10, null}, {11, null},
                        {12, null}, {13, null}, {14, null},
                        {15, null}, {16, null}, {17, null},
                        {18, null}, {19, null}, {20, null},
                        {21, null}, {22, null},
                    } 
                },
                {1, new Dictionary<int,MusicNotes?>
                    {
                        {0, null },{1, null}, {2, null},
                        {3, null}, {4, null}, {5, null},
                        {6, null}, {7, null}, {8, null},
                        {9, null}, {10, null}, {11, null},
                        {12, null}, {13, null}, {14, null},
                        {15, null}, {16, null}, {17, null},
                        {18, null}, {19, null}, {20, null},
                        {21, null}, {22, null},
                    }
                },
                {2, new Dictionary<int,MusicNotes?>
                    {
                        {0, null },{1, null}, {2, null},
                        {3, null}, {4, null}, {5, null},
                        {6, null}, {7, null}, {8, null},
                        {9, null}, {10, null}, {11, null},
                        {12, null}, {13, null}, {14, null},
                        {15, null}, {16, null}, {17, null},
                        {18, null}, {19, null}, {20, null},
                        {21, null}, {22, null},
                    }
                },
                {3, new Dictionary<int,MusicNotes?>
                    {
                        {0, null },{1, null}, {2, null},
                        {3, null}, {4, null}, {5, null},
                        {6, null}, {7, null}, {8, null},
                        {9, null}, {10, null}, {11, null},
                        {12, null}, {13, null}, {14, null},
                        {15, null}, {16, null}, {17, null},
                        {18, null}, {19, null}, {20, null},
                        {21, null}, {22, null},
                    }
                },
                {4, new Dictionary<int,MusicNotes?>
                    {
                        {0, null },{1, null}, {2, null},
                        {3, null}, {4, null}, {5, null},
                        {6, null}, {7, null}, {8, null},
                        {9, null}, {10, null}, {11, null},
                        {12, null}, {13, null}, {14, null},
                        {15, null}, {16, null}, {17, null},
                        {18, null}, {19, null}, {20, null},
                        {21, null}, {22, null},
                    }
                },
                {5, new Dictionary<int,MusicNotes?>
                    {
                        {0, null },{1, null}, {2, null},
                        {3, null}, {4, null}, {5, null},
                        {6, null}, {7, null}, {8, null},
                        {9, null}, {10, null}, {11, null},
                        {12, null}, {13, null}, {14, null},
                        {15, null}, {16, null}, {17, null},
                        {18, null}, {19, null}, {20, null},
                        {21, null}, {22, null},
                    }
                },
        };

        public MainWindow()
        {
            InitializeComponent();

            WindowStartupLocation = WindowStartupLocation.Manual;
            Top = 100;
            Left = 100;

            if (!File.Exists(savedPositionsFileName))
            {
                using FileStream createStream = File.Create(savedPositionsFileName);
                var result = JsonSerializer.SerializeAsync(createStream, savedPositions);
                createStream.Dispose();
            }
            else
            {
                savedPositions = JsonSerializer.Deserialize<List<Positions>>(File.ReadAllText(savedPositionsFileName));
            }

            if(!File.Exists(savedTuningsFileName))
            {
                using FileStream createStream = File.Create(savedTuningsFileName);
                var result = JsonSerializer.SerializeAsync(createStream, tunings);
                createStream.Dispose();
            }
            else
            {
                tunings = JsonSerializer.Deserialize<List<Tuning>>(File.ReadAllText(savedTuningsFileName));
            }

            //Add Tunings To ComboBox
            foreach (var item in tunings)
            {
                tuningsComboBox.Items.Add(item);
            }
            //Add Notes To ComboBox
            foreach (var note in Enum.GetValues(typeof(MusicNotes)))
            {
                var noteName = Enum.GetName(typeof(MusicNotes), note);

                if(noteName.Length > 1)
                {
                    noteName = noteName[0] + "#";
                }
                var keyNote = new KeyNoteComboBoxItem()
                {
                    Name = noteName,
                    KeyNote = (MusicNotes)note,
                };
                keyNoteComboBox.Items.Add(keyNote);
            }
            //foreach (var position in savedPositions)
            //{
            //    SavedPositionsComboBox.Items.Add(position);
            //}


        }

        private void Reset_Click(object sender, RoutedEventArgs e) //RESET POSITIONER PÅ SKALAN
        {
            var toggleButtons = toggleButtonStackPanel.Children;

            foreach( var child in toggleButtons)
            {
                if(toggleButtons.IndexOf((UIElement)child) != 0)
                {
                    ((ToggleButton)child).IsChecked = false;
                }
                //if (((ToggleButton)child).Content != "R")//ÄNDRA OM IMPLEMENTERAR NOTER PÅ POSITIONER
                //{
                //    ((ToggleButton)child).IsChecked = false;
                //}
                
            }
            Array.Fill(positions, false,1,11);
            generateScalePattern();
        }

        private void RandomizePositions_Click(object sender, RoutedEventArgs e) //SLUMPMÄSSIGT GENERERA SKALA
        {
            Random random = new Random();
            for(int i = 1; i < positions.Length; i++)
            {
                positions[i] = random.Next(2) == 1 ? true : false;
            }

            for(int i = 1; i < positions.Length; i++)
            {
                ((ToggleButton)toggleButtonStackPanel.Children[i]).IsChecked = positions[i];
            }

            generateScalePattern();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) //ON START
        {
            

            hasLoaded = true;

        }

        private void positionToggleButton_Click(object sender, RoutedEventArgs e) //SÄTTER POSITION PÅ SKALA MANUELLT
        {
            var button = (ToggleButton)sender;
            var position = toggleButtonStackPanel.Children.IndexOf(button);
            positions[position] = !positions[position];

            generateScalePattern();
        }

        private void tuningDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)//ÄNDRA STÄMNING
        {
            if(tuningsComboBox.Items.Count > 0) 
            {
                setNotesOnStrings();
                setNotesOnButtons();
            }
            

            if(hasLoaded)
            {
                generateScalePattern();
            }

        }
        private void setNotesOnStrings()//LAGRAR NOTER I Dictionary<int, Dictionary<int, MusicNotes?>> strings
        {
            var tuningString = 0;
            foreach (var child in strings)
            {

                var gString = child.Value;


            var tuning = ((Tuning)tuningsComboBox.SelectedItem).Notes;

            var notes = (MusicNotes[])Enum.GetValues(typeof(MusicNotes));

            var notesPosition = 0;

            for (int i = 0; i < gString.Count; i++)
            {
                if (i == 0)
                {
                    gString[0] = tuning[tuningString];
                    notesPosition = Array.IndexOf(notes, tuning[tuningString]);
                }
                else
                {
                    gString[i] = notes[notesPosition];
                }
                notesPosition++;
                if (notesPosition >= 12)
                {
                    notesPosition = 0;
                }
            }
                tuningString++;
            }


        }

        private void setNotesOnButtons() //SKRIVER UT NOTER PÅ UI
        {
            var canvasStrings = stringCanvas.Children; //WHERE IM INPUTTING THE CONTENT/GONNA MARK THE SCALE NOTES
            
            var canvasNumber = 5; //INDEXING STRINGS

            foreach(var canvas in canvasStrings)
            {
                var currentStringNotes = strings[canvasNumber];
                //
                for (int i = 0; i < ((Canvas)canvas).Children.Count; i++)
                {
                    var button = (Button)((Canvas)canvas).Children[i];

                    var content = Enum.GetName(typeof(MusicNotes), currentStringNotes[i].Value);
                    
                    if(content.Length > 1)
                    {
                        content = content[0] + "#";
                    }

                    button.Content = content;
                    
                }
                canvasNumber--;
            }
        }

        //private void setScaleOntoNotes_Click(object sender, RoutedEventArgs e) //GENERERA SKALA(TA BORT KANSKE)
        //{
        //    generateScalePattern();
        //}

        public void generateScalePattern() //MARKERA SKALA MÖNSTER
        {
            var key = ((KeyNoteComboBoxItem)keyNoteComboBox.SelectedItem).KeyNote;
            var notes = (MusicNotes[])Enum.GetValues(typeof(MusicNotes));

            var noteIndex = Array.IndexOf(notes, key);

            var notesOnScale = new MusicNotes?[12];

            Array.Fill(notesOnScale, null); //GET NOTES FOR SCALE

            for (int i = 0; i < positions.Length; i++)
            {
                if (noteIndex >= 12)
                {
                    noteIndex = 0;
                }

                if (positions[i])
                {
                    notesOnScale[i] = notes[noteIndex];
                }
                noteIndex++;

            }

            var canvasStrings = stringCanvas.Children;
            var canvasIndex = 5;

            foreach (var canvas in canvasStrings)
            {
                var currentStringNotes = strings[canvasIndex];
                
                for (int i = 0; i < ((Canvas)canvas).Children.Count; i++)
                {
                    var button = (Button)((Canvas)canvas).Children[i];

                    if (notesOnScale.Any(x => x == currentStringNotes[i].Value))
                    {

                        if (currentStringNotes[i].Value == key)
                        {
                            button.Background = Brushes.Yellow;
                        }
                        else
                        {
                            button.Background = Brushes.Red;
                        }

                    }
                    else
                    {
                        button.Background = Brushes.DarkGray;
                        button.Foreground = Brushes.Black;
                    }

                }
                canvasIndex--;
            }
        }

        private void keyNoteComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)//GENERERA NYTT SKALAMÖNSTER BEROENDE PÅ KEY NÄR KEY VÄLJS
        {
            if (hasLoaded)
            {
                ChangeToggleButton(positionButtonsShowAsNotes);
            }
            
            generateScalePattern();
        }

        private void SavePositions_Click(object sender, RoutedEventArgs e)
        {
            AddScaleWindow addScaleWindow = new AddScaleWindow();
            addScaleWindow.Owner = this;
            addScaleWindow.Top = this.Top + 170;
            addScaleWindow.Left = this.Left + 30;
            ((MainWindow)Application.Current.MainWindow).IsEnabled = false;
            addScaleWindow.Show();
        }

        private void LoadPositions_Click(object sender, RoutedEventArgs e)
        {   LoadScaleWindow loadScaleWindow = new LoadScaleWindow();
            loadScaleWindow.Owner = this;
            loadScaleWindow.Top = Top + 170;
            loadScaleWindow.Left = Left + 30;
            ((MainWindow)Application.Current.MainWindow).IsEnabled = false;
            loadScaleWindow.Show();

        }

        private void AddTuning_Click(object sender, RoutedEventArgs e)
        {
            AddTuningWindow addtuningWindow = new AddTuningWindow();
            addtuningWindow.Owner = this;
            addtuningWindow.Top = Top + 170;
            addtuningWindow.Left = Left + 100;
            ((MainWindow)Application.Current.MainWindow).IsEnabled = false;
            addtuningWindow.Show();

        }

        private void ChangeToggleButtonDisplay_Click(object sender, RoutedEventArgs e)
        {
            positionButtonsShowAsNotes = !positionButtonsShowAsNotes;
            if (hasLoaded)
            {
                ChangeToggleButton(positionButtonsShowAsNotes);
            }
            if(positionButtonsShowAsNotes)
            {
                ((Button)sender).Content = "See Positions";
            }
            else
            {
                ((Button)sender).Content = "See Notes";
            }
            
        }

        private void ChangeToggleButton(bool positionButtonsShowAsNotes)
        {
            var noteList = keyNoteComboBox.Items;
            var toggleButtons = toggleButtonStackPanel.Children;
            var noteIndex = keyNoteComboBox.SelectedIndex;

            if (positionButtonsShowAsNotes)
            {
                for (int i = 0; i < toggleButtons.Count; i++)
                {
                    if (i != 0)
                    {
                        var noteString = Enum.GetName(typeof(MusicNotes), ((KeyNoteComboBoxItem)noteList[noteIndex]).KeyNote);
                        if (noteString.Length > 1)
                        {
                            noteString = noteString[0] + "#";
                        }
                        ((ToggleButton)toggleButtons[i]).Content = noteString;
                    }
                    noteIndex++;
                    if (noteIndex > 11)
                    {
                        noteIndex = 0;
                    }

                }
            }
            else
            {
                
                for(int i = 0;i < toggleButtons.Count;i++)
                {
                    if(i != 0)
                    {
                        ((ToggleButton)toggleButtons[i]).Content = (i + 1).ToString();
                    }
                }
            }
        }

        //private void LoadPositions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (!((ComboBox)sender).Items.IsEmpty) 
        //    {
        //        positions = ((Positions)((ComboBox)sender).SelectedItem).NotePositions;

        //        //FOR LOOP TO CHECK POSITION BUTTONS
        //        for (int i = 1; i < positions.Length; i++)
        //        {
        //            ((ToggleButton)toggleButtonStackPanel.Children[i]).IsChecked = positions[i];
        //        }
        //        generateScalePattern();
        //    }

        //}
    }
}
