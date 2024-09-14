using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using MsBox.Avalonia;
using System;
using System.Collections.Generic;
using System.Timers;

namespace Pop
{
    public partial class MainWindow : Window
    {
        private static Timer _timer;
        List<string> sentences = new List<string>();
        List<int> poppedSentenceIndex = new List<int>();
        public MainWindow()
        {
            InitializeComponent();

            Closing += (sender, e) =>
            {
                e.Cancel = true;
                Hide();
            };
        }
        public async void ClickHandler(object sender, RoutedEventArgs args)
        {
            sentences.Clear();
            if (SentencesTextBox.Text == null)
            {
                var box = MessageBoxManager.GetMessageBoxStandard("Error!", "You should enter at least one sentence.");
                await box.ShowAsync();
            }
            else if (SentencesTextBox.Text != null && !string.IsNullOrEmpty(SentencesTextBox.Text))
            {
                string sentence = "";
                for (int i = 0; i < SentencesTextBox.Text.Length; i++)
                {
                    if (SentencesTextBox.Text[i] != ';')
                    {
                        sentence += SentencesTextBox.Text[i];
                    }
                    else if (SentencesTextBox.Text[i] == ';' && !string.IsNullOrEmpty(sentence))
                    {
                        sentences.Add(sentence);
                        sentence = "";
                    }
                    if (i == SentencesTextBox.Text.Length - 1 && SentencesTextBox.Text[i] != ';')
                    {
                        sentences.Add(sentence);
                    }
                }
                if (sentences != null)
                {
                    var box = MessageBoxManager.GetMessageBoxStandard("Saved.", "Your sentences are saved, your pops will show after a while.");
                    await box.ShowAsync();
                }

            }
            TimeHandler();
        }

        private void TimeHandler()
        {
            _timer = new Timer((Convert.ToUInt16(TimerIntervalInput.Text) * 1000) * 60);

            _timer.Elapsed += OnTimedEvent;

            _timer.AutoReset = true;
            _timer.Enabled = true;
        }
        private async void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Console.WriteLine("The timer elapsed. Calling the function...");
            Random rnd = new Random();
            int r = rnd.Next(sentences.Count);
            if (sentences.Count > 1) 
            {
                if (poppedSentenceIndex.Count == sentences.Count)
                {
                    poppedSentenceIndex.Clear();
                }
                while (poppedSentenceIndex.Contains(r))
                {
                    r = rnd.Next(sentences.Count);
                }
                poppedSentenceIndex.Add(r);
            }

            Dispatcher.UIThread.Post(() =>
            {
                var popup = new PopUp();
                popup.ShowNotificationAsync("Pop!", sentences[r]);
            });
        }
    }
}