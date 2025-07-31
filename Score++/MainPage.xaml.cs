namespace Score__;
#pragma warning disable CS0618
public partial class MainPage : ContentPage
{
    private readonly GameSession session;
    private readonly Stack<(string player, int delta)> undoStack = new();
    private readonly Stack<(string player, int delta)> redoStack = new();



    public MainPage(GameSession loadedSession)
    {
        InitializeComponent();
        session = loadedSession;
        playerPicker.ItemsSource = session.PlayerScores.Keys.ToList();
        UpdateScoreDisplay();
    }


    private void OnAddScoreClicked(object sender, EventArgs e)
    {
        AddScore();
    }

    private void OnSubtractScoreClicked(object sender, EventArgs e)
    {
        SubtractScore();
    }

    private string lastPlayer = null!;
    private int lastPoints = 0;

    private void AddScore()
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

    private void SubtractScore()
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

    private void OnUndoClicked(object sender, EventArgs e)
    {
        if (undoStack.Count != 0)
        {
            var (player, delta) = undoStack.Pop();
            session.PlayerScores[player] -= delta;
            redoStack.Push((player, delta));
            UpdateScoreDisplay();
        }
    }

    private void OnRedoClicked(object sender, EventArgs e)
    {
        if (redoStack.Count != 0)
        {
            var (player, delta) = redoStack.Pop();
            session.PlayerScores[player] += delta;
            undoStack.Push((player, delta));
            UpdateScoreDisplay();
        }
    }

    private void OnExitToHome(object sender, EventArgs e)
    {
        Application.Current!.MainPage = new HomePage();
    }


    private void UpdateScoreDisplay()
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
    
    private async void OnDeleteCurrentSession(object sender, EventArgs e)
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
