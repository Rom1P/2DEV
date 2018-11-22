using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    private string pathFolder = "Assets\\Levels";
    public string sequence;
    private string[] folders;

    public GameObject LevelButtonPrefab;
    public GameObject PlayButton;
    public GameObject CanvasButtonParent;
    public GameObject LevelEditorButton;
    public GameObject ScoresButton;

    public SelectSequenceManager SelectSequenceManager
    {
        get
        {
            throw new System.NotImplementedException();
        }

        set
        {
        }
    }

    public ScoreDisplayManager ScoreDisplayManager
    {
        get
        {
            throw new System.NotImplementedException();
        }

        set
        {
        }
    }

    public MenuManager MenuManager
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
        LoadAllSequences();

        Button LevelEditorButtonComponent = LevelEditorButton.GetComponent<Button>();
        LevelEditorButtonComponent.onClick.AddListener(LevelEditorButtonClicked);

        Button PlayButtonComponent = PlayButton.GetComponent<Button>();
        PlayButtonComponent.onClick.AddListener(PlayButtonClicked);

        Button ScoresButtonComponent = ScoresButton.GetComponent<Button>();
        ScoresButtonComponent.onClick.AddListener(ButtonScoresClicked);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //Load all folders
    void LoadAllSequences()
    {
        folders = Directory.GetDirectories(pathFolder);

        int indexLevel = 0;

        foreach (string nameFolder in folders)
        {
            GameObject ButtonLevel = Instantiate(LevelButtonPrefab, Vector3.zero, Quaternion.identity);

            ButtonLevel.transform.SetParent(CanvasButtonParent.transform);

            Vector3 buttonPostion = new Vector3();

            

            int positionY = indexLevel * -150 + 250;

            buttonPostion[1] = positionY;

            buttonPostion[2] = 0;
            

            string NameFile = nameFolder.Substring(pathFolder.Length + 1, nameFolder.Length - pathFolder.Length-1);
            

            ButtonLevel.GetComponent<RectTransform>().localPosition = buttonPostion;
            ButtonLevel.GetComponentInChildren<Text>().text = NameFile;


            Button ButtonLevelComponent = ButtonLevel.GetComponent<Button>();
            ButtonLevelComponent.onClick.AddListener(SequenceButtonClicked);

            indexLevel++;
        }

    }

    
    void SequenceButtonClicked()
    {
        sequence = (EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text);

        if (!PlayButton.activeSelf)
        {
            PlayButton.SetActive(true);
        }
    }

    void LevelEditorButtonClicked()
    {
        SceneManager.LoadScene("SelectSequence", LoadSceneMode.Single);
    }

    void PlayButtonClicked()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);        
    }

    void ButtonScoresClicked()
    {
        SceneManager.LoadScene("Scores", LoadSceneMode.Single);
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
