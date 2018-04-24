using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class CreateBoard : MonoBehaviour {

    public GameObject CasePrefab;
    public GameObject EmptyCasePrefab;
    public GameObject ArrivalCasePrefab;
    public GameObject CubeGame;




    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void LoadBoard(string path)
    {

        float ScaleX = CasePrefab.transform.localScale[0];
        float ScaleY = CasePrefab.transform.localScale[1];
        float ScaleZ = CasePrefab.transform.localScale[2];

        float IndexX = ScaleX / 2;
        float IndexY = ScaleY / 2;
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
                    Instantiate(CasePrefab, new Vector3(IndexX, IndexY, IndexZ), transform.rotation);
                }

                else if (xLine[i].ToString() == "2")
                {
                    CubeGame.transform.localPosition = new Vector3(IndexX, CubeGame.transform.localScale[1]/2, IndexZ);
                }

                else if (xLine[i].ToString() == "3")
                {
                    Instantiate(ArrivalCasePrefab, new Vector3(IndexX, IndexY, IndexZ), transform.rotation);
                    GameObject GameManager = GameObject.Find("GameManager");
                    GameManager.SendMessage("ReceiveArrivalPoint", new Vector3(IndexX, IndexY, IndexZ));
                }


                IndexX += ScaleX;
            }

            IndexZ += ScaleZ;
        }



        
        
    }
}
