using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadMyScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
    SceneManager.LoadSceneAsync("GEARS/Scenes/Menu", LoadSceneMode.Single);
	}
	

}
