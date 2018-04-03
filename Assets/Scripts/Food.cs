using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Food : MonoBehaviour {
	Vector3 startPoint;
	Collider col;
	void Start () {
		startPoint = transform.position + new Vector3(0,2f,0);
		col = GetComponent<Collider>();
	}
	void Update () {
		transform.position =  startPoint+new Vector3(0, Mathf.Sin(Time.time * Mathf.PI / 2) ,0);
	}

}
