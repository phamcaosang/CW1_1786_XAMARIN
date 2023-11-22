using System.Collections.ObjectModel;

namespace app1786;

public partial class MainPage : ContentPage
{
    private SQLiteDatabase database;
    private ObservableCollection<Hike> hikeList;

    public MainPage()
    {
        InitializeComponent();
        database = new SQLiteDatabase();
        LoadHikeList();
    }

    private void LoadHikeList()
    {
        hikeList = database.GetHikeList();
        hikeListView.ItemsSource = hikeList;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        hikeListView.ItemSelected += async (sender, e) =>
        {
            if (e.SelectedItem == null)
                return;

            Hike selectedHike = (Hike)e.SelectedItem;

            string hikeDetails = $"ID: {selectedHike.Id}\n" + 
                                $"Name: {selectedHike.Name}\n" +
                                $"Location: {selectedHike.Location}\n" +
                                $"Date: {selectedHike.Date}\n" +
                                $"Parking Availability: {(selectedHike.ParkingAvailability ? "Yes" : "No")}\n" +
                                $"Length: {selectedHike.Length}\n" +
                                $"Difficulty: {selectedHike.Difficulty}\n" +
                                $"Description: {selectedHike.Description}";

            await DisplayAlert("Hike Details", hikeDetails, "OK");

            // Deselect the item
            hikeListView.SelectedItem = null;
        };
    }

    private async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        // Get the selected Hike object associated with the clicked button
        var button = (Button)sender;
        var hike = (Hike)button.BindingContext;

        // Display a confirmation dialog
        bool confirmed = await DisplayAlert("Confirm Deletion", "Are you sure you want to delete this hike?" + hike.Id.ToString(), "Yes", "No");

        if (confirmed)
        {
            database.DeleteHike(hike.Id);
            LoadHikeList();

        }
    }


    private void OnSubmitButtonClicked(object sender, EventArgs e)
    {
        string name = nameEditText.Text;
        string location = locationEditText.Text;
        string dateStr = dateEditText.Text;
        bool parkingAvailability = yesRadioButton.IsChecked;
        string lengthStr = lengthEditText.Text;
        string difficulty = difficultyEditText.Text;
        string description = descriptionEditText.Text;

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(location) || string.IsNullOrWhiteSpace(dateStr) || string.IsNullOrWhiteSpace(lengthStr))
        {
            messageLabel.Text = "Please fill in all required fields.";
            return;
        }

        if (!DateTime.TryParse(dateStr, out _))
        {
            messageLabel.Text = "Please enter a valid date (yyyy-MM-dd).";
            return;
        }

        if (!double.TryParse(lengthStr, out _))
        {
            messageLabel.Text = "Please enter a valid length.";
            return;
        }

        var newHike = new Hike
        {
            Name = name,
            Location = location,
            Date = dateStr,
            ParkingAvailability = parkingAvailability,
            Length = lengthStr,
            Difficulty = difficulty,
            Description = description
        };

        int hikeId = database.InsertHike(newHike);
        if (hikeId > 0)
        {
            hikeList.Add(newHike);
            LoadHikeList();
            messageLabel.Text = "Hike added successfully.";
            ClearForm();
        }
        else
        {
            messageLabel.Text = "Error adding hike.";
        }
    }



    private void ClearForm()
    {
        nameEditText.Text = string.Empty;
        locationEditText.Text = string.Empty;
        dateEditText.Text = string.Empty;
        yesRadioButton.IsChecked = false;
        noRadioButton.IsChecked = false;
        lengthEditText.Text = string.Empty;
        difficultyEditText.Text = string.Empty;
        descriptionEditText.Text = string.Empty;
    }
}