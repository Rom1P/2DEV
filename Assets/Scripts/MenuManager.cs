using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    public Button StartButtonObject;

	// Use this for initialization
	void Start () {
        Button StartButton = StartButtonObject.GetComponent<Button>();
        StartButton.onClick.AddListener(OnClickStartButton);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnClickStartButton()
    {
        Debug.Log("Click on Start Button");
    }
}
