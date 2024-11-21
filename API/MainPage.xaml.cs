namespace API
{
    public partial class MainPage : ContentPage
    {
        string[] nobelPrizeCategories=
        [
            "Chemistry", 
            "Economic Sciences", 
            "Literature", 
            "Peace", 
            "Physics",
            "Physiology or Medicine"  
        ];

        List<PrizeWinner> prizeWinners = [];

        public MainPage()
        {
            InitializeComponent();
            CategoryPicker.ItemsSource = nobelPrizeCategories;
        }

        private void SubmitButton_OnClicked(object? sender, EventArgs e)
        {
            int rok;
            if (string.IsNullOrWhiteSpace(YearEntry.Text) || CategoryPicker.SelectedItem is null || !int.TryParse(YearEntry.Text, out rok))
            {
                DisplayAlert("Error", "Zadejte validní data", "OK");
                return;
            }

            if (rok < 1901 || rok > DateTime.Now.Year)
            {
                DisplayAlert("Error", "V této době nebyly udělelny žádné ceny", "OK");
            }
            var root = PrizeWinner.GetApiData(CategoryPicker.SelectedItem.ToString(), rok);
            if (root[0].laureates is null)
            {
                DisplayAlert("Error", "Tuto kategorii v daném roce nikdo nevyhrál :(", "OK");
                return;
            }
            prizeWinners = [];
            for (var i = 0; i < root[0].laureates.Count; i++)
            {
                var laureate = root[0].laureates[i];
                prizeWinners.Add(new (laureate, CategoryPicker.SelectedItem.ToString(), root[0]));
            }
            InfoLayout.Children.Clear();
            Refresh();
        }

        private void Refresh()
        {
            ResultListView.ItemsSource = null;
            ResultListView.ItemsSource = prizeWinners;
        }

        private void ResultListView_OnItemSelected(object? sender, SelectedItemChangedEventArgs e)
        {
            var laureat = ResultListView.SelectedItem as PrizeWinner;
            if (laureat is null) return;

            InfoLayout.Children.Clear();
            Label nameLbl = new()
            {
                FontSize = 30,
                Text = laureat.Name,
                FontAttributes = FontAttributes.Bold
            };
            Label categoryLbl = new()
            {
                FontSize = 20,
                Text = laureat.Category
            };
            Label descriptionLbl = new()
            {
                FontSize = 15,
                Text = "\n" + laureat.Description
            };
            Label prizeAmountLbl = new()
            {
                FontSize = 15,
                Text = $"\nPrize amount: {laureat.prizeAmount} öre"
            };
            Label prizeAmountAdjustedLbl = new()
            {
                FontSize = 15,
                Text = $"Prize amount adjusted: {laureat.prizeAmountAdjusted} öre"
            };

            InfoLayout.Children.Add(nameLbl);
            InfoLayout.Children.Add(categoryLbl);
            InfoLayout.Children.Add(prizeAmountLbl);
            InfoLayout.Children.Add(prizeAmountAdjustedLbl);
            InfoLayout.Children.Add(descriptionLbl);
        }
    }

}
