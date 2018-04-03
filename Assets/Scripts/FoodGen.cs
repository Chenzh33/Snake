using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodGen : MonoBehaviour {
	public GameObject food;
	public void GenFood(){
		float pX = UnityEngine.Random.Range (-10.0f, 10.0f);
		float pZ = UnityEngine.Random.Range (-10.0f, 10.0f);
		Vector3 p = new Vector3(pX,0,pZ);
		GameObject newFood = Instantiate (food, p, Quaternion.identity) as GameObject;
	}
}
