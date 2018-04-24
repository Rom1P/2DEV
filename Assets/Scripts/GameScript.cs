using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour {

    Vector3 TopRight;
    Vector3 BottomRight;
    Vector3 TopLeft;
    Vector3 BottomLeft;

    public GameObject centerPrefab;

    private MenuManager MenuManagerAccess;

    private string NameFile;

    private Vector3 ArrivalPoint;


    // Use this for initialization
    void Start () {

        try
        {
            GameObject GameManagerObject = GameObject.Find("MenuManager");

            MenuManagerAccess = (GameManagerObject.GetComponentInChildren<MenuManager>());

            string NameFile = MenuManagerAccess.NameFile;


            Destroy(GameManagerObject);
        }

        catch
        {
            NameFile = "LevelTest";
        }

        Debug.Log(NameFile);


        string CompletePath = "Assets\\Levels\\" + NameFile + ".txt";
        Debug.Log(CompletePath);


        GameObject BoardLoaderObject = GameObject.Find("BoardLoader");
        BoardLoaderObject.SendMessage("LoadBoard", CompletePath);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void ReceiveArrivalPoint(Vector3 Message)
    {
        ArrivalPoint = Message;
        Debug.Log("Arrival Point Center = " + ArrivalPoint);
    }

    void ReceiveAfterRotate()
    {

        /*
         * Use To debug and mark points with a Sphere GameObject
         * 
         * GameObject TopRightGO =  Instantiate(centerPrefab, TopRight, transform.rotation);
        GameObject TopLeftGO =  Instantiate(centerPrefab, TopLeft, transform.rotation);
        GameObject BottomLeftGO =  Instantiate(centerPrefab, BottomLeft, transform.rotation);
        GameObject BottomRightGO =  Instantiate(centerPrefab, BottomRight, transform.rotation);*/


        if ((int)transform.localEulerAngles[2] == 90 || (int)transform.localEulerAngles[2] == 270)
        {
            Debug.Log("Rotated horizontally (X Main Scale)");
            
            TopRight = new Vector3(transform.localPosition[0] + transform.localScale[0], 0, transform.localPosition[2] + transform.localScale[2] / 2);
            TopLeft = new Vector3(transform.localPosition[0] - transform.localScale[0], 0, transform.localPosition[2] + transform.localScale[2] / 2);
            BottomLeft = new Vector3(transform.localPosition[0] - transform.localScale[0], 0, transform.localPosition[2] - transform.localScale[2] / 2);
            BottomRight = new Vector3(transform.localPosition[0] + transform.localScale[0], 0, transform.localPosition[2] - transform.localScale[2] / 2);
        }

        else if ((int)transform.localEulerAngles[0] == 90 || (int)transform.localEulerAngles[0] == 270)
        {
            Debug.Log("Rotated vertically (Z Main Scale)");
            
            TopRight = new Vector3(transform.localPosition[0] + transform.localScale[0] / 2, 0, transform.localPosition[2] + transform.localScale[2]);
            TopLeft = new Vector3(transform.localPosition[0] - transform.localScale[0] / 2, 0, transform.localPosition[2] + transform.localScale[2]);
            BottomLeft = new Vector3(transform.localPosition[0] - transform.localScale[0] / 2, 0, transform.localPosition[2] - transform.localScale[2]);
            BottomRight = new Vector3(transform.localPosition[0] + transform.localScale[0] / 2, 0, transform.localPosition[2] - transform.localScale[2]);
        }

        else
        {
            Debug.Log("Standing Up (Y Main Scale)");

            TopRight = new Vector3(transform.localPosition[0] + transform.localScale[0]/2, 0,transform.localPosition[2] + transform.localScale[2]/2);
            TopLeft = new Vector3(transform.localPosition[0] - transform.localScale[0]/2, 0,transform.localPosition[2] + transform.localScale[2]/2);
            BottomLeft = new Vector3(transform.localPosition[0] - transform.localScale[0]/2, 0,transform.localPosition[2] - transform.localScale[2]/2);
            BottomRight = new Vector3(transform.localPosition[0] + transform.localScale[0]/2, 0,transform.localPosition[2] - transform.localScale[2]/2);            
        }



    }
    
    void CheckWin()
    {
        
    }
}
