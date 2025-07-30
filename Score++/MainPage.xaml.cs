namespace Score__;
#pragma warning disable CS0618
#pragma warning disable XC0022
public partial class MainPage : ContentPage
{
    readonly GameSession session;
    readonly Stack<(string player, int delta)> undoStack = new();


    public MainPage(GameSession loadedSession)
    {
        InitializeComponent();
        session = loadedSession;
        playerPicker.ItemsSource = session.PlayerScores.Keys.ToList();
        UpdateScoreDisplay();
    }


    void OnAddScoreClicked(object sender, EventArgs e)
    {
        AddScore();
    }

    void OnSubtractScoreClicked(object sender, EventArgs e)
    {
        SubtractScore();
    }

    string lastPlayer = null!;
    int lastPoints = 0;

    void AddScore()
    {
        string selectedPlayer = playerPicker.SelectedItem as string ?? null!;
        if (selectedPlayer == null || string.IsNullOrWhiteSpace(scoreEntry.Text)) return;

        if (int.TryParse(scoreEntry.Text, out int points))
        {
            session.PlayerScores[selectedPlayer] += points;

            lastPlayer = selectedPlayer;
            lastPoints = points;

            undoStack.Push((selectedPlayer, points));

            scoreEntry.Text = string.Empty;
            UpdateScoreDisplay();
        }
    }

    void SubtractScore()
    {
        string selectedPlayer = playerPicker.SelectedItem as string ?? null!;
        if (selectedPlayer == null || string.IsNullOrWhiteSpace(scoreEntry.Text)) return;

        if (int.TryParse(scoreEntry.Text, out int points))
        {
            session.PlayerScores[selectedPlayer] -= points;

            lastPlayer = selectedPlayer;
            lastPoints = points;

            undoStack.Push((selectedPlayer, -points));

            scoreEntry.Text = string.Empty;
            UpdateScoreDisplay();
        }
    }

    void OnUndoClicked(object sender, EventArgs e)
    {
        if (undoStack.Any())
        {
            var (player, delta) = undoStack.Pop();
            session.PlayerScores[player] -= delta;
            UpdateScoreDisplay();
        }
    }

    void OnExitToHome(object sender, EventArgs e)
    {
        Application.Current!.MainPage = new HomePage();
    }


    void UpdateScoreDisplay()
    {
        scoreDisplay.Text = string.Join("\n", session.PlayerScores.Select(p => $"{p.Key}: {p.Value}"));
        var sessions = SessionStorage.LoadSessions();
        for (int i = 0; i < sessions.Count; i++)
        {
            if (sessions[i].SessionId == session.SessionId)
            {
                sessions[i] = session;
                break;
            }
        }
        SessionStorage.SaveSessions(sessions);
    }
    
    async void OnDeleteCurrentSession(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert(
            "Confirm Delete",
            "Are you sure you want to delete this session?",
            "Delete",
            "Cancel");

        if (confirm)
        {
            var all = SessionStorage.LoadSessions();
            all = all.Where(s => s.SessionId != session.SessionId).ToList();
            SessionStorage.SaveSessions(all);
            Application.Current!.MainPage = new HomePage();
        }
    }

}
