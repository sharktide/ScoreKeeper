namespace Score__;

public partial class NewGamePage : ContentPage
{
    public NewGamePage()
    {
        InitializeComponent();
    }

    void OnCreateFields(object sender, EventArgs e)
    {
        playerNameFields.Children.Clear();
        if (int.TryParse(playerCountEntry.Text, out int count) && count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                playerNameFields.Children.Add(new Entry
                {
                    Placeholder = $"Player {i + 1}",
                    Margin = new Thickness(0, 4)
                });
            }
        }
    }

    void OnStartGame(object sender, EventArgs e)
    {
        var playerNames = playerNameFields.Children
            .OfType<Entry>()
            .Select(entry => entry.Text)
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .Distinct()
            .ToList();

        if (playerNames.Count == 0)
        {
            DisplayAlert("Error", "Please enter at least one player name.", "OK");
            return;
        }

        var session = new GameSession
        {
            SessionName = $"{DateTime.Now:MMM dd, yyyy - HH:mm}",
            PlayerScores = playerNames.ToDictionary(name => name, _ => 0)
        };

        var sessions = SessionStorage.LoadSessions();
        sessions.Add(session);
        SessionStorage.SaveSessions(sessions);

        #pragma warning disable CS0618
        Application.Current!.MainPage = new MainPage(session);
        #pragma warning restore CS0618
    }
}
