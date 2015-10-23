using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {

    public Vector2 mousePos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.position = mousePos;
	}
}
