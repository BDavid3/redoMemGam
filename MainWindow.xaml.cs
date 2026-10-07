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

        // DNF -> Clear the Board after finishing game
        private bool isSizeChosen;
        private bool isThemeChosen;
        private ListBoxItem selectedSize;
        private ListBoxItem selectedTheme;
        private int selectedSizeInt;
        private string selectedThemeStr;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Click_StartGameButton(object sender, RoutedEventArgs e)
        {

            // Clear the full game stat




            isSizeChosen = (lstbox_sizeListbox.SelectedItem != null);
            isThemeChosen = (lstbox_themeListbox.SelectedItem != null);

            if (isSizeChosen != true || isThemeChosen != true) return;

            // Create the Rows and Columns

            selectedSize = (ListBoxItem)lstbox_sizeListbox.SelectedItem;
            selectedTheme = (ListBoxItem)lstbox_themeListbox.SelectedItem;
            selectedSizeInt = int.Parse(selectedSize.Tag.ToString());
            selectedThemeStr = selectedTheme.Tag.ToString();

            for (int i = 0; i < selectedSizeInt; i++)
            {
                grd_gameGrid.RowDefinitions.Add(new RowDefinition());
                grd_gameGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int row = 0; row < selectedSizeInt; row++)
            {
                for (int column = 0; column < selectedSizeInt; column++)
                {
                    Button newButton = new Button
                    {
                        Content = "?",
                        FontSize = 20,
                        FontWeight = FontWeights.Bold
                    };

                    newButton.Click += Button_Click;
                    Grid.SetRow(newButton, row);
                    Grid.SetColumn(newButton, column);
                    grd_gameGrid.Children.Add(newButton);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}