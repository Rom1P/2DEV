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


    //First setup our differents gameobjects we need

    //Buttons

    public GameObject ButtonRetryLoose;
    public GameObject ButtonMenuLoose;
    public GameObject ButtonQuitLoose;
    public GameObject ButtonNextWin;
    public GameObject ButtonMenuWin;
    public GameObject ButtonQuitWin;

    public GameObject ButtonMenu;

    public GameObject LooseButtons;
    public GameObject WinButtons;

    public GameObject centerPrefab;
    public GameObject CubeGame;

    public GameObject WoodCasePrefab;
    public GameObject BrickCasePrefab;

    public GameObject BoardLoaderObject;

    //Json Classes

    GameDataEditor loadedData;
    SequenceDataEditor sequenceData;

    //Access To previous scenes

    private MenuManager MenuManagerAccess;

    //UI Text
    
    public Text TextMoves;
    public Text TextNameLevel;
    public Text TimerText;

    public Vector3 TopRight;
    public Vector3 BottomRight;
    public Vector3 TopLeft;
    public Vector3 BottomLeft;

    //Var that contains important elements to launch the level

    private bool withTimer;

    private float timeElapsed;

    private float timerGame;

    private bool inGame;

    private string NameFile;

    public string sequence;

    private string CompletePath;

    public int Moves;

    //Data of level

    private string[] BoardListImport;
    private List<string> BoardList = new List<string>();


    private Dictionary<string, List<GameObject>> NormalSwitches;
    private Dictionary<string, List<GameObject>> StrongSwitches;

    private List<string> CurrentlyActivatedCasesBridge;

    private string arrivalTeleport;

    public CubeManager CubeManager
    {
        get
        {
            throw new System.NotImplementedException();
        }

        set
        {
        }
    }

    public CreateBoard CreateBoard
    {
        get
        {
            throw new System.NotImplementedException();
        }

        set
        {
        }
    }

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

    public SaveScoreManager SaveScoreManager
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

        //Get name of level to load by accessing other scenes
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

        //Load sequence data in json


        string DataJson = File.ReadAllText("Assets\\Levels\\" + sequence + "\\sequenceData.json");

        sequenceData = JsonUtility.FromJson<SequenceDataEditor>(DataJson);



        //Get paths
        
        TextNameLevel.GetComponentInChildren<Text>().text = NameFile;


        CompletePath = "Assets\\Levels\\" + sequence + "\\" + NameFile + ".txt";

        string DataPath = "Assets\\Levels\\" + sequence + "\\" + NameFile + ".json";


        //Load Board as a list

        string readText = File.ReadAllText(CompletePath);

        BoardListImport = Regex.Split(readText, "\n");

        //Need to clear list in case of restarting

        BoardList.Clear();

        foreach (string TempString in BoardListImport)
        {
            if (TempString.Length > 1)
            {
                BoardList.Add(TempString);
            }
        }

        //Tell to BoardLoader to load the board

        BoardLoaderObject = GameObject.Find("BoardLoader");
        BoardLoaderObject.SendMessage("LoadBoard", CompletePath);

        //Create dictionnary that contains case of switches as key and list of GameObject to toggle as values

        NormalSwitches = new Dictionary<string, List<GameObject>>();
        StrongSwitches = new Dictionary<string, List<GameObject>>();

        //Usefull when the cube is on a bridge case to know if that case is on or off

        CurrentlyActivatedCasesBridge = new List<string>();

        //Load JsonData of the level

        string jsonString = File.ReadAllText(DataPath);

        loadedData = JsonUtility.FromJson<GameDataEditor>(jsonString);
        LoadData();

        //Add UI

        AddButtonsListener();

        //We start the game so turn bool true and set parameters of the level

        inGame = true;

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


    //Setup UI
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


        Button MenuButton = ButtonMenu.GetComponent<Button>();
        MenuButton.onClick.AddListener(GoMenu);

    }


    //When Move has been done we check current rotation of cube and then get values of case it is on
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

                string Coordinate1 = "(" + (CurrentColumn + 1).ToString() + "," + (CurrentLine + 1).ToString() + ")";
                string Coordinate2 = "(" + (CurrentColumn + 2).ToString() + "," + (CurrentLine + 1).ToString() + ")";



                string CaseValue1 = BoardList[CurrentLine][CurrentColumn].ToString();
                string CaseValue2 = BoardList[CurrentLine][CurrentColumn + 1].ToString();


                if ((CaseValue1 == "1" || CaseValue1 == "2" || CaseValue1 == "3" || CaseValue1 == "4" || CaseValue1 == "5" || CaseValue1 == "6" || CaseValue1 == "7" || CaseValue1 == "8" || CaseValue1 == "9") && (CaseValue2 == "1" || CaseValue2 == "2" || CaseValue2 == "3" || CaseValue2 == "4" || CaseValue2 == "5" || CaseValue2 == "6" || CaseValue2 == "7" || CaseValue1 == "8" || CaseValue2 == "9"))
                {
                    /* Bridge */
                    if (CaseValue1 == "5")
                    {
                        List<GameObject> CaseBridgeList = new List<GameObject>();


                        string keyList = (CurrentColumn + 1).ToString() + "," + (CurrentLine + 1).ToString();


                        CaseBridgeList = NormalSwitches[keyList];


                        for (var i = 0; i < CaseBridgeList.Count; i++)
                        {
                            int tempPosX = (int)CaseBridgeList[i].transform.position[0] - 5;
                            int tempPosZ = (int)CaseBridgeList[i].transform.position[2] - 5;

                            int indexXTemp = tempPosX / 10;
                            int indexZTemp = tempPosZ / 10;

                            string CoordinateTemp = "(" + (indexXTemp + 1).ToString() + "," + (indexZTemp + 1).ToString() + ")";

                            if (!CaseBridgeList[i].activeSelf)
                            {
                                CaseBridgeList[i].SetActive(true);
                                CurrentlyActivatedCasesBridge.Add(CoordinateTemp);
                            }

                            else
                            {
                                CaseBridgeList[i].SetActive(false);
                                CurrentlyActivatedCasesBridge.Remove(CoordinateTemp);
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

                            int tempPosX = (int)CaseBridgeList[i].transform.position[0] - 5;
                            int tempPosZ = (int)CaseBridgeList[i].transform.position[2] - 5;

                            int indexXTemp = tempPosX / 10;
                            int indexZTemp = tempPosZ / 10;

                            string CoordinateTemp = "(" + (indexXTemp + 1).ToString() + "," + (indexZTemp + 1).ToString() + ")";

                            if (!CaseBridgeList[i].activeSelf)
                            {
                                CaseBridgeList[i].SetActive(true);
                                CurrentlyActivatedCasesBridge.Add(CoordinateTemp);
                            }

                            else
                            {
                                CaseBridgeList[i].SetActive(false);
                                CurrentlyActivatedCasesBridge.Remove(CoordinateTemp);
                            }

                        }
                    }

                    if ((CaseValue1 == "3" || CaseValue1 == "4") && !CurrentlyActivatedCasesBridge.Contains(Coordinate1))
                    {
                        LoosingProtocol();
                    }

                    if ((CaseValue2 == "3" || CaseValue2 == "4") && !CurrentlyActivatedCasesBridge.Contains(Coordinate2))
                    {
                        
                        LoosingProtocol();
                    }
                }

                
                else
                {
                    LoosingProtocol();
                }
            }

            catch (Exception e)
            {
                print(e);
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


                string Coordinate1 = "(" + (CurrentColumn + 1).ToString() + "," + (CurrentLine + 1).ToString() + ")";
                string Coordinate2 = "(" + (CurrentColumn + 1).ToString() + "," + (CurrentLine+2).ToString() + ")";

                if ((CaseValue1 == "1" || CaseValue1 == "2" || CaseValue1 == "3" || CaseValue1 == "4" || CaseValue1 == "5" || CaseValue1 == "6" || CaseValue1 == "7" || CaseValue1 == "8" || CaseValue1 == "9") && (CaseValue2 == "1" || CaseValue2 == "2" || CaseValue2 == "3" || CaseValue2 == "4" || CaseValue2 == "5" || CaseValue2 == "6" || CaseValue2 == "7" || CaseValue1 == "8" || CaseValue2 == "9"))
                {
                    /* Bridge */
                    if (CaseValue1 == "5")
                    {
                        List<GameObject> CaseBridgeList = new List<GameObject>();


                        string keyList = (CurrentColumn + 1).ToString() + "," + (CurrentLine + 1).ToString();


                        CaseBridgeList = NormalSwitches[keyList];


                        for (var i = 0; i < CaseBridgeList.Count; i++)
                        {
                            int tempPosX = (int)CaseBridgeList[i].transform.position[0] - 5;
                            int tempPosZ = (int)CaseBridgeList[i].transform.position[2] - 5;

                            int indexXTemp = tempPosX / 10;
                            int indexZTemp = tempPosZ / 10;

                            string CoordinateTemp = "(" + (indexXTemp + 1).ToString() + "," + (indexZTemp + 1).ToString() + ")";

                            if (!CaseBridgeList[i].activeSelf)
                            {
                                CaseBridgeList[i].SetActive(true);
                                CurrentlyActivatedCasesBridge.Add(CoordinateTemp);
                            }

                            else
                            {
                                CaseBridgeList[i].SetActive(false);
                                CurrentlyActivatedCasesBridge.Remove(CoordinateTemp);
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
                            int tempPosX = (int)CaseBridgeList[i].transform.position[0] - 5;
                            int tempPosZ = (int)CaseBridgeList[i].transform.position[2] - 5;

                            int indexXTemp = tempPosX / 10;
                            int indexZTemp = tempPosZ / 10;

                            string CoordinateTemp = "(" + (indexXTemp + 1).ToString() + "," + (indexZTemp + 1).ToString() + ")";

                            if (!CaseBridgeList[i].activeSelf)
                            {
                                CaseBridgeList[i].SetActive(true);
                                CurrentlyActivatedCasesBridge.Add(CoordinateTemp);
                            }

                            else
                            {
                                CaseBridgeList[i].SetActive(false);
                                CurrentlyActivatedCasesBridge.Remove(CoordinateTemp);
                            }

                        }
                    }

                    if ((CaseValue1 == "3" || CaseValue1 == "4" ) && !CurrentlyActivatedCasesBridge.Contains(Coordinate1))
                    {
                        LoosingProtocol();
                    }

                    if ((CaseValue2 == "3" || CaseValue2 == "4") && !CurrentlyActivatedCasesBridge.Contains(Coordinate2))
                    {
                        LoosingProtocol();
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


                string Coordinate = "(" + (CurrentColumn + 1).ToString() + "," + (CurrentLine + 1).ToString() + ")";

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

                if (CaseValue == "3" && !CurrentlyActivatedCasesBridge.Contains(Coordinate))
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
                        int tempPosX = (int)CaseBridgeList[i].transform.position[0] - 5;
                        int tempPosZ = (int)CaseBridgeList[i].transform.position[2] - 5;

                        int indexXTemp = tempPosX / 10;
                        int indexZTemp = tempPosZ / 10;

                        string CoordinateTemp = "(" + (indexXTemp + 1).ToString() + "," + (indexZTemp + 1).ToString() + ")";

                        if (!CaseBridgeList[i].activeSelf)
                        {
                            CaseBridgeList[i].SetActive(true);
                            CurrentlyActivatedCasesBridge.Add(CoordinateTemp);
                        }

                        else
                        {
                            CaseBridgeList[i].SetActive(false);
                            CurrentlyActivatedCasesBridge.Remove(CoordinateTemp);
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
                        int tempPosX = (int)CaseBridgeList[i].transform.position[0] - 5;
                        int tempPosZ = (int)CaseBridgeList[i].transform.position[2] - 5;

                        int indexXTemp = tempPosX / 10;
                        int indexZTemp = tempPosZ / 10;

                        string CoordinateTemp = "(" + (indexXTemp + 1).ToString() + "," + (indexZTemp + 1).ToString() + ")";

                        if (!CaseBridgeList[i].activeSelf)
                        {
                            CaseBridgeList[i].SetActive(true);
                            CurrentlyActivatedCasesBridge.Add(CoordinateTemp);
                        }

                        else
                        {
                            CaseBridgeList[i].SetActive(false);
                            CurrentlyActivatedCasesBridge.Remove(CoordinateTemp);
                        }
                    }
                }

                if (CaseValue == "7")
                {
                    string[] arrivalTeleportArray = arrivalTeleport.Split(',');

                    string arrivalTeleportXString = arrivalTeleportArray[0].Substring(1, arrivalTeleportArray[0].Length-1);
                    string arrivalTeleportZString = arrivalTeleportArray[1].Substring(0, arrivalTeleportArray[1].Length-1);

                    int arrivalTeleportX = Int32.Parse(arrivalTeleportXString);
                    int arrivalTeleportZ = Int32.Parse(arrivalTeleportZString);

                    int positionX = (arrivalTeleportX * 10)-5;
                    int positionZ = (arrivalTeleportZ * 10)-5;

                    CubeGame.transform.position = new Vector3(positionX, 10, positionZ);
                }
            }

            catch (Exception e)
            {
                print(e);
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


    //Load JsonFile (Bridges and teleporters)
    void LoadData()
    {

        /* Load Switches And Bridges */
        SetSpawnPoint();

        try
        {

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

                        else
                        {
                            string CoordinateTemp = "(" + indexXCase.ToString() + "," + indexZCase.ToString() + ")";
                            CurrentlyActivatedCasesBridge.Add(CoordinateTemp);
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

                        else
                        {
                            string CoordinateTemp = "(" + indexXCase.ToString() + "," + indexZCase.ToString() + ")";
                            CurrentlyActivatedCasesBridge.Add(CoordinateTemp);
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
            
        }


        catch
        {

        }

        /* Load Teleporter */

        arrivalTeleport = loadedData.arrivalTeleport;
    }


    //Call in case of loose
    void LoosingProtocol()
    {
        inGame = false;
        CubeGame.SendMessage("Loose");

        LooseButtons.SetActive(true);

        Collider CubeCollider = CubeGame.GetComponent<Collider>();
        CubeCollider.isTrigger = false;
        CubeCollider.attachedRigidbody.useGravity = true;

    }


    //Call in case of win
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

        if (CurrentProgress.Count == sequenceData.ListLevels.Count && NameFile == SaveProgress.ListLevels[SaveProgress.ListLevels.Count-1])
        {
            SceneManager.LoadScene("SaveScore", LoadSceneMode.Single);
        }

        else
        {
            CubeGame.SendMessage("Loose");

            WinButtons.SetActive(true);

            Collider CubeCollider = CubeGame.GetComponent<Collider>();
            CubeCollider.isTrigger = false;
            CubeCollider.attachedRigidbody.useGravity = true;
        }
    }


    //When data has been loaded, place the cube at the spawn
    void SetSpawnPoint()
    {
        float postionX;
        float postionZ;

        postionX = (loadedData.SpawnX * CubeGame.transform.localScale[0]) - (CubeGame.transform.localScale[0] / 2);
        postionZ = (loadedData.SpawnZ * CubeGame.transform.localScale[2]) - (CubeGame.transform.localScale[2] / 2);

        CubeGame.transform.localPosition = new Vector3(postionX, CubeGame.transform.localScale[1] / 2, postionZ);
        CubeGame.transform.localEulerAngles = new Vector3(0, 0, 0);
    }


    //Set timer when user wants to use it
    void UseTimer()
    {
        timerGame = loadedData.timer;
    }


    //Go back to menu
    void GoMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    //Clear level and data and restart
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


    //Clear data and load next level (if exists)
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

        try
        {
            CurrentProgress.Add(sequenceData.ListLevels[CurrentProgress.Count]);
        }


        //If progress == listLevels
        catch
        {

        }

        SequenceDataEditor SaveProgress = new SequenceDataEditor();
        SaveProgress.ListLevels = sequenceData.ListLevels;
        SaveProgress.LevelProgress = CurrentProgress;
        SaveProgress.Moves = sequenceData.Moves;
        SaveProgress.Time = sequenceData.Time;


        string SaveProgressString = JsonUtility.ToJson(SaveProgress);

        File.WriteAllText("Assets\\Levels\\" + sequence + "\\sequenceData.json", SaveProgressString);

        

        try
        {
            for (int i = 0; i <= CurrentProgress.Count; i++)
            {
                if (CurrentProgress[i] == NameFile)
                {
                    NameFile = CurrentProgress[i + 1];
                    if (i<= CurrentProgress.Count -1)
                    {
                        CurrentProgress.RemoveAt(CurrentProgress.Count - 1);
                    }
                    break;
                }
            }
        }

        catch
        {
            NameFile = CurrentProgress[CurrentProgress.Count - 1];
        }

        TextNameLevel.GetComponentInChildren<Text>().text = NameFile;



        CompletePath = "Assets\\Levels\\" + sequence + "\\" + NameFile + ".txt";

        string DataPath = "Assets\\Levels\\" + sequence + "\\" + NameFile + ".json";


        string readText = File.ReadAllText(CompletePath);

        BoardListImport = Regex.Split(readText, "\n");

        BoardList.Clear();

        foreach (string TempString in BoardListImport)
        {
            if (TempString.Length > 1)
            {
                BoardList.Add(TempString);
            }
        }

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


    //Use to save elements for scores scene
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void QuitGame()
    {
        Application.Quit();
    }

}
