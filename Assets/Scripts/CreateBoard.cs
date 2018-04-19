using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class CreateBoard : MonoBehaviour {

    public GameObject CasePrefab;

    public string path;

	// Use this for initialization
	void Start () {
        LoadBoard();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void LoadBoard()
    {

        float ScaleX = CasePrefab.transform.localScale[0];
        float ScaleY = CasePrefab.transform.localScale[1];
        float ScaleZ = CasePrefab.transform.localScale[2];

        float IndexX = ScaleX / 2;
        float IndexY = ScaleY / 2;
        float IndexZ = ScaleZ / 2;

        path = "Assets\\Levels\\LevelTest.txt";
        string readText = File.ReadAllText(path);

        string[] xLines = Regex.Split(readText, "\n");

        foreach (string xLine in xLines)
        {
            IndexX = ScaleX / 2;
            for (int i = 0; i < xLine.Length; i++)
            {
                if(xLine[i].ToString() == "0")
                {
                    // Do Nothing yet
                }

                else if (xLine[i].ToString() == "1")
                {
                    Instantiate(CasePrefab, new Vector3(IndexX, IndexY, IndexZ), transform.rotation);
                }


                IndexX += ScaleX;
            }

            IndexZ += ScaleZ;
        }



        
        
    }
}
