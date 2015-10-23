using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public GameObject enemyToSpawn;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetButtonDown("Fire2")) {
            GameObject enemySpawned = Instantiate(enemyToSpawn, (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity) as GameObject;
        }
	}
}
