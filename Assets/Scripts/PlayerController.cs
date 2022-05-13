using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float speed = 100.0f;
	public Text countText;
	public Text winText;
	//public GUITexture myMoveBtn = null;

	private Rigidbody rb;
	private int count;
	private int maxPickups;
	private int finId1 = -1; //id finger for cancel touch event
	private float moveHorizonal;
	private float moveVertical;
	private Vector3 touchPosition;
	Vector3 movement = new Vector3 ( 0.0f, 0.0f, 0.0f);
	bool supportsMultiTouch;


	void Start() 
	{
		rb = GetComponent<Rigidbody>();
		count = 0;
		SetCountText ();
		winText.text = "";
		maxPickups = GameObject.FindGameObjectsWithTag ("Pick Up").Length; 
		supportsMultiTouch = Input.multiTouchEnabled;
	}

	void Awake() 
	{
		rb = GetComponent<Rigidbody>();
		count = 0;
		SetCountText ();
		winText.text = "";
		maxPickups = GameObject.FindGameObjectsWithTag ("Pick Up").Length; 
	}

	void FixedUpdate () {

		moveHorizonal = Input.GetAxis ("Horizontal");
		moveVertical = Input.GetAxis ("Vertical");
		if ((moveHorizonal == 0.0f) & (moveVertical == 0.0f) ) {
		  moveHorizonal = Input.GetAxis ("Mouse X");
		  moveVertical = Input.GetAxis ("Mouse Y");
		}
		if (supportsMultiTouch) {
			if ((Input.touchCount > 0) && 
			   Input.GetTouch(0).phase == TouchPhase.Moved) {
				    Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
					moveHorizonal = touchDeltaPosition.x;
					moveVertical = touchDeltaPosition.y;
			}
		}
		movement.Set (moveHorizonal, 0.0f, moveVertical);
		rb.AddForce (movement * speed);
	}
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Pick Up")) {
			other.gameObject.SetActive (false);
			count++;
			SetCountText ();

		}
	}

	void SetCountText ()
	{
		countText.text = "Count: " + count.ToString ();
		if (count >= maxPickups) {
			winText.text = "YOU WIN!";
		}
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.R)) 
			{
			  //Application.LoadLevel(0);
			SceneManager.LoadScene("MiniGame");
		 	//Application.LoadLevel (Application.loadedLevelName);
			 // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
		//if (supportsMultiTouch) {
		//	if (Input.GetTouch(0).tapCount > 1) {
		//		SceneManager.LoadScene("MiniGame");
		//	}
		//}
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			Application.Quit (); 
		}
	}

	void OnCollisionEnter(Collision other)
	{
		//Checking if the collision was with a wall 
		if(other.gameObject.tag == "Wall")
		{
			rb = GetComponent<Rigidbody>();
			Vector3 vel = rb.velocity;
			//Finding out the horizontal direction of the ball after hitting a wall
			Vector2 horizontalvector = new Vector2(vel.x, vel.z);
			//Normalizing the vector so only the direction remains without the speed
			horizontalvector = horizontalvector.normalized;
			//Adding the force of the original vector to the horizontal direction 
			horizontalvector = vel.magnitude * horizontalvector;
			//Now since the horizontal vector has the same force of the original velocity
			//All that remains is to give the ball the new velocity.
			//horizontalvector.x in x axis
			//horizontalvector.y in z axis
			rb.velocity = new Vector3(horizontalvector.x, 0, horizontalvector.y);
		}
	}


}
