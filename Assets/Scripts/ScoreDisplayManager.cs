using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScoreDisplayManager : MonoBehaviour {

    private ScoreDataEditor ScoreDataAccess;
    // Use this for initialization
    void Start () {

        string DataScoreJson = File.ReadAllText("Assets\\Data\\scores.json");

        ScoreDataAccess = JsonUtility.FromJson<ScoreDataEditor>(DataScoreJson);
        List<string> ListSequences = ScoreDataAccess.ListSequences;
        List<string> ListMoves = ScoreDataAccess.ListMoves;
        List<string> ListTimes = ScoreDataAccess.ListTimes;
        List<string> ListPlayers = ScoreDataAccess.ListPlayers;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
