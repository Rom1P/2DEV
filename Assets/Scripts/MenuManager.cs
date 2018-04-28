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

    public GameObject ToggleTimer;

    public Canvas CanvasButtonParent;
    public string NameFile;
    private string[] files;

    public bool ToggleTimerIsOn = false;

    private int indexPage = 1;

    private string pathFolder = "Assets\\Levels";


    // Use this for initialization
    void Start()
    {
        LoadAllFiles();

        Button LeftArrowButtonComponent = LeftArrow.GetComponent<Button>();
        LeftArrowButtonComponent.onClick.AddListener(LeftArrowClicked);

        Button RightArrowButtonComponent = RightArrow.GetComponent<Button>();
        RightArrowButtonComponent.onClick.AddListener(RightArrowClicked);


        Button StartButtonComponent = StartButtonObject.GetComponent<Button>();
        StartButtonComponent.onClick.AddListener(OnClickStartButton);

        Toggle toggleComponent = ToggleTimer.GetComponent<Toggle>();
        toggleComponent.onValueChanged.AddListener(ToggleValueChanged);
    }

    private void ToggleValueChanged(bool value)
    {
        ToggleTimerIsOn = value;
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    void LoadAllFiles()
    {

        int indexLevel = 0;

        files = Directory.GetFiles(pathFolder, "*.txt");
        
        if (files.Length > 8)
        {
            RightArrow.SetActive(true);
        }


        foreach (string fileTxt in files)
        {


            string NameFile = fileTxt.Substring(pathFolder.Length + 1, fileTxt.Length - pathFolder.Length - 5);

            


            GameObject ButtonLevel = Instantiate(LevelButtonPrefab, Vector3.zero, Quaternion.identity);

            ButtonLevel.transform.SetParent(CanvasButtonParent.transform);

            Vector3 buttonPostion = new Vector3();

            if (indexLevel % 2 == 0)
            {
                buttonPostion[0] = 200;
            }

            else
            {
                buttonPostion[0] = -200;
            }

            int positionY = (indexLevel / 2 * -100) + 200;

            buttonPostion[1] = positionY;

            buttonPostion[2] = 0;

            ButtonLevel.GetComponent<RectTransform>().localPosition = buttonPostion;
            ButtonLevel.GetComponentInChildren<Text>().text = NameFile;


            Button ButtonLevelComponent = ButtonLevel.GetComponent<Button>();
            ButtonLevelComponent.onClick.AddListener(OneLevelButtonClicked);

            indexLevel++;

            if (indexLevel == 8)
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

        foreach (string fileName in files)
        {
            ListFilesCopy.Add(fileName);
        }

        ListFilesCopy.RemoveRange(0, 8 * (indexPage-1));
        

        int indexLevel = 0;

        foreach (string fileName in ListFilesCopy)
        {


            string NameFile = fileName.Substring(pathFolder.Length + 1, fileName.Length - pathFolder.Length - 5);




            GameObject ButtonLevel = Instantiate(LevelButtonPrefab, Vector3.zero, Quaternion.identity);

            ButtonLevel.transform.SetParent(CanvasButtonParent.transform);

            Vector3 buttonPostion = new Vector3();

            if (indexLevel % 2 == 0)
            {
                buttonPostion[0] = -200;
            }

            else
            {
                buttonPostion[0] = 200;
            }

            int positionY = (indexLevel / 2 * -100) + 200;

            buttonPostion[1] = positionY;

            buttonPostion[2] = 0;

            ButtonLevel.GetComponent<RectTransform>().localPosition = buttonPostion;
            ButtonLevel.GetComponentInChildren<Text>().text = NameFile;


            Button ButtonLevelComponent = ButtonLevel.GetComponent<Button>();
            ButtonLevelComponent.onClick.AddListener(OneLevelButtonClicked);

            indexLevel++;

            if (indexLevel == 8)
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

        foreach (string fileName in files)
        {
            ListFilesCopy.Add(fileName);
        }
        
        ListFilesCopy.RemoveRange(0, 8*indexPage);

        indexPage++;

        int indexLevel = 0;
        
        foreach (string fileName in ListFilesCopy)
        {


            string NameFile = fileName.Substring(pathFolder.Length + 1, fileName.Length - pathFolder.Length - 5);




            GameObject ButtonLevel = Instantiate(LevelButtonPrefab, Vector3.zero, Quaternion.identity);

            ButtonLevel.transform.SetParent(CanvasButtonParent.transform);

            Vector3 buttonPostion = new Vector3();

            if (indexLevel % 2 == 0)
            {
                buttonPostion[0] = -200;
            }

            else
            {
                buttonPostion[0] = 200;
            }

            int positionY = (indexLevel / 2 * -100) + 200;

            buttonPostion[1] = positionY;

            buttonPostion[2] = 0;

            ButtonLevel.GetComponent<RectTransform>().localPosition = buttonPostion;
            ButtonLevel.GetComponentInChildren<Text>().text = NameFile;


            Button ButtonLevelComponent = ButtonLevel.GetComponent<Button>();
            ButtonLevelComponent.onClick.AddListener(OneLevelButtonClicked);

            indexLevel++;

            if (indexLevel == 8)
            {
                indexLevel = 0;
                break;
            }


        }

        if (ListFilesCopy.Count <= 8)
        {
            RightArrow.SetActive(false);
        }

        
    }
    
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    

}
