using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FoodGen))]
public class Snake : MonoBehaviour {
	public enum MoveState { Left, Right, Up, Down }
	public MoveState currentState;
	public MoveState inputState;
	private FoodGen fg;
	public static event System.Action OnEatFood;
	public static event System.Action OnGameOver;
	public float interval = 1.0f;
	public float speed = 2.0f;
	private float currentTime = 0.0f;
	public GameObject head;
	public GameObject tail;
	private Vector3 direction;
	public LinkList<Tail> tails;
	private Vector3 tailSpawnPoint;


	// Use this for initialization
	void Start () {
		UnityEngine.Random.InitState ((int) Time.realtimeSinceStartup);
		int state = UnityEngine.Random.Range (1, 4);
		switch (state) {
			case 1:
				currentState = MoveState.Left;
				break;
			case 2:
				currentState = MoveState.Right;
				break;
			case 3:
				currentState = MoveState.Up;
				break;
			case 4:
				currentState = MoveState.Down;
				break;
		}
		tails = new LinkList<Tail> ();
		fg = GetComponent<FoodGen>();
		FreshMove ();
		OnEatFood += fg.GenFood; 
		OnEatFood += AddTail; 
		OnGameOver += GameOver; 
		fg.GenFood();
	}

	public Vector3 GetMoveDirection (MoveState state) {
		Vector3 direction = Vector3.zero;

		switch (state) {
			case MoveState.Left:
				direction.x = -1.0f;
				break;
			case MoveState.Right:
				direction.x = 1.0f;
				break;
			case MoveState.Up:
				direction.z = 1.0f;
				break;
			case MoveState.Down:
				direction.z = -1.0f;
				break;
		}
		return direction;
	}

	// Update is called once per frame
	void Update () {
		float xInput = Input.GetAxisRaw ("Horizontal");
		float yInput = Input.GetAxisRaw ("Vertical");
		if (xInput > 0) {
			inputState = MoveState.Right;
		}
		if (xInput < 0) {
			inputState = MoveState.Left;
		}
		if (yInput > 0) {
			inputState = MoveState.Up;
		}
		if (yInput < 0) {
			inputState = MoveState.Down;
		}
		
	}

	void FixedUpdate () {
		currentTime += Time.fixedDeltaTime;
		if (currentTime >= interval) {
			if (inputState != currentState) {
				if (GetMoveDirection (inputState) + GetMoveDirection (currentState) != Vector3.zero) {

					currentState = inputState;
					FreshMove ();
				}
			}
			currentTime = 0.0f;
		}
		transform.Translate (direction * speed * Time.fixedDeltaTime, Space.World);
		FreshTailSpawnPoint ();
	}

	void FreshMove () {
		if (direction != GetMoveDirection (currentState)) {
			direction = GetMoveDirection (currentState);
			Vector3 rotEuler = new Vector3 (0, Vector3.SignedAngle (transform.forward, direction, Vector3.up), 0);
			transform.Rotate (rotEuler, Space.World);
			FreshTurnPoints ();
		}
	}

	void FreshTurnPoints () {
		if (tails.head == null)
			return;
		LinkNode<Tail> iter = tails.head;
		while (iter != null) {
			iter.data.nextTurnPoints.AddNodeEnd (new LinkNode<Vector3> (transform.position));
			iter = iter.next;

		}
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Food")) {
			other.gameObject.SetActive (false);
			Debug.Log ("collision!");
			OnEatFood();
		}
		if(other.gameObject.CompareTag("Tail") && other.gameObject != tails.FirstNode().data.gameObject){
			OnGameOver();

		}
	}
	void FreshTailSpawnPoint () {
		Transform tailPoint;
		if (tails.head != null)
			tailPoint = tails.LastNode ().data.transform;
		else
			tailPoint = transform;
		tailSpawnPoint = tailPoint.position - tailPoint.forward * 2;
	}

	void AddTail () {
		Transform currentTailTransform;
		if (tails.head != null)
			currentTailTransform = tails.LastNode ().data.transform;
		else
			currentTailTransform = transform;
		GameObject newTail = Instantiate (tail, tailSpawnPoint, currentTailTransform.rotation) as GameObject;
		LinkNode<Tail> newTailNode = new LinkNode<Tail> (newTail.GetComponent<Tail>());

		Tail parent;
		if (tails.head != null) {

			parent = tails.LastNode ().data;
		} else {
			parent = null;

		}
		tails.AddNodeEnd (newTailNode);
		Tail tl = tails.LastNode ().data.GetComponent<Tail> ();
		tl.SetHead (this.gameObject);
		tl.SetParent (parent);
		tl.Init ();
		OnGameOver += tl.GameOver;
	}

	void GameOver(){
		Destroy(gameObject);
	}
}