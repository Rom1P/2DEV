using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreDisplayManager : MonoBehaviour {

    private ScoreDataEditor ScoreDataAccess;

    private List<string> ListSequences;
    private List<string> ListMoves;
    private List<string> ListTimes;
    private List<string> ListPlayers;

    public GameObject TextPrefab;
    public GameObject ButtonMenu;

    public Canvas CanvasTextParent;

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


        Button ButtonMenuComponent = ButtonMenu.GetComponent<Button>();
        ButtonMenuComponent.onClick.AddListener(GoMenu);

        string DataScoreJson = File.ReadAllText("Assets\\Data\\scores.json");

        ScoreDataAccess = JsonUtility.FromJson<ScoreDataEditor>(DataScoreJson);
        ListSequences = ScoreDataAccess.ListSequences;
        ListMoves = ScoreDataAccess.ListMoves;
        ListTimes = ScoreDataAccess.ListTimes;
        ListPlayers = ScoreDataAccess.ListPlayers;

        DisplayData();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void DisplayData()
    {
        int indexSequence = 0;
        foreach (string sequenceName in ListSequences)
        {

            GameObject TextSequence = Instantiate(TextPrefab, Vector3.zero, Quaternion.identity);

            TextSequence.transform.SetParent(CanvasTextParent.transform);

            TextSequence.GetComponent<Text>().text = sequenceName;

            TextSequence.transform.localPosition = new Vector3(-400, indexSequence * (-75) + 250, 0);

            indexSequence++;

            if (indexSequence == 10)
            {
                break;
            }
        }

        int indexMoves = 0;
        foreach (string moves in ListMoves)
        {

            GameObject TextSequence = Instantiate(TextPrefab, Vector3.zero, Quaternion.identity);

            TextSequence.transform.SetParent(CanvasTextParent.transform);

            TextSequence.GetComponent<Text>().text = moves + " Moves";

            TextSequence.transform.localPosition = new Vector3(-175, indexMoves * (-75) + 250, 0);

            indexMoves++;

            if (indexMoves == 10)
            {
                break;
            }
        }

        int indexTime = 0;
        foreach (string time in ListTimes)
        {
            int RealTime = Int32.Parse(time);
            
            int minutes = RealTime / 60;

            int secondes = RealTime - (60 * minutes);

            string TextToDisplay = minutes.ToString() + ":" + secondes.ToString();

            GameObject TextSequence = Instantiate(TextPrefab, Vector3.zero, Quaternion.identity);

            TextSequence.transform.SetParent(CanvasTextParent.transform);

            TextSequence.GetComponent<Text>().text = TextToDisplay;

            TextSequence.transform.localPosition = new Vector3(50, indexTime * (-75) + 250, 0);

            indexTime++;

            if (indexTime == 10)
            {
                break;
            }
        }

        int indexPlayer = 0;
        foreach (string player in ListPlayers)
        {

            GameObject TextSequence = Instantiate(TextPrefab, Vector3.zero, Quaternion.identity);

            TextSequence.transform.SetParent(CanvasTextParent.transform);

            TextSequence.GetComponent<Text>().text = player;

            TextSequence.transform.localPosition = new Vector3(400, indexPlayer * (-75) + 250, 0);

            indexPlayer++;

            if (indexPlayer == 10)
            {
                break;
            }
        }
    }

    //Go back to menu
    void GoMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
