using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace redo
{
    public partial class MainWindow : Window
    {
        class Card {
            public string text { get; set; }
            public int pairId { get; set; }
        }

        private Button firstButton;
        private Button secondButton;
        private int attempts;
        private int matchedPairs;
        private int totalPairs;
        private bool isBusy;
        private readonly DispatcherTimer flipBackTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            flipBackTimer.Interval = TimeSpan.FromMilliseconds(800);
            flipBackTimer.Tick += FlipBackTimer_Tick;
        }

        private void Click_StartGameButton(object sender, RoutedEventArgs e)
        {
            if (lstbox_sizeListbox.SelectedItem == null || lstbox_themeListbox.SelectedItem == null)
            {
                MessageBox.Show("Please select size and theme.");
                return;
            }

            ResetBoard();

            ListBoxItem selectedSizeRaw = (ListBoxItem)lstbox_sizeListbox.SelectedItem;
            ListBoxItem selectedThemeRaw = (ListBoxItem)lstbox_themeListbox.SelectedItem;
            int size = int.Parse(selectedSizeRaw.Tag.ToString());
            string theme = selectedThemeRaw.Tag.ToString();


            totalPairs = (size * size) / 2;
            List<Card> cardList = GenerateAndShuffle(theme, totalPairs);

            if (cardList.Count != size * size)
            {
                MessageBox.Show($"Expected {size * size} cards, got {cardList.Count}.\nTheme tag: '{theme}', size: {size}");
                return;
            }

            for (int i = 0; i < size; i++)
            {
                grd_gameGrid.RowDefinitions.Add(new RowDefinition());
                grd_gameGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            int cardIndex = 0;
            for (int row = 0; row < size; row++)
            {
                for (int column = 0; column < size; column++)
                {
                    Button btn = new Button
                    {
                        Content = "?",
                        FontSize = 20,
                        FontWeight = FontWeights.Bold,
                        Tag = cardList[cardIndex]
                    };

                    btn.Click += Button_Click;

                    Grid.SetRow(btn, row);
                    Grid.SetColumn(btn, column);
                    grd_gameGrid.Children.Add(btn);

                    cardIndex++;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isBusy) return;

            Button clickedButton = sender as Button;
            if (clickedButton == null || clickedButton.Content.ToString() != "?") return;

            Card outCardData = (Card)clickedButton.Tag;
            clickedButton.Content = outCardData.text;

            if (firstButton == null) {
                firstButton = clickedButton;
            }
            else {
                secondButton = clickedButton;
                attempts++;
                txtblock_attempts.Text = attempts.ToString();
                CheckForMatch();
            }
        }
        private void CheckForMatch(){
            Card tempFirstCard = (Card)firstButton.Tag;
            Card tempSecondCard = (Card)secondButton.Tag;

            if (tempFirstCard.pairId == tempSecondCard.pairId)
            {
                firstButton.IsEnabled = false;
                secondButton.IsEnabled = false;
                firstButton = null;
                secondButton = null;
                matchedPairs++;

                if (matchedPairs == totalPairs)
                {
                    MessageBox.Show($"You finished in {attempts} attempts");
                    ResetBoard();
                }
            }
            else
            {
                isBusy = true;
                flipBackTimer.Start();
            }
        }
        private void FlipBackTimer_Tick(object sender, EventArgs e)
        {
            flipBackTimer.Stop();

            firstButton.Content = "?";
            secondButton.Content = "?";
            firstButton = null;
            secondButton = null;
            isBusy = false;
        }
        private List<Card> GenerateAndShuffle(string theme, int pairsCount){

            List<Card> cards = new List<Card>();
            List<string> baseItems = new List<string>();

            if (theme == "numbers")
            {
                baseItems = Enumerable.Range(1, pairsCount).Select(i => i.ToString()).ToList();

                for (int i = 0; i < baseItems.Count; i++)
                {
                    cards.Add(new Card { text = baseItems[i], pairId = i });
                    cards.Add(new Card { text = baseItems[i], pairId = i });
                }
            }

            else if (theme == "smiley")
            {
                string[] smileys = { "😀", "😃", "😄", "😁", "😆", "😅", "😂", "🤣", "😊", "😇",
                                        "🙂", "🙃", "😉", "😌", "😍", "🥰", "😘", "😗", "😙", "😚",
                                        "😋", "😛", "😝", "😜", "🤪", "🤨", "🧐", "🤓", "😎", "🥸",
                                        "🤩", "🥳", "😏", "😒", "😞", "😔", "😟", "😕" };
                baseItems = smileys.Take(pairsCount).ToList();

                for (int i = 0; i < baseItems.Count; i++)
                {
                    cards.Add(new Card { text = baseItems[i], pairId = i });
                    cards.Add(new Card { text = baseItems[i], pairId = i });
                }
            }

            else if (theme == "countries")
            {
                var countryData = new[]
                {
                    Tuple.Create("Magyarország", "Budapest"), Tuple.Create("Franciaország", "Párizs"),
                    Tuple.Create("Németország", "Berlin"), Tuple.Create("Olaszország", "Róma"),
                    Tuple.Create("Spanyolország", "Madrid"), Tuple.Create("Anglia", "London"),
                    Tuple.Create("Ausztria", "Bécs"), Tuple.Create("Japán", "Tokió"),
                    Tuple.Create("USA", "Washington"), Tuple.Create("Kína", "Peking"),
                    Tuple.Create("Görögország", "Athén"), Tuple.Create("Egyiptom", "Kairó"),
                    Tuple.Create("Brazília", "Brazíliaváros"), Tuple.Create("Ausztrália", "Canberra"),
                    Tuple.Create("Kanada", "Ottawa"), Tuple.Create("Oroszország", "Moszkva"),
                    Tuple.Create("Svájc", "Bern"), Tuple.Create("Csehország", "Prága"),
                    Tuple.Create("Lengyelország", "Varsó"), Tuple.Create("Svédország", "Stockholm"),
                    Tuple.Create("Norvégia", "Oslo"), Tuple.Create("Finnország", "Helsinki"),
                    Tuple.Create("Dánia", "Koppenhága"), Tuple.Create("Törökország", "Ankara"),
                    Tuple.Create("India", "Újdelhi"), Tuple.Create("Mexikó", "Mexikóváros"),
                    Tuple.Create("Dél-Korea", "Szöul"), Tuple.Create("Argentína", "Buenos Aires"),
                    Tuple.Create("Portugália", "Lisszabon"), Tuple.Create("Belgium", "Brüsszel"),
                    Tuple.Create("Hollandia", "Amszterdam"), Tuple.Create("Írország", "Dublin")
                };

                var selected = countryData.Take(pairsCount).ToList();

                for (int i = 0; i < selected.Count; i++)
                {
                    cards.Add(new Card { text = selected[i].Item1, pairId = i });
                    cards.Add(new Card { text = selected[i].Item2, pairId = i });
                }
            }

            cards = cards.OrderBy(i => Guid.NewGuid()).ToList();
            return cards;
        }
        private void ResetBoard()
        {
            flipBackTimer.Stop();
            isBusy = false;
            firstButton = null;
            secondButton = null;
            attempts = 0;
            txtblock_attempts.Text = attempts.ToString();
            matchedPairs = 0;

            grd_gameGrid.Children.Clear();
            grd_gameGrid.RowDefinitions.Clear();
            grd_gameGrid.ColumnDefinitions.Clear();
        }
    }
}