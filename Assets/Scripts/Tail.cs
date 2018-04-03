using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour {
	private GameObject head;
	private Tail prev;
	public LinkList<Vector3> nextTurnPoints;
	Snake s;
	void Awake () {
		nextTurnPoints = new LinkList<Vector3> ();
	}
	public void SetHead (GameObject _head) {
		head = _head;
	}

	public void SetParent (Tail _prev) {
		prev = _prev;
	}
	public void Init () {
		s = head.GetComponent<Snake> ();
		if (prev != null) {
			
			LinkNode<Vector3> iter = prev.nextTurnPoints.head;
			while (iter != null) {
				nextTurnPoints.AddNodeEnd (new LinkNode<Vector3> (new Vector3 (iter.data.x, iter.data.y, iter.data.z)));
				iter = iter.next;
			}
		}
	}
	public void GameOver(){
		Destroy(gameObject);
	}

	void FixedUpdate () {

		transform.Translate (transform.forward * s.speed * Time.fixedDeltaTime, Space.World);
		if (nextTurnPoints.head != null) {
			Debug.DrawLine (transform.position, nextTurnPoints.FirstNode ().data, Color.red, 1.0f);
			if (Vector3.Distance (transform.position, nextTurnPoints.FirstNode ().data) < 0.01f) {
				nextTurnPoints.DeleteFirstNode ();
				Vector3 rotEuler;
				if (prev != null)
					rotEuler = new Vector3 (0, Vector3.SignedAngle (transform.forward, prev.transform.position - transform.position, Vector3.up), 0);
				else
					rotEuler = new Vector3 (0, Vector3.SignedAngle (transform.forward, head.transform.position - transform.position, Vector3.up), 0);
				transform.Rotate (rotEuler, Space.World);

			}

		}
	}
}