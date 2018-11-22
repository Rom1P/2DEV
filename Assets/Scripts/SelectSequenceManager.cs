using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectSequenceManager : MonoBehaviour {

    public GameObject Custom1;
    public GameObject Custom2;
    public GameObject Custom3;
    public GameObject Clear1;
    public GameObject Clear2;
    public GameObject Clear3;

    public string sequenceSelected;

    public LevelEditorManager LevelEditorManager
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

        Button ButtonCustom1 = Custom1.GetComponent<Button>();
        ButtonCustom1.onClick.AddListener(Select1);

        Button ButtonCustom2 = Custom2.GetComponent<Button>();
        ButtonCustom2.onClick.AddListener(Select2);

        Button ButtonCustom3 = Custom3.GetComponent<Button>();
        ButtonCustom3.onClick.AddListener(Select3);

        Button Clear1Button = Clear1.GetComponent<Button>();
        Clear1Button.onClick.AddListener(ClearSequence1);

        Button Clear2Button = Clear2.GetComponent<Button>();
        Clear2Button.onClick.AddListener(ClearSequence2);

        Button Clear3Button = Clear3.GetComponent<Button>();
        Clear3Button.onClick.AddListener(ClearSequence3);

        GameObject GameManagerObject = GameObject.Find("MainMenuManager");
        Destroy(GameManagerObject);

    }

    // Update is called once per frame
    void Update () {
		
	}

    void Select1()
    {
        sequenceSelected = "CustomRun1";
        SceneManager.LoadScene("LevelEditor", LoadSceneMode.Single);
    }

    void Select2()
    {
        sequenceSelected = "CustomRun2";
        SceneManager.LoadScene("LevelEditor", LoadSceneMode.Single);
    }
    void Select3()
    {
        sequenceSelected = "CustomRun3";
        SceneManager.LoadScene("LevelEditor", LoadSceneMode.Single);
    }

    void ClearSequence1()
    {
        List<string> ListLevel = new List<string>();
        List<string> ListLevelProgress = new List<string>();

        SequenceDataEditor ResetData = new SequenceDataEditor();
        ResetData.ListLevels = ListLevel;
        ResetData.LevelProgress = ListLevelProgress;
        ResetData.Moves = 0;
        ResetData.Time = 0;


        string SaveProgressString = JsonUtility.ToJson(ResetData);

        File.WriteAllText("Assets\\Levels\\CustomRun1\\sequenceData.json", SaveProgressString);
    }
    void ClearSequence2()
    {
        List<string> ListLevel = new List<string>();
        List<string> ListLevelProgress = new List<string>();

        SequenceDataEditor ResetData = new SequenceDataEditor();
        ResetData.ListLevels = ListLevel;
        ResetData.LevelProgress = ListLevelProgress;
        ResetData.Moves = 0;
        ResetData.Time = 0;


        string SaveProgressString = JsonUtility.ToJson(ResetData);

        File.WriteAllText("Assets\\Levels\\CustomRun2\\sequenceData.json", SaveProgressString);
    }
    void ClearSequence3()
    {
        List<string> ListLevel = new List<string>();
        List<string> ListLevelProgress = new List<string>();

        SequenceDataEditor ResetData = new SequenceDataEditor();
        ResetData.ListLevels = ListLevel;
        ResetData.LevelProgress = ListLevelProgress;
        ResetData.Moves = 0;
        ResetData.Time = 0;


        string SaveProgressString = JsonUtility.ToJson(ResetData);

        File.WriteAllText("Assets\\Levels\\CustomRun3\\sequenceData.json", SaveProgressString);

    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }


}
