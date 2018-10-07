using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomPrefab : MonoBehaviour {

    private Color atomColor;
    private Vector3 coordinates;

    public Color AtomColor
    {
        get{ return atomColor; }
        set { atomColor = value; }
    }

    public Vector3 Coordinates
    {
        get { return coordinates; }
        set { coordinates = value; }
    }
    // Use this for initialization
    void Start () {
        this.gameObject.transform.position = coordinates;
        this.gameObject.GetComponent<Renderer>().material.color = atomColor;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
