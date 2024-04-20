using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;

[System.Serializable]
public class AllSongs
{

    public List<SongBlueprint> songs;

}

[System.Serializable]
public class SongBlueprint
{
    public string name;
    public List<float> midi_score_beats;
    public List<float> midi_score_downbeats;
    public List<float> midi_offbeats;
    public List<float> easy;
    public List<float> medium;
    public List<float> hard;
}

