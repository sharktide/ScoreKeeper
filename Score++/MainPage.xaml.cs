namespace Score__;

public partial class MainPage : ContentPage
{
    Dictionary<string, int> playerScores = new()
    {
        { "Player 1", 0 },
        { "Player 2", 0 },
        { "Player 3", 0 }
    };

    public MainPage()
    {
        InitializeComponent();
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

	string lastPlayer = null;
	int lastPoints = 0;

	void AddScore()
	{
		string selectedPlayer = playerPicker.SelectedItem as string;
		if (selectedPlayer == null || string.IsNullOrWhiteSpace(scoreEntry.Text)) return;

		if (int.TryParse(scoreEntry.Text, out int points))
		{
			playerScores[selectedPlayer] += points;

			// Save last action
			lastPlayer = selectedPlayer;
			lastPoints = points;

			scoreEntry.Text = string.Empty;
			UpdateScoreDisplay();
		}
	}

    void SubtractScore()
    {
        string selectedPlayer = playerPicker.SelectedItem as string;
		if (selectedPlayer == null || string.IsNullOrWhiteSpace(scoreEntry.Text)) return;

        if (int.TryParse(scoreEntry.Text, out int points))
        {
            playerScores[selectedPlayer] -= points;

            lastPlayer = selectedPlayer;
            lastPoints = points;

            scoreEntry.Text = string.Empty;
            UpdateScoreDisplay();
        }
    }

	void OnUndoClicked(object sender, EventArgs e)
	{
		if (lastPlayer != null && lastPoints != 0)
		{
			playerScores[lastPlayer] -= lastPoints;
			UpdateScoreDisplay();

			lastPlayer = null;
			lastPoints = 0;
		}
	}

    void UpdateScoreDisplay()
    {
        scoreDisplay.Text = string.Join("\n", playerScores.Select(p => $"{p.Key}: {p.Value}"));
    }
}
