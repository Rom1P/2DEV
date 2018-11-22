using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveScoreManager : MonoBehaviour {

    private GameScript GameScriptAccess;
    private string sequence;
    private int time;
    private int moves;

    private SequenceDataEditor sequenceScoreData;
    private ScoreDataEditor ScoreDataAccess;

    public GameObject ButtonSave;
    public GameObject ButtonMenu;

    public Text NameText;

    public GameDataEditor GameDataEditor
    {
        get
        {
            throw new System.NotImplementedException();
        }

        set
        {
        }
    }

    public ScoreDataEditor ScoreDataEditor
    {
        get
        {
            throw new System.NotImplementedException();
        }

        set
        {
        }
    }

    // Use this for initialization
    void Start () {
        try
        {

            GameObject GameManagerObject = GameObject.Find("GameManager");

            GameScriptAccess = (GameManagerObject.GetComponentInChildren<GameScript>());
            sequence = GameScriptAccess.sequence;

            Destroy(GameManagerObject);
        }

        catch
        {
            sequence = "Normal";
        }


        Button ButtonSaveComponent = ButtonSave.GetComponent<Button>();
        ButtonSaveComponent.onClick.AddListener(SaveData);

        Button ButtonMenuComponent = ButtonMenu.GetComponent<Button>();
        ButtonMenuComponent.onClick.AddListener(GoMenu);

        string DataSaveJson = File.ReadAllText("Assets\\Levels\\" + sequence + "\\sequenceData.json");

        sequenceScoreData = JsonUtility.FromJson<SequenceDataEditor>(DataSaveJson);

        time = sequenceScoreData.Time;
        moves = sequenceScoreData.Moves;

        ResetSequenceData();
        


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void SaveData()
    {
        string DataScoreJson = File.ReadAllText("Assets\\Data\\scores.json");

        ScoreDataAccess = JsonUtility.FromJson<ScoreDataEditor>(DataScoreJson);
        List<string> ListSequences = ScoreDataAccess.ListSequences;
        List<string> ListMoves = ScoreDataAccess.ListMoves;
        List<string> ListTimes = ScoreDataAccess.ListTimes;
        List<string> ListPlayers = ScoreDataAccess.ListPlayers;

        string NamePlayer = NameText.GetComponent<Text>().text;
        

        ListSequences.Add(sequence);
        ListMoves.Add(moves.ToString());
        ListTimes.Add(time.ToString());
        ListPlayers.Add(NamePlayer);

        ScoreDataAccess.ListSequences = ListSequences;
        ScoreDataAccess.ListMoves = ListMoves;
        ScoreDataAccess.ListTimes = ListTimes;
        ScoreDataAccess.ListPlayers = ListPlayers;

        string updateScores = JsonUtility.ToJson(ScoreDataAccess);
        
        File.WriteAllText("Assets\\Data\\scores.json", updateScores);
    }

    void ResetSequenceData()
    {

        List<string> ListLevelProgress = new List<string>();
        ListLevelProgress.Add(sequenceScoreData.ListLevels[0]);

        SequenceDataEditor ResetData = new SequenceDataEditor();
        ResetData.ListLevels = sequenceScoreData.ListLevels;
        ResetData.LevelProgress = ListLevelProgress;
        ResetData.Moves = 0;
        ResetData.Time = 0;


        string SaveProgressString = JsonUtility.ToJson(ResetData);

        File.WriteAllText("Assets\\Levels\\" + sequence + "\\sequenceData.json", SaveProgressString);

    }

    //Go back to menu
    void GoMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

}
