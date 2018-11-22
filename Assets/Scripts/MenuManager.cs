using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public GameObject StartButtonObject;

    public GameObject LevelButtonPrefab;

    public Text NameLevelObject;

    public GameObject GroupDisplay;
    public GameObject LeftArrow;
    public GameObject RightArrow;
    public GameObject MainMenuButton;


    public GameObject ToggleTimer;


    public Canvas CanvasButtonParent;
    public string NameFile;
    private List<string> ListLevels;
    private List<string> ListProgress;

    public bool ToggleTimerIsOn = false;

    private int indexPage = 1;

    public string sequence;

    private string pathFolder = "Assets\\Levels\\";

    private MainMenuManager MenuManagerAccess;

    SequenceDataEditor sequenceData;

    public GameScript GameScript
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

    void Start()
    {

        Button LeftArrowButtonComponent = LeftArrow.GetComponent<Button>();
        LeftArrowButtonComponent.onClick.AddListener(LeftArrowClicked);

        Button RightArrowButtonComponent = RightArrow.GetComponent<Button>();
        RightArrowButtonComponent.onClick.AddListener(RightArrowClicked);


        Button StartButtonComponent = StartButtonObject.GetComponent<Button>();
        StartButtonComponent.onClick.AddListener(OnClickStartButton);


        Button MainMenuButtonComponent = MainMenuButton.GetComponent<Button>();
        MainMenuButtonComponent.onClick.AddListener(GoBackToMainMenu);

        Toggle toggleComponent = ToggleTimer.GetComponent<Toggle>();
        toggleComponent.onValueChanged.AddListener(ToggleValueChanged);

        try
        {
            GameObject GameManagerObject = GameObject.Find("MainMenuManager");

            MenuManagerAccess = (GameManagerObject.GetComponentInChildren<MainMenuManager>());

            sequence = MenuManagerAccess.sequence;
            Destroy(GameManagerObject);
        }

        catch
        {
            sequence = "Normal";
        }

        pathFolder += sequence;

        LoadSequenceData();

        LoadAllFiles();
    }

    private void ToggleValueChanged(bool value)
    {
        ToggleTimerIsOn = value;
    }

    // Update is called once per frame
    void Update()
    {

    }
       
    void LoadSequenceData()
    {
        string DataJson = File.ReadAllText(pathFolder+"\\sequenceData.json");
        sequenceData = JsonUtility.FromJson<SequenceDataEditor>(DataJson);
    }
    
    //Get all levels in Json file
    void LoadAllFiles()
    {

        int indexLevel = 0;
        ListLevels = new List<string>();
        ListProgress = new List<string>();

        
        ListLevels = sequenceData.ListLevels;
        ListProgress = sequenceData.LevelProgress;

        if (ListLevels.Count > 10)
        {
            RightArrow.SetActive(true);
        }


        foreach (string LevelName in ListLevels)
        {


            GameObject ButtonLevel = Instantiate(LevelButtonPrefab, Vector3.zero, Quaternion.identity);

            ButtonLevel.transform.SetParent(CanvasButtonParent.transform);

            Vector3 buttonPostion = new Vector3();

            if (indexLevel % 2 == 0)
            {
                buttonPostion[0] = -300;
            }

            else
            {
                buttonPostion[0] = 300;
            }

            int positionY = (indexLevel / 2 * -150) + 300;

            buttonPostion[1] = positionY;

            buttonPostion[2] = 0;

            ButtonLevel.GetComponent<RectTransform>().localPosition = buttonPostion;
            ButtonLevel.GetComponentInChildren<Text>().text = LevelName;
            
            Button ButtonLevelComponent = ButtonLevel.GetComponent<Button>();



            if (ListProgress.Contains(LevelName))
            {
                ButtonLevelComponent.onClick.AddListener(OneLevelButtonClicked);
            }

            else
            {
                ButtonLevelComponent.interactable = false;
            }

            indexLevel++;

            if (indexLevel == 10)
            {
                indexLevel = 0;
                break;
            }
        }
        

    }

    void OneLevelButtonClicked()
    {
        NameFile = (EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text);

        if (!GroupDisplay.activeSelf)
        {
            GroupDisplay.SetActive(true);

            StartButtonObject.SetActive(true);
            Button StartButton = StartButtonObject.GetComponent<Button>();
            StartButton.onClick.AddListener(OnClickStartButton);
        }


        NameLevelObject.text = NameFile;
        

    }

    void OnClickStartButton()
    {
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }

    void LeftArrowClicked()
    {
        if (!RightArrow.activeSelf)
        {
            RightArrow.SetActive(true);
        }

        indexPage--;

        if (indexPage == 1)
        {
            LeftArrow.SetActive(false);
        }

        GameObject[] gameObjects;

        gameObjects = GameObject.FindGameObjectsWithTag("LevelButtonTag");

        for (var i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }

        List<string> ListFilesCopy = new List<string>();

        foreach (string fileName in ListLevels)
        {
            ListFilesCopy.Add(fileName);
        }

        ListFilesCopy.RemoveRange(0, 10 * (indexPage-1));
        

        int indexLevel = 0;

        foreach (string fileName in ListFilesCopy)
        {

            GameObject ButtonLevel = Instantiate(LevelButtonPrefab, Vector3.zero, Quaternion.identity);

            ButtonLevel.transform.SetParent(CanvasButtonParent.transform);

            Vector3 buttonPostion = new Vector3();

            if (indexLevel % 2 == 0)
            {
                buttonPostion[0] = -300;
            }

            else
            {
                buttonPostion[0] = 300;
            }

            int positionY = (indexLevel / 2 * -150) + 300;

            buttonPostion[1] = positionY;

            buttonPostion[2] = 0;

            ButtonLevel.GetComponent<RectTransform>().localPosition = buttonPostion;
            ButtonLevel.GetComponentInChildren<Text>().text = fileName;


            Button ButtonLevelComponent = ButtonLevel.GetComponent<Button>();
            ButtonLevelComponent.onClick.AddListener(OneLevelButtonClicked);

            indexLevel++;

            if (indexLevel == 10)
            {
                indexLevel = 0;
                break;
            }


        }



    }

    void RightArrowClicked()
    {
        if (!LeftArrow.activeSelf)
        {
            LeftArrow.SetActive(true);
        }

        GameObject[] gameObjects;

        gameObjects = GameObject.FindGameObjectsWithTag("LevelButtonTag");

        for (var i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }

        List<string> ListFilesCopy = new List<string>();

        foreach (string fileName in ListLevels)
        {
            ListFilesCopy.Add(fileName);
        }
        
        ListFilesCopy.RemoveRange(0, 10*indexPage);

        indexPage++;

        int indexLevel = 0;
        
        foreach (string fileName in ListFilesCopy)
        {
            
            GameObject ButtonLevel = Instantiate(LevelButtonPrefab, Vector3.zero, Quaternion.identity);

            ButtonLevel.transform.SetParent(CanvasButtonParent.transform);

            Vector3 buttonPostion = new Vector3();

            if (indexLevel % 2 == 0)
            {
                buttonPostion[0] = -300;
            }

            else
            {
                buttonPostion[0] = 300;
            }

            int positionY = (indexLevel / 2 * -150) + 300;

            buttonPostion[1] = positionY;

            buttonPostion[2] = 0;

            ButtonLevel.GetComponent<RectTransform>().localPosition = buttonPostion;
            ButtonLevel.GetComponentInChildren<Text>().text = fileName;


            Button ButtonLevelComponent = ButtonLevel.GetComponent<Button>();
            ButtonLevelComponent.onClick.AddListener(OneLevelButtonClicked);

            indexLevel++;

            if (indexLevel == 10)
            {
                indexLevel = 0;
                break;
            }


        }

        if (ListFilesCopy.Count <= 10)
        {
            RightArrow.SetActive(false);
        }

        
    }
    
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void GoNextAndSkip()
    {
        Debug.Log("potato");
    }

    void GoBackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
    

    
    

}
