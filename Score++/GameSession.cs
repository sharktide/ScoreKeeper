namespace Score__;
using System.Text.Json;
public class GameSession
{
    public string SessionId { get; set; } = Guid.NewGuid().ToString();
    public string SessionName { get; set; } = $"{DateTime.Now:MMM dd, yyyy - HH:mm}";
    public Dictionary<string, int> PlayerScores { get; set; } = new();
    public string ?LastPlayer { get; set; }
    public int LastPoints { get; set; }
}

public static class SessionStorage
{
    const string StorageKey = "ScoreSessions";

    public static List<GameSession> LoadSessions()
    {
        var json = Preferences.Get(StorageKey, string.Empty);
        return string.IsNullOrWhiteSpace(json)
            ? new()
            : JsonSerializer.Deserialize<List<GameSession>>(json)!;
    }

    public static void SaveSessions(List<GameSession> sessions)
    {
        var json = JsonSerializer.Serialize(sessions);
        Preferences.Set(StorageKey, json);
    }
}

