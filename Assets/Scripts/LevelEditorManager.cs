using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelEditorManager : MonoBehaviour
{

    private int dimensions;
    public GameObject ButtonDim10;
    public GameObject ButtonDim15;
    

    public GameObject ButtonSave;

    public GameObject SetBrick;
    public GameObject SetWood;
    public GameObject SetNormal;
    public GameObject SetStrong;
    public GameObject SetSpawn;
    public GameObject SetArrival;

    public GameObject GroupButtons;

    List<List<string>> mapData;

    public GameObject ButtonCasePrefab;

    public Canvas CanvasButtonParent;

    private int SpawnX = 1;
    private int SpawnZ = 1;
    private int timer = 60;
    private List<String> ListBridges = new List<string>();

    private GameDataEditor GameDataAccess = new GameDataEditor();

    private string sequence;


    private SequenceDataEditor sequenceDataSaving;

    public Text LevelNameText;
    public Text TimerText;

    private Color32 ColorBrick = new Color32(109,109,109,255);
    private Color32 ColorWood = new Color32(103, 80, 80,255);
    private Color32 ColorNormalSwitch;
    private Color32 ColorStrongSwitch;
    private Color32 ColorSpawn = new Color32(1, 87, 155,255);
    private Color32 ColorArrivalPoint = new Color32(251, 140, 0,255);

    private int currentLine;
    private int currentColumn;
    private GameObject currentCase;
    private GameObject ArrivalPoint;
    private GameObject Spawn;
    private int ArrivalLine;
    private int ArrivalColumn;

    public GameObject TipBridge;

    private bool AddingToBridge = false;
    private string CurrentBridgeBuilding ="";

    // Use this for initialization
    void Start()
    {
        
        Button ButtonDim10Component = ButtonDim10.GetComponent<Button>();
        ButtonDim10Component.onClick.AddListener(SetDim10);

        Button ButtonDim15Component = ButtonDim15.GetComponent<Button>();
        ButtonDim15Component.onClick.AddListener(SetDim15);

        Button ButtonSaveComponent = ButtonSave.GetComponent<Button>();
        ButtonSaveComponent.onClick.AddListener(OnClickButtonSave);

        Button SetBrickButton = SetBrick.GetComponent<Button>();
        SetBrickButton.onClick.AddListener(SetCaseAsBrick);

        Button SetWoodButton = SetWood.GetComponent<Button>();
        SetWoodButton.onClick.AddListener(SetCaseAsWood);

        Button SetArrivalButton = SetArrival.GetComponent<Button>();
        SetArrivalButton.onClick.AddListener(SetCaseAsArrivalPoint);

        Button SetSpawnButton = SetSpawn.GetComponent<Button>();
        SetSpawnButton.onClick.AddListener(SetCaseAsSpawn);

        Button SetNormalSwitchButton = SetNormal.GetComponent<Button>();
        SetNormalSwitchButton.onClick.AddListener(SetCaseAsNormalSwitch);

        Button SetStrongSwitchButton = SetStrong.GetComponent<Button>();
        SetStrongSwitchButton.onClick.AddListener(SetCaseAsStrongSwitch);


        mapData = new List<List<string>>();

        try
        {
            GameObject SelectSequenceManagerObject = GameObject.Find("SelectSequenceManager");


            SelectSequenceManager SelectSequenceManagerAccess = new SelectSequenceManager();

            SelectSequenceManagerAccess = (SelectSequenceManagerObject.GetComponentInChildren<SelectSequenceManager>());

            sequence = SelectSequenceManagerAccess.sequenceSelected;
            Destroy(SelectSequenceManagerObject);
        }

        catch
        {
            sequence = "CustomRun1";
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    

    void SetDim10()
    {
        dimensions = 10;
        Button ButtonDim10Component = ButtonDim10.GetComponent<Button>();
        ButtonDim10Component.interactable = false;
        ButtonDim15.SetActive(false);

        LoadBoard();
    }

    void SetDim15()
    {
        dimensions = 15;
        ButtonDim15.SetActive(false);
        ButtonDim10.SetActive(false);

        LoadBoard();
    }
    
    void LoadBoard()
    {
        for (int i=0; i <= dimensions; i++)
        {
            List<string> TempList = new List<string>();
            for (int j = 0; j <= dimensions; j++)
            {
                TempList.Add("0");
                Vector3 position = new Vector3((i * 50)-((dimensions*50)/2), j * 50- ((dimensions * 50) / 2), 0);

                GameObject ButtonCase = Instantiate(ButtonCasePrefab, Vector3.zero, Quaternion.identity);

                ButtonCase.transform.SetParent(CanvasButtonParent.transform);

                ButtonCase.transform.localPosition = position;

                ButtonCase.name = i.ToString() + ","+ j.ToString();

                Button ButtonCaseComponent = ButtonCase.GetComponent<Button>();
                ButtonCaseComponent.onClick.AddListener(OnClickButtonCase);



            }
            mapData.Add(TempList);
        }

    }

    void OnClickButtonCase()
    {
        Debug.Log(EventSystem.current.currentSelectedGameObject.name);

        string NameObject = EventSystem.current.currentSelectedGameObject.name;

        string[] SplitName = NameObject.Split(',');

        currentLine = Int32.Parse(SplitName[1]);
        currentColumn = Int32.Parse(SplitName[0]);
        currentCase = EventSystem.current.currentSelectedGameObject;



        if (!GroupButtons.activeSelf)
        {
            GroupButtons.SetActive(true);
        }
    }

    void SaveData(string pathFolder,string NameLevel)
    {

        mapData[ArrivalLine][ArrivalColumn] = "7";

        File.WriteAllText(pathFolder + NameLevel + ".txt", "");
        foreach (List<string> TempListTostring in mapData)
        {
            string TempString = string.Join("", TempListTostring.ToArray());
            TempString += "\n";

            File.AppendAllText(pathFolder + NameLevel + ".txt", TempString + Environment.NewLine);
        }

        GameDataAccess.ListBridges = ListBridges;
        GameDataAccess.SpawnX = SpawnX;
        GameDataAccess.SpawnZ = SpawnZ;
        GameDataAccess.timer = timer;

        string GameDataSave = JsonUtility.ToJson(GameDataAccess);

        File.WriteAllText(pathFolder+NameLevel+".json", GameDataSave);
        


        if(!File.Exists(pathFolder + "sequenceData.json"))
        {
            List<string> ListLevel = new List<string>();
            List<string> ListLevelProgress = new List<string>();

            SequenceDataEditor ResetData = new SequenceDataEditor();
            ResetData.ListLevels = ListLevel;
            ResetData.LevelProgress = ListLevelProgress;
            ResetData.Moves = 0;
            ResetData.Time = 0;


            string SaveProgressString = JsonUtility.ToJson(ResetData);

            File.WriteAllText(pathFolder+"\\sequenceData.json", SaveProgressString);
        }

        string DataSequence = File.ReadAllText(pathFolder+ "sequenceData.json");

        sequenceDataSaving = JsonUtility.FromJson<SequenceDataEditor>(DataSequence);

        SequenceDataEditor UpdatingSequence = new SequenceDataEditor();

        UpdatingSequence.Moves = sequenceDataSaving.Moves;
        UpdatingSequence.Time = sequenceDataSaving.Time;

        List<string> ListLevels = sequenceDataSaving.ListLevels;
        List<string> ListProgress = sequenceDataSaving.LevelProgress;

        ListLevels.Add(NameLevel);

        if (ListProgress.Count == 0)
        {
            ListProgress.Add(ListLevels[0]);
        }


        UpdatingSequence.LevelProgress = ListProgress;
        UpdatingSequence.ListLevels = ListLevels;


        string UpdatedSequence = JsonUtility.ToJson(sequenceDataSaving);

        File.WriteAllText(pathFolder + "sequenceData.json", UpdatedSequence);
    }

    void OnClickButtonSave()
    {
        string NameLevel = LevelNameText.GetComponent<Text>().text;
        string pathFolder = "Assets\\Levels\\" + sequence + "\\";

        SaveData(pathFolder, NameLevel);
    }

    void SetCaseAsBrick()
    {

        ColorBlock cb = currentCase.GetComponent<Button>().colors;
        cb.normalColor = ColorBrick;
        currentCase.GetComponent<Button>().colors = cb;

        if (!AddingToBridge)
        {
            mapData[currentLine][currentColumn] = "1";
        }

        else
        {
            mapData[currentLine][currentColumn] = "3";
            CurrentBridgeBuilding += "(" + currentLine.ToString() + "," + currentColumn.ToString() + ");";
        }
        
    }

    void SetCaseAsWood()
    {

        ColorBlock cb = currentCase.GetComponent<Button>().colors;
        cb.normalColor = ColorWood;
        currentCase.GetComponent<Button>().colors = cb;

        if (!AddingToBridge)
        {
            mapData[currentLine][currentColumn] = "2";
        }

        else
        {
            mapData[currentLine][currentColumn] = "4";
            CurrentBridgeBuilding += "(" + currentLine.ToString() + "," + currentColumn.ToString() + ");";
        }

    }

    void SetCaseAsArrivalPoint()
    {
        try
        {
            ColorBlock cbArrivalPoint = ArrivalPoint.GetComponent<Button>().colors;
            cbArrivalPoint.normalColor = Color.white;
            ArrivalPoint.GetComponent<Button>().colors = cbArrivalPoint;
        }

        catch
        {

        }

        ColorBlock cb = currentCase.GetComponent<Button>().colors;
        cb.normalColor = ColorArrivalPoint;
        currentCase.GetComponent<Button>().colors = cb;

        ArrivalLine = currentLine;
        ArrivalColumn = currentColumn;

        ArrivalPoint = currentCase;

    }

    void SetCaseAsSpawn()
    {
        try
        {
            ColorBlock cbSpawn = Spawn.GetComponent<Button>().colors;
            cbSpawn.normalColor = Color.white;
            Spawn.GetComponent<Button>().colors = cbSpawn;
        }

        catch
        {

        }

        ColorBlock cb = currentCase.GetComponent<Button>().colors;
        cb.normalColor = ColorSpawn;
        currentCase.GetComponent<Button>().colors = cb;

        SpawnX = currentLine;
        SpawnZ = currentColumn;

        Spawn = currentCase;

    }

    void SetCaseAsNormalSwitch()
    {
        if (!AddingToBridge)
        {
            ColorBlock cb = currentCase.GetComponent<Button>().colors;
            cb.normalColor = ColorNormalSwitch;
            currentCase.GetComponent<Button>().colors = cb;

            mapData[currentLine][currentColumn] = "5";

            AddingToBridge = true;
            TipBridge.SetActive(true);

            CurrentBridgeBuilding += "N0(" + currentLine.ToString() + "," + currentColumn.ToString() + ")-[";


        }

        else
        {
            CurrentBridgeBuilding = CurrentBridgeBuilding.Substring(0, CurrentBridgeBuilding.Length - 1);
            CurrentBridgeBuilding += "]";
            ListBridges.Add(CurrentBridgeBuilding);
            CurrentBridgeBuilding = "";
            AddingToBridge = false;
            TipBridge.SetActive(false);
        }
    }

    void SetCaseAsStrongSwitch()
    {
        if (!AddingToBridge)
        {
            ColorBlock cb = currentCase.GetComponent<Button>().colors;
            cb.normalColor = ColorStrongSwitch;
            currentCase.GetComponent<Button>().colors = cb;

            mapData[currentLine][currentColumn] = "6";

            AddingToBridge = true;
            TipBridge.SetActive(true);

            CurrentBridgeBuilding += "S0(" + currentLine.ToString() + "," + currentColumn.ToString() + ")-[";


        }

        else
        {
            CurrentBridgeBuilding = CurrentBridgeBuilding.Substring(0, CurrentBridgeBuilding.Length - 1);
            CurrentBridgeBuilding += "]";
            ListBridges.Add(CurrentBridgeBuilding);
            CurrentBridgeBuilding = "";
            AddingToBridge = false;
            TipBridge.SetActive(false);
        }
    }
}

