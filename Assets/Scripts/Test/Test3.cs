using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test3 : MonoBehaviour {
    private bool updatedOnce = false;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (updatedOnce == false)
        {
            GameObject go = GameObject.Find("RenderObjects");
            bool floor = go.GetComponent<RenderAtomsAndBonds>().calledUtility;
            if (floor == true)
            {
                this.gameObject.transform.position = new Vector3(6,6,6);
                Debug.Log("Position: " + this.gameObject.transform.position);
                updatedOnce = true;
            }
        }
    }
}
