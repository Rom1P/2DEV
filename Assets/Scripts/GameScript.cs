using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameScript : MonoBehaviour
{

    /*
     * Axis X = Lines; Axis Z = Columns;
     * 
     * 11111
       01010
       11121
       
       11111 = Line; 10 = Column
       2 : Line = 1(0) Column = 4(3)*/

    Vector3 TopRight;
    Vector3 BottomRight;
    Vector3 TopLeft;
    Vector3 BottomLeft;

    public GameObject centerPrefab;
    public GameObject CubeGame;


    public GameObject WoodCasePrefab;
    public GameObject BrickCasePrefab;

    public Text TextMoves;
    public Text TextNameLevel;

    private MenuManager MenuManagerAccess;

    private string NameFile;


    private string[] BoardList;

    GameObject BoardLoaderObject;

    private new Dictionary<string, List<GameObject>> NormalSwitches;
    private new Dictionary<string, List<GameObject>> StrongSwitches;

    int Moves;

    GameDataEditor loadedData;

    private bool withTimer;

    private float timeElapsed;

    private float timerGame;

    private bool inGame;

    public Text TimerText;

    public GameObject LooseButtons;
    public GameObject WinButtons;

    public string sequence;

    /* Buttons */

    public GameObject ButtonRetryLoose;
    public GameObject ButtonMenuLoose;
    public GameObject ButtonQuitLoose;
    public GameObject ButtonNextWin;
    public GameObject ButtonMenuWin;
    public GameObject ButtonQuitWin;

    private string CompletePath;

    SequenceDataEditor sequenceData;


    // Use this for initialization
    void Start()
    {


        try
        {
            GameObject GameManagerObject = GameObject.Find("MenuManager");

            MenuManagerAccess = (GameManagerObject.GetComponentInChildren<MenuManager>());

            NameFile = MenuManagerAccess.NameFile;

            withTimer = MenuManagerAccess.ToggleTimerIsOn;


            sequence = MenuManagerAccess.sequence;

            Destroy(GameManagerObject);
        }

        catch
        {

            NameFile = "Level1";
            sequence = "Normal";
            withTimer = false;
        }

        inGame = true;

        string DataJson = File.ReadAllText("Assets\\Levels\\" + sequence + "\\sequenceData.json");

        sequenceData = JsonUtility.FromJson<SequenceDataEditor>(DataJson);

        TextNameLevel.GetComponentInChildren<Text>().text = NameFile;



        CompletePath = "Assets\\Levels\\" + sequence + "\\" + NameFile + ".txt";

        string DataPath = "Assets\\Levels\\" + sequence + "\\" + NameFile + ".json";


        string readText = File.ReadAllText(CompletePath);

        BoardList = Regex.Split(readText, "\n");

        BoardLoaderObject = GameObject.Find("BoardLoader");
        BoardLoaderObject.SendMessage("LoadBoard", CompletePath);

        NormalSwitches = new Dictionary<string, List<GameObject>>();
        StrongSwitches = new Dictionary<string, List<GameObject>>();

        AddButtonsListener();



        string jsonString = File.ReadAllText(DataPath);

        loadedData = JsonUtility.FromJson<GameDataEditor>(jsonString);
        LoadData();

        timeElapsed = 0;

        if (withTimer)
        {
            UseTimer();
        }

        Moves = 0;


    }

    // Update is called once per frame
    void Update()
    {
        if (inGame)
        {
            timeElapsed += Time.deltaTime;

            int minutes = (int)timeElapsed / 60;

            int secondes = (int)timeElapsed - (60 * minutes);

            string TextToDisplay = minutes.ToString() + ":" + secondes.ToString();


            if (withTimer)
            {
                if (timeElapsed >= timerGame)
                {
                    LoosingProtocol();
                }


                int minutesTimer = (int)timerGame / 60;

                int secondesTimer = (int)timerGame - (60 * minutesTimer);

                TextToDisplay += " / " + minutesTimer.ToString() + ":" + secondesTimer.ToString();
            }

            try
            {
                TimerText.GetComponent<Text>().text = TextToDisplay;
            }

            catch
            {

            }

        }


    }

    void AddButtonsListener()
    {
        /* Retry */
        Button RetryButtonLoose = ButtonRetryLoose.GetComponent<Button>();
        RetryButtonLoose.onClick.AddListener(RestartLevel);

        /* Menu */
        Button MenuButtonLoose = ButtonMenuLoose.GetComponent<Button>();
        MenuButtonLoose.onClick.AddListener(GoMenu);

        /* Quit */
        Button QuitButtonLoose = ButtonQuitLoose.GetComponent<Button>();
        QuitButtonLoose.onClick.AddListener(QuitGame);

        /* Next */
        Button NextButtonWin = ButtonNextWin.GetComponent<Button>();
        NextButtonWin.onClick.AddListener(NextLevel);

        /* Menu */
        Button MenuButtonWin = ButtonMenuWin.GetComponent<Button>();
        MenuButtonWin.onClick.AddListener(GoMenu);

        /* Quit */
        Button QuitButtonWin = ButtonQuitWin.GetComponent<Button>();
        QuitButtonWin.onClick.AddListener(QuitGame);

    }

    void ReceiveAfterRotate()
    {

        Moves++;


        TextMoves.GetComponentInChildren<Text>().text = Moves.ToString();



        if ((int)CubeGame.transform.localEulerAngles[2] == 90 || (int)CubeGame.transform.localEulerAngles[2] == 270)
        {

            TopRight = new Vector3(CubeGame.transform.localPosition[0] + CubeGame.transform.localScale[0], 0, CubeGame.transform.localPosition[2] + CubeGame.transform.localScale[2] / 2);
            TopLeft = new Vector3(CubeGame.transform.localPosition[0] - CubeGame.transform.localScale[0], 0, CubeGame.transform.localPosition[2] + CubeGame.transform.localScale[2] / 2);
            BottomLeft = new Vector3(CubeGame.transform.localPosition[0] - CubeGame.transform.localScale[0], 0, CubeGame.transform.localPosition[2] - CubeGame.transform.localScale[2] / 2);
            BottomRight = new Vector3(CubeGame.transform.localPosition[0] + CubeGame.transform.localScale[0], 0, CubeGame.transform.localPosition[2] - CubeGame.transform.localScale[2] / 2);

            try
            {
                int CurrentLine = (int)BottomLeft[2] / (int)CubeGame.transform.localScale[2];
                int CurrentColumn = (int)BottomLeft[0] / (int)CubeGame.transform.localScale[0];


                string CaseValue1 = BoardList[CurrentLine][CurrentColumn].ToString();
                string CaseValue2 = BoardList[CurrentLine][CurrentColumn + 1].ToString();


                if ((CaseValue1 == "1" || CaseValue1 == "2" || CaseValue1 == "3" || CaseValue1 == "4" || CaseValue1 == "5" || CaseValue1 == "6") && (CaseValue2 == "1" || CaseValue2 == "2" || CaseValue2 == "3" || CaseValue2 == "4" || CaseValue2 == "5" || CaseValue2 == "6"))
                {
                    /* Bridge */
                    if (CaseValue1 == "5")
                    {
                        List<GameObject> CaseBridgeList = new List<GameObject>();


                        string keyList = (CurrentColumn + 1).ToString() + "," + (CurrentLine + 1).ToString();


                        CaseBridgeList = NormalSwitches[keyList];


                        for (var i = 0; i < CaseBridgeList.Count; i++)
                        {
                            if (!CaseBridgeList[i].activeSelf)
                            {
                                CaseBridgeList[i].SetActive(true);
                            }

                            else
                            {
                                CaseBridgeList[i].SetActive(false);
                            }

                        }
                    }


                    if (CaseValue2 == "5")
                    {
                        List<GameObject> CaseBridgeList = new List<GameObject>();


                        string keyList = (CurrentColumn + 2).ToString() + "," + (CurrentLine + 1).ToString();

                        CaseBridgeList = NormalSwitches[keyList];


                        for (var i = 0; i < CaseBridgeList.Count; i++)
                        {
                            if (!CaseBridgeList[i].activeSelf)
                            {
                                CaseBridgeList[i].SetActive(true);
                            }

                            else
                            {
                                CaseBridgeList[i].SetActive(false);
                            }

                        }
                    }
                }


                else
                {
                    LoosingProtocol();

                }
            }

            catch
            {
                LoosingProtocol();
            }





        }

        else if ((int)CubeGame.transform.localEulerAngles[0] == 90 || (int)CubeGame.transform.localEulerAngles[0] == 270)
        {
            /* Vertically Axis Z*/

            TopRight = new Vector3(CubeGame.transform.localPosition[0] + CubeGame.transform.localScale[0] / 2, 0, CubeGame.transform.localPosition[2] + CubeGame.transform.localScale[2]);
            TopLeft = new Vector3(CubeGame.transform.localPosition[0] - CubeGame.transform.localScale[0] / 2, 0, CubeGame.transform.localPosition[2] + CubeGame.transform.localScale[2]);
            BottomLeft = new Vector3(CubeGame.transform.localPosition[0] - CubeGame.transform.localScale[0] / 2, 0, CubeGame.transform.localPosition[2] - CubeGame.transform.localScale[2]);
            BottomRight = new Vector3(CubeGame.transform.localPosition[0] + CubeGame.transform.localScale[0] / 2, 0, CubeGame.transform.localPosition[2] - CubeGame.transform.localScale[2]);


            try
            {
                int CurrentLine = (int)BottomLeft[2] / (int)CubeGame.transform.localScale[2];
                int CurrentColumn = (int)BottomLeft[0] / (int)CubeGame.transform.localScale[0];


                string CaseValue1 = BoardList[CurrentLine][CurrentColumn].ToString();
                string CaseValue2 = BoardList[CurrentLine + 1][CurrentColumn].ToString();

                if ((CaseValue1 == "1" || CaseValue1 == "2" || CaseValue1 == "3" || CaseValue1 == "4" || CaseValue1 == "5" || CaseValue1 == "6") && (CaseValue2 == "1" || CaseValue2 == "2" || CaseValue2 == "3" || CaseValue2 == "4" || CaseValue2 == "5" || CaseValue2 == "6"))
                {
                    /* Bridge */
                    if (CaseValue1 == "5")
                    {
                        List<GameObject> CaseBridgeList = new List<GameObject>();


                        string keyList = (CurrentColumn + 1).ToString() + "," + (CurrentLine + 1).ToString();


                        CaseBridgeList = NormalSwitches[keyList];


                        for (var i = 0; i < CaseBridgeList.Count; i++)
                        {
                            if (!CaseBridgeList[i].activeSelf)
                            {
                                CaseBridgeList[i].SetActive(true);
                            }

                            else
                            {
                                CaseBridgeList[i].SetActive(false);
                            }

                        }
                    }


                    if (CaseValue2 == "5")
                    {
                        List<GameObject> CaseBridgeList = new List<GameObject>();


                        string keyList = (CurrentColumn + 1).ToString() + "," + (CurrentLine + 2).ToString();


                        CaseBridgeList = NormalSwitches[keyList];


                        for (var i = 0; i < CaseBridgeList.Count; i++)
                        {
                            if (!CaseBridgeList[i].activeSelf)
                            {
                                CaseBridgeList[i].SetActive(true);
                            }

                            else
                            {
                                CaseBridgeList[i].SetActive(false);
                            }

                        }
                    }
                }

                else
                {
                    LoosingProtocol();
                }

            }

            catch
            {
                LoosingProtocol();
            }



        }

        else
        {

            TopRight = new Vector3(CubeGame.transform.localPosition[0] + CubeGame.transform.localScale[0] / 2, 0, CubeGame.transform.localPosition[2] + CubeGame.transform.localScale[2] / 2);
            TopLeft = new Vector3(CubeGame.transform.localPosition[0] - CubeGame.transform.localScale[0] / 2, 0, CubeGame.transform.localPosition[2] + CubeGame.transform.localScale[2] / 2);
            BottomLeft = new Vector3(CubeGame.transform.localPosition[0] - CubeGame.transform.localScale[0] / 2, 0, CubeGame.transform.localPosition[2] - CubeGame.transform.localScale[2] / 2);
            BottomRight = new Vector3(CubeGame.transform.localPosition[0] + CubeGame.transform.localScale[0] / 2, 0, CubeGame.transform.localPosition[2] - CubeGame.transform.localScale[2] / 2);


            try
            {
                int CurrentLine = (int)BottomLeft[2] / (int)CubeGame.transform.localScale[2];
                int CurrentColumn = (int)BottomLeft[0] / (int)CubeGame.transform.localScale[0];

                string CaseValue = BoardList[CurrentLine][CurrentColumn].ToString();


                if (CaseValue == "1" || CaseValue == "3")
                {

                }

                if (CaseValue == "9")
                {
                    WinProtocol();
                }

                if (CaseValue == "0")
                {
                    LoosingProtocol();
                }

                if (CaseValue == "2" || CaseValue == "4")
                {
                    /* Wood Case*/


                    LoosingProtocol();
                }

                if (CaseValue == "5")
                {
                    List<GameObject> CaseBridgeList = new List<GameObject>();


                    string keyList = (CurrentColumn + 1).ToString() + "," + (CurrentLine + 1).ToString();


                    CaseBridgeList = NormalSwitches[keyList];


                    for (var i = 0; i < CaseBridgeList.Count; i++)
                    {
                        if (!CaseBridgeList[i].activeSelf)
                        {
                            CaseBridgeList[i].SetActive(true);
                        }

                        else
                        {
                            CaseBridgeList[i].SetActive(false);
                        }

                    }

                }

                if (CaseValue == "6")
                {
                    List<GameObject> CaseBridgeList = new List<GameObject>();


                    string keyList = (CurrentColumn + 1).ToString() + "," + (CurrentLine + 1).ToString();


                    CaseBridgeList = StrongSwitches[keyList];


                    for (var i = 0; i < CaseBridgeList.Count; i++)
                    {
                        if (!CaseBridgeList[i].activeSelf)
                        {
                            CaseBridgeList[i].SetActive(true);
                        }

                        else
                        {
                            CaseBridgeList[i].SetActive(false);
                        }

                    }

                }
            }

            catch
            {
                LoosingProtocol();
            }



        }


        /*
         * Use To debug and mark points with a Sphere GameObject
         * 
         * GameObject TopRightGO =  Instantiate(centerPrefab, TopRight, CubeGame.transform.rotation);
        GameObject TopLeftGO =  Instantiate(centerPrefab, TopLeft, CubeGame.transform.rotation);
        GameObject BottomLeftGO =  Instantiate(centerPrefab, BottomLeft, CubeGame.transform.rotation);
        GameObject BottomRightGO =  Instantiate(centerPrefab, BottomRight, CubeGame.transform.rotation);*/

    }

    void LoadData()
    {

        /* Load Switches And Bridges */


        SetSpawnPoint();

        foreach (string DataBridge in loadedData.ListBridges)
        {
            bool SwitchToLoadNormal = true;
            bool BridgeOpen = false;

            List<GameObject> CaseBridgeTemp = new List<GameObject>();

            int indexXSwitch;
            int indexZSwitch;

            if (DataBridge[0].ToString() == "S")
            {
                SwitchToLoadNormal = false;
            }

            else if (DataBridge[0].ToString() == "N")
            {
                SwitchToLoadNormal = true;
            }

            if (DataBridge[1].ToString() == "0")
            {
                BridgeOpen = false;
            }

            else if (DataBridge[1].ToString() == "1")
            {
                BridgeOpen = true;
            }

            string[] SplitsData = DataBridge.Split('-');

            string Coord = SplitsData[0];
            int indexTemp = Coord.IndexOf("(") + 1;
            string RawCoord = Coord.Substring(indexTemp, Coord.Length - indexTemp - 1);


            string[] SplitsCoord = RawCoord.Split(',');

            indexXSwitch = Int32.Parse(SplitsCoord[0]);
            indexZSwitch = Int32.Parse(SplitsCoord[1]);


            string ListCaseBridge = SplitsData[1];

            ListCaseBridge = ListCaseBridge.Substring(1, ListCaseBridge.Length - 2);


            string[] SplitsCoordData = ListCaseBridge.Split(';');

            int indexXCase;
            int indexZCase;

            foreach (string dataCoord in SplitsCoordData)
            {

                int indexTemp2 = dataCoord.IndexOf("(") + 1;



                string RawCaseCoord = dataCoord.Substring(indexTemp2, dataCoord.Length - indexTemp2 - 1);
                string[] SplitCoordCase = RawCaseCoord.Split(',');



                indexXCase = Int32.Parse(SplitCoordCase[0]);
                indexZCase = Int32.Parse(SplitCoordCase[1]);



                float ScaleY = WoodCasePrefab.transform.localScale[1];
                float IndexY = -(ScaleY / 2);


                float postionX = (indexXCase * WoodCasePrefab.transform.localScale[0]) - (WoodCasePrefab.transform.localScale[0] / 2);
                float postionZ = (indexZCase * WoodCasePrefab.transform.localScale[2]) - (WoodCasePrefab.transform.localScale[2] / 2);

                GameObject TempCase;

                if (BoardList[indexZCase - 1][indexXCase - 1].ToString() == "3")
                {
                    TempCase = Instantiate(BrickCasePrefab, new Vector3(postionX, IndexY, postionZ), transform.rotation);
                    CaseBridgeTemp.Add(TempCase);
                    if (!BridgeOpen)
                    {
                        TempCase.SetActive(false);
                    }
                }

                else if (BoardList[indexZCase - 1][indexXCase - 1].ToString() == "4")
                {
                    TempCase = Instantiate(WoodCasePrefab, new Vector3(postionX, IndexY, postionZ), transform.rotation);
                    CaseBridgeTemp.Add(TempCase);
                    if (!BridgeOpen)
                    {
                        TempCase.SetActive(false);
                    }
                }




            }

            string CoordSwitchForList = indexXSwitch.ToString() + "," + indexZSwitch.ToString();

            try
            {
                if (SwitchToLoadNormal)
                {
                    NormalSwitches.Add(CoordSwitchForList, CaseBridgeTemp);
                }

                else
                {
                    StrongSwitches.Add(CoordSwitchForList, CaseBridgeTemp);
                }
            }

            catch
            {

            }






        }

        /* Load Teleporters */
    }

    void LoosingProtocol()
    {
        Debug.Log("U loose");
        inGame = false;
        CubeGame.SendMessage("Loose");

        LooseButtons.SetActive(true);

        Collider CubeCollider = CubeGame.GetComponent<Collider>();
        CubeCollider.isTrigger = false;
        CubeCollider.attachedRigidbody.useGravity = true;

    }

    void WinProtocol()
    {

        inGame = false;
        List<String> CurrentProgress = sequenceData.LevelProgress;

        SequenceDataEditor SaveProgress = new SequenceDataEditor();
        SaveProgress.ListLevels = sequenceData.ListLevels;
        SaveProgress.LevelProgress = CurrentProgress;
        SaveProgress.Moves = sequenceData.Moves + Moves;
        SaveProgress.Time = sequenceData.Time + (int)timeElapsed;


        string SaveProgressString = JsonUtility.ToJson(SaveProgress);

        File.WriteAllText("Assets\\Levels\\" + sequence + "\\sequenceData.json", SaveProgressString);

        string DataReloadJson = File.ReadAllText("Assets\\Levels\\" + sequence + "\\sequenceData.json");

        sequenceData = JsonUtility.FromJson<SequenceDataEditor>(DataReloadJson);

        if (CurrentProgress.Count == sequenceData.ListLevels.Count)
        {
            SceneManager.LoadScene("SaveScore", LoadSceneMode.Single);
        }

        else
        {
            Debug.Log("U Win");
            CubeGame.SendMessage("Loose");

            WinButtons.SetActive(true);

            Collider CubeCollider = CubeGame.GetComponent<Collider>();
            CubeCollider.isTrigger = false;
            CubeCollider.attachedRigidbody.useGravity = true;
        }
    }

    void SetSpawnPoint()
    {
        float postionX;
        float postionZ;

        postionX = (loadedData.SpawnX * CubeGame.transform.localScale[0]) - (CubeGame.transform.localScale[0] / 2);
        postionZ = (loadedData.SpawnZ * CubeGame.transform.localScale[2]) - (CubeGame.transform.localScale[2] / 2);

        CubeGame.transform.localPosition = new Vector3(postionX, CubeGame.transform.localScale[1] / 2, postionZ);
        CubeGame.transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    void UseTimer()
    {
        timerGame = loadedData.timer;
    }

    void GoMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    void RestartLevel()
    {
        GameObject[] gameObjects;

        gameObjects = GameObject.FindGameObjectsWithTag("ToDestroy");

        for (var i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }


        inGame = true;

        NormalSwitches = new Dictionary<string, List<GameObject>>();
        StrongSwitches = new Dictionary<string, List<GameObject>>();

        LooseButtons.SetActive(false);

        Collider CubeCollider = CubeGame.GetComponent<Collider>();
        CubeCollider.isTrigger = true;
        CubeCollider.attachedRigidbody.useGravity = false;

        Rigidbody rigidbodyCube = CubeGame.GetComponent<Rigidbody>();

        rigidbodyCube.velocity = Vector3.zero;
        rigidbodyCube.angularVelocity = Vector3.zero;


        SetSpawnPoint();

        CubeGame.SendMessage("Restart");

        BoardLoaderObject.SendMessage("LoadBoard", CompletePath);

        LoadData();

        timeElapsed = 0;

        Moves = 0;

    }

    void QuitGame()
    {
        Application.Quit();
    }

    void NextLevel()
    {


        GameObject[] gameObjects;

        gameObjects = GameObject.FindGameObjectsWithTag("ToDestroy");

        for (var i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }

        inGame = true;


        CubeGame.SendMessage("Restart");

        WinButtons.SetActive(false);

        Collider CubeCollider = CubeGame.GetComponent<Collider>();
        CubeCollider.isTrigger = true;
        CubeCollider.attachedRigidbody.useGravity = false;

        Rigidbody rigidbodyCube = CubeGame.GetComponent<Rigidbody>();

        rigidbodyCube.velocity = Vector3.zero;
        rigidbodyCube.angularVelocity = Vector3.zero;



        List<String> CurrentProgress = sequenceData.LevelProgress;
        

        CurrentProgress.Add(sequenceData.ListLevels[CurrentProgress.Count]);


        SequenceDataEditor SaveProgress = new SequenceDataEditor();
        SaveProgress.ListLevels = sequenceData.ListLevels;
        SaveProgress.LevelProgress = CurrentProgress;
        SaveProgress.Moves = sequenceData.Moves;
        SaveProgress.Time = sequenceData.Time;


        string SaveProgressString = JsonUtility.ToJson(SaveProgress);

        File.WriteAllText("Assets\\Levels\\" + sequence + "\\sequenceData.json", SaveProgressString);

        NameFile = CurrentProgress[CurrentProgress.Count - 1];


        TextNameLevel.GetComponentInChildren<Text>().text = NameFile;



        CompletePath = "Assets\\Levels\\" + sequence + "\\" + NameFile + ".txt";

        string DataPath = "Assets\\Levels\\" + sequence + "\\" + NameFile + ".json";


        string readText = File.ReadAllText(CompletePath);

        BoardList = Regex.Split(readText, "\n");

        BoardLoaderObject = GameObject.Find("BoardLoader");
        BoardLoaderObject.SendMessage("LoadBoard", CompletePath);

        NormalSwitches = new Dictionary<string, List<GameObject>>();
        StrongSwitches = new Dictionary<string, List<GameObject>>();


        string DataJson2 = File.ReadAllText(DataPath);

        loadedData = JsonUtility.FromJson<GameDataEditor>(DataJson2);
        LoadData();


        string DataReloadJson = File.ReadAllText("Assets\\Levels\\" + sequence + "\\sequenceData.json");

        sequenceData = JsonUtility.FromJson<SequenceDataEditor>(DataReloadJson);

        timeElapsed = 0;

        if (withTimer)
        {
            UseTimer();
        }

        Moves = 0;



    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }



}
