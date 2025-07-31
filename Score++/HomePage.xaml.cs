namespace Score__;
#pragma warning disable CS0618
public partial class HomePage : ContentPage
{
    List<GameSession> sessions;

    public HomePage()
    {
        InitializeComponent();
        sessions = SessionStorage.LoadSessions();
        sessionList.ItemsSource = sessions;
        emptySessionsLabel.IsVisible = sessions.Count == 0;
    }

    private void OnStartNewGame(object sender, EventArgs e)
    {
        Application.Current!.MainPage = new NewGamePage();
    }

    private void OnResumeGame(object sender, EventArgs e)
    {
        var lastSession = sessions.LastOrDefault();
        if (lastSession != null)
        {
            var mainPage = new MainPage(lastSession);
            Application.Current!.MainPage = mainPage;
        }
        else
        {
            DisplayAlert("Error", "No saved session found!", "OK");
        }
    }

    private void OnLoadClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is string sessionId)
        {
            var selected = sessions.FirstOrDefault(s => s.SessionId == sessionId);
            if (selected != null)
            {
                var mainPage = new MainPage(selected);
                Application.Current!.MainPage = mainPage;
            }
        }
    }
    private async void OnDeleteSessionClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is string sessionId)
        {
            bool confirm = await DisplayAlert(
                "Confirm Delete",
                "Are you sure you want to delete this session?",
                "Delete",
                "Cancel");

            if (confirm)
            {
                sessions = sessions.Where(s => s.SessionId != sessionId).ToList();
                SessionStorage.SaveSessions(sessions);
                sessionList.ItemsSource = sessions;
            }
        }
    }


}
