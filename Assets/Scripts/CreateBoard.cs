using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBoard : MonoBehaviour {

    public GameObject CasePrefab;

	// Use this for initialization
	void Start () {
        LoadBoard();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void LoadBoard()
    {
        GameObject casePrefabClone = Instantiate(CasePrefab, transform.position, transform.rotation);
    }
}
