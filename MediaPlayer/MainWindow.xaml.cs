using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
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

namespace MediaPlayer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        bool repeat = false;
        public MainWindow()
        {
            InitializeComponent();

            Thread thread = new Thread(_ =>
            {
                while (true)
                {
                    this.Dispatcher.Invoke(() => { 
                        audioSlider.Value = media.Position.Ticks;
                        labl1.Content=media.Position.ToString(@"mm\ss");
                        if (media.NaturalDuration.HasTimeSpan)
                        {
                            TimeSpan posr = media.NaturalDuration.TimeSpan - media.Position;
                            labl2.Content = posr.ToString(@"mm\ss");
                        }
                    });
                    Thread.Sleep(1000);
                }

            });
            thread.Start();

        }



        private void audioSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            media.Position = new TimeSpan(Convert.ToInt64(audioSlider.Value));

        }
        private void media_MediaOpened(object sender, RoutedEventArgs e)
        {
            audioSlider.Value = 0;
            audioSlider.Maximum = media.NaturalDuration.TimeSpan.Ticks;


        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog { IsFolderPicker = true };
            var result = dialog.ShowDialog();


            if (result == CommonFileDialogResult.Ok)
            {
                string[] files = Directory.GetFiles(dialog.FileName, "*.mp3");
                foreach (string file in files)
                {
                    track_list.Items.Add(file);

                }
                media.Source = new Uri(files[0]);
                track_list.SelectedIndex = 0;
            }

            media.Play();
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            media.Play();
        }

        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            media.Pause();
        }

        private void Button_Click4(object sender, RoutedEventArgs e)
        {
            if (track_list.SelectedIndex < track_list.Items.Count - 1)
            {
                track_list.SelectedIndex = track_list.SelectedIndex + 1;

            }
            play();
        }

        private void Button_Click5(object sender, RoutedEventArgs e)
        {
            if (track_list.SelectedIndex > 0)
            {
                track_list.SelectedIndex = track_list.SelectedIndex - 1;
            }
            play();
        }

        private void track_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            play();
        }

        private void Button_Click6(object sender, RoutedEventArgs e)
        {
            repeat = !repeat;
        }

        private void media_MediaEnded(object sender, RoutedEventArgs e)
        {
            track_list.SelectedIndex++;
            if (repeat)
            {
                track_list.SelectedIndex--;
            }
        }
        private void play()
        {
            int a = track_list.SelectedIndex;
            media.Source = new Uri((string)track_list.Items[a]);
            media.Play();
        }

        private void Button_Click7(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            
           
                media.Source = new Uri(track_list.Items[random.Next(0, track_list.Items.Count)].ToString());
                media.Play();
            
        }
    }

}

 