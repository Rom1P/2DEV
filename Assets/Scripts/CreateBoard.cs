using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class CreateBoard : MonoBehaviour {

    public GameObject WoodCasePrefab;
    public GameObject BrickCasePrefab;
    public GameObject NormalSwitchPrefab;
    public GameObject StrongSwitchPrefab;
    public GameObject EmptyCasePrefab;
    public GameObject ArrivalCasePrefab;
    public GameObject CubeGame;

    private List<GameObject> CaseBridgeList = new List<GameObject>();
      


    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void LoadBoard(string path)
    {

        float ScaleX = WoodCasePrefab.transform.localScale[0];
        float ScaleY = WoodCasePrefab.transform.localScale[1];
        float ScaleZ = WoodCasePrefab.transform.localScale[2];

        float IndexX = ScaleX / 2;
        float IndexY = -(ScaleY / 2);
        float IndexZ = ScaleZ / 2;
        

        
        string readText = File.ReadAllText(path);

        string[] xLines = Regex.Split(readText, "\n");

        foreach (string xLine in xLines)
        {
            IndexX = ScaleX / 2;
            for (int i = 0; i < xLine.Length; i++)
            {
                if(xLine[i].ToString() == "0")
                {
                    Instantiate(EmptyCasePrefab, new Vector3(IndexX, IndexY, IndexZ), transform.rotation);
                }

                else if (xLine[i].ToString() == "1")
                {
                    Instantiate(BrickCasePrefab, new Vector3(IndexX, IndexY, IndexZ), transform.rotation);
                }

                else if (xLine[i].ToString() == "2")
                {
                    Instantiate(WoodCasePrefab, new Vector3(IndexX, IndexY, IndexZ), transform.rotation);
                }

                else if (xLine[i].ToString() == "5")
                {
                    Instantiate(NormalSwitchPrefab, new Vector3(IndexX, IndexY, IndexZ), transform.rotation);
                }

                else if (xLine[i].ToString() == "6")
                {
                    Instantiate(StrongSwitchPrefab, new Vector3(IndexX, IndexY, IndexZ), transform.rotation);
                }

                else if (xLine[i].ToString() == "9")
                {
                    Instantiate(ArrivalCasePrefab, new Vector3(IndexX, IndexY, IndexZ), transform.rotation);
                }





                IndexX += ScaleX;
            }

            IndexZ += ScaleZ;
        }
        
    }
    
}
