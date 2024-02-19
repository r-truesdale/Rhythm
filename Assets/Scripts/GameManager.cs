using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using MidiPlayerTK;
using TMPro;
public class GameManager : MonoBehaviour
{
    public MidiFilePlayer midiFilePlayer;
    public GameObject arrowPrefab;
    public JArray time;
    public int selectedSongIndex;
    public TMP_Text beatNum;
    private bool[] arrowsSpawned;

    void Start()
    {
        selectedSongIndex = PlayerPrefs.GetInt("selectedSongIndex", 0);
        time = songData.GetMidiScoreBeats(selectedSongIndex);
        Debug.Log(time);
        arrowsSpawned = new bool[time.Count];
    }
    void Update()
    {
        var currentTime = (float)midiFilePlayer.MPTK_PlayTime.TotalSeconds;
        // Spawn arrows based on beat timings
        for (int i = 0; i < time.Count; i++)
        {
            float spawnTime = time[i].Value<float>();
            float timeRange = 0.01f;

            if (!arrowsSpawned[i] && Mathf.Abs(currentTime - spawnTime) < timeRange)
            {
                SpawnArrow();
                beatNum.text = time[i].ToString();
                Debug.Log(time[i]);
                // Mark the arrow as spawned for this beat
                arrowsSpawned[i] = true;
            }
        }
    }
    private void SpawnArrow()
    {
        GameObject arrow = Instantiate(arrowPrefab, new Vector3(Random.Range(-120, 100), 50, -10), Quaternion.identity);
    }
}
