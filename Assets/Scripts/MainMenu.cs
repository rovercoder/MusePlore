using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void startDiscovery()
    {
        Application.LoadLevel("discovery");
    }

    public void viewCollections()
    {
        Application.LoadLevel("collections");
    }
    
}
