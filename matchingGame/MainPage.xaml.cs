using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MatchingGame
{
    public partial class MainPage : ContentPage
    {
        private List<string> icons = new List<string>
        {
            "🍎", "🍌", "🍒", "🍓", "🍑", "🍍",
            "🥑", "🥕", "🍇", "🍈", "🥥", "🍅"
        };

        private List<Button> cardButtons = new List<Button>();
        private string[] cardValues;
        private Button firstCard, secondCard;
        private bool isBusy = false;

        public MainPage()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            // Duplicate the icons to create pairs
            List<string> pairedIcons = icons.Concat(icons).ToList();

            // Shuffle the icons
            Random rand = new Random();
            cardValues = pairedIcons.OrderBy(x => rand.Next()).ToArray();

            // Create buttons for each card
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Button button = new Button
                    {
                        Text = "",
                        FontSize = 24,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    };

                    button.Clicked += OnCardClicked;

                    cardButtons.Add(button);
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    Grid.Children.Add(button);
                }
            }
        }

        private void OnCardClicked(object sender, EventArgs e)
        {
            if (isBusy)
                return;

            var clickedCard = (Button)sender;

            if (clickedCard == null || clickedCard.Text != "" || (firstCard != null && secondCard != null))
                return;

            clickedCard.Text = cardValues[cardButtons.IndexOf(clickedCard)];

            if (firstCard == null)
            {
                firstCard = clickedCard;
            }
            else
            {
                secondCard = clickedCard;
                isBusy = true;

                // Check for a match
                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    if (firstCard.Text == secondCard.Text)
                    {
                        firstCard.IsEnabled = false;
                        secondCard.IsEnabled = false;

                        if (cardButtons.All(b => !b.IsEnabled))
                        {
                            lblResult.Text = "Congratulations! You matched all cards!";
                        }
                    }
                    else
                    {
                        firstCard.Text = "";
                        secondCard.Text = "";
                    }

                    firstCard = null;
                    secondCard = null;
                    isBusy = false;

                    return false;
                });
            }
        }
    }
}
