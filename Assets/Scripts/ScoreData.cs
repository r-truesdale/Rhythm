[System.Serializable]
public class ScoreData
{
    public string levelName;
    public string gameMode;
    public int earlyScore;
    public int earlyMissScore;
    public int perfectScore;
    public int lateScore;
    public int lateMissScore;
    public int totalScore;
    public string timestamp;

    public ScoreData(string levelName, string gameMode, int earlyScore, int earlyMissScore, int perfectScore, int lateScore, int lateMissScore, int totalScore, string timestamp)
    {
        this.levelName = levelName;
        this.gameMode = gameMode;
        this.earlyScore = earlyScore;
        this.earlyMissScore = earlyMissScore;
        this.perfectScore = perfectScore;
        this.lateScore = lateScore;
        this.lateMissScore = lateMissScore;
        this.totalScore = totalScore;
        this.timestamp = timestamp;
    }
}
