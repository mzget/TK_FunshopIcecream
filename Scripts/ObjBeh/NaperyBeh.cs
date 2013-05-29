using UnityEngine;
using System.Collections;

public class NaperyBeh : ScriptableObject {

    public ProductAssemble productAssemble;
    public GameObject instance;

	// Use this for initialization
	void OnEnable () {
        instance = GameObject.Find("Napery");
	}
	
	void OnDestroy () {
	
	}

    internal bool CheckingStatus()
    {
        if (productAssemble == null)
            return true;
        else
            return false;
    }
}
