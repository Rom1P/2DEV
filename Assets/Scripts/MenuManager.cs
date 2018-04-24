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

    public Canvas CanvasButtonParent;
    public string NameFile;

    // Use this for initialization
    void Start()
    {
        LoadAllFiles();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    void LoadAllFiles()
    {

        int indexLevel = 1;

        string pathFolder = "Assets\\Levels";

        string[] files = Directory.GetFiles(pathFolder, "*.txt");


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

            int positionY = (indexLevel / 2 * 100) + 200;

            buttonPostion[1] = positionY;

            buttonPostion[2] = 0;

            ButtonLevel.GetComponent<RectTransform>().localPosition = buttonPostion;
            ButtonLevel.GetComponentInChildren<Text>().text = NameFile;


            Button ButtonLevelComponent = ButtonLevel.GetComponent<Button>();
            ButtonLevelComponent.onClick.AddListener(OneLevelButtonClicked);

            indexLevel++;


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
        Debug.Log("Click on Start Button");
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);

        Debug.Log("New Scene Loaded");
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

}
