using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour {

    Vector3 TopRight = new Vector3();
    Vector3 BottomRight = new Vector3();
    Vector3 TopLeft = new Vector3();
    Vector3 BottomLeft = new Vector3();


    // Use this for initialization
    void Start () {      

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void ReceiveAfterRotate()
    {
        Debug.Log(transform.localPosition);
        Debug.Log(transform.localEulerAngles);

        if ((int)transform.localEulerAngles[2] == 90 || (int)transform.localEulerAngles[2] == 270)
        {
            Debug.Log("Rotated horizontally");
        }

        else if ((int)transform.localEulerAngles[0] == 90 || (int)transform.localEulerAngles[0] == 270)
        {
            Debug.Log("Rotated vertically");
        }

        else
        {
            Debug.Log("Standing Up");
        }

    }
    
}
