using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreData
{
    // Start is called before the first frame update
    public string levelName;
    public string gameMode;
    public int earlyScore;
    public int earlyMissScore;
    public int perfectScore;
    public int lateScore;
    public int lateMissScore;
    public string timestamp;

    public ScoreData(string levelName, string gameMode, int earlyScore, int earlyMissScore, int perfectScore, int lateScore, int lateMissScore, string timestamp){
        this.levelName = levelName;
        this.gameMode = gameMode;
        this.earlyScore = earlyScore;
        this.earlyMissScore = earlyMissScore;
        this.perfectScore = perfectScore;
        this.lateScore = lateScore;
        this.lateMissScore = lateMissScore;
        this.timestamp = timestamp;
    }
}
