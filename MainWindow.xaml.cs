using System.CodeDom;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace redo
{
    public partial class MainWindow : Window
    {
        class Card {
            public string text { get; set; }
            public int pairId { get; set; }
        }

        Random rnd = new Random();
        private Button firstButton;
        private Button secondButton;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Click_StartGameButton(object sender, RoutedEventArgs e)
        {
            bool isSizeChosen = (lstbox_sizeListbox.SelectedItem != null);
            bool isThemeChosen = (lstbox_themeListbox.SelectedItem != null);

            ListBoxItem selectedSizeRaw = (ListBoxItem)lstbox_sizeListbox.SelectedItem;
            ListBoxItem selectedThemeRaw = (ListBoxItem)lstbox_themeListbox.SelectedItem;

            int selectedSizeInt = int.Parse(selectedSizeRaw.Tag.ToString());
            string selectedThemeStr = selectedThemeRaw.Tag.ToString();

            int pairsCount = selectedSizeInt * selectedSizeInt / 2;

            int cardIndex = 0;

            List<Card> finalCardList = new List<Card>();

            Random random = new Random();

            if (isSizeChosen != true || isThemeChosen != true) return;

            finalCardList = GenerateAndShuffle(selectedThemeStr,selectedSizeInt);

            for (int i = 0; i < selectedSizeInt; i++)
            {
                grd_gameGrid.RowDefinitions.Add(new RowDefinition());
                grd_gameGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int row = 0; row < selectedSizeInt; row++)
            {
                for (int column = 0; column < selectedSizeInt; column++)
                {
                    Button btn = new Button
                    {
                        Content = "?",
                        FontSize = 20,
                        FontWeight = FontWeights.Bold,
                        Tag = finalCardList[cardIndex]
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
            Button clickedButton = sender as Button;
            if (clickedButton == null || clickedButton.Content.ToString() != "?") return;

            Card outCardData = (Card)clickedButton.Tag;
            clickedButton.Content = outCardData.text;

            if (firstButton == null) {
                firstButton = clickedButton;
            }
            else {
                secondButton = clickedButton;
                // Attempts ++
                // Check for Match
            }
        }
        private List<Card> GenerateAndShuffle(string theme, int pairsCount){

            List<Card> cards = new List<Card>();
            List<string> baseItems = new List<string>();

            if (theme == "Numbers")
            {
                baseItems = Enumerable.Range(1, pairsCount).Select(i => i.ToString()).ToList();

                for (int i = 0; i < baseItems.Count; i++)
                {
                    cards.Add(new Card { text = baseItems[i], pairId = i });
                    cards.Add(new Card { text = baseItems[i], pairId = i });
                }
            }

            else if (theme == "Smiley")
            {
                string[] smileys = { "😀", "😃", "😄", "😁", "😆", "😅", "😂", "🤣", "😊", "😇" };
                baseItems = smileys.Take(pairsCount).ToList();

                for (int i = 0; i < baseItems.Count; i++)
                {
                    cards.Add(new Card { text = baseItems[i], pairId = i });
                    cards.Add(new Card { text = baseItems[i], pairId = i });
                }
            }

            else if (theme == "Countries")
            {
                var countryData = new[]
                {
                    Tuple.Create("Magyarország", "Budapest"), Tuple.Create("Franciaország", "Párizs"),
                    Tuple.Create("Németország", "Berlin"), Tuple.Create("Olaszország", "Róma"),
                    Tuple.Create("Spanyolország", "Madrid"), Tuple.Create("Anglia", "London"),
                    Tuple.Create("Ausztria", "Bécs"), Tuple.Create("Japán", "Tokió"),
                    Tuple.Create("USA", "Washington"), Tuple.Create("Kína", "Peking")
                };

                var selected = countryData.Take(pairsCount).ToList();

                for (int i = 0; i < selected.Count; i++)
                {
                    cards.Add(new Card { text = selected[i].Item1, pairId = i });
                    cards.Add(new Card { text = selected[i].Item2, pairId = i });
                }
            }

            cards.Shuffle();
            return cards;
        }
    }
}