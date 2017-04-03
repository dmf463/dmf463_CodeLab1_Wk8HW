using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElevatorScript : MonoBehaviour {

    public GameObject elevatorDoor;
    public float speed;
	float openHeight = 4.5f;	// How high the elevator door is when it is considered 'open'
	float closedHeight = 1.583f;			// How high the elevator door is when it is considered 'closed'
    public static bool isElevatorOpening = false;
	public static bool doorOpen = false;
    public static bool isElevatorClosing = false;
	Transform headCollider;
	public GameObject readyLight;
	bool lightOn;
    public int nextLevel;

    GameObject backWall;
    public AudioSource audioSource;
    public AudioClip elevatorBeep;

	void Start()
	{
		headCollider = GameObject.Find("FollowHead").transform;
	}

    void OnTriggerStay(Collider other)
    {
        if (this.name == "Open Trigger")
        {
            if (other.gameObject.name == "Foot Cube")
            {
                Debug.Log("Test working");
                isElevatorOpening = true;
                isElevatorClosing = false;
            }
        }

        if(this.name == "Close Trigger")
        {
            if (other.gameObject.name == "Foot Cube")
            {
                isElevatorClosing = true;
                isElevatorOpening = false;
            }
        }
    }

    public void OpenDoor()
    {
        isElevatorOpening = true;
    }
	
	// Update is called once per frame
	void Update () {

        if (isElevatorOpening == true && doorOpen == false)
        {
            Debug.Log("Elevator Opening");
            isElevatorClosing = false;
            elevatorDoor.transform.Translate(Vector3.up * speed * Time.deltaTime);
			
			// If we've gone above the player's head, turn on the light.
			if (elevatorDoor.transform.position.y-elevatorDoor.transform.localScale.y/2 >= headCollider.position.y + .15f && !lightOn)
			{
				readyLight.GetComponent<Light>().intensity = 5f;
                if (audioSource != null) audioSource.PlayOneShot(elevatorBeep);
				lightOn = true;
			}
			
			if (elevatorDoor.transform.localPosition.y >= openHeight)
			{
				doorOpen = true;
			}
        }

        if (isElevatorClosing == true)
        {
            Debug.Log("Elevator Closing");
            isElevatorOpening = false;
            if (elevatorDoor.transform.localPosition.y > closedHeight)
			{
				elevatorDoor.transform.Translate(Vector3.down * speed * Time.deltaTime);
				readyLight.GetComponent<Light>().intensity -= 1f*Time.deltaTime;
			}
			else if (elevatorDoor.transform.localPosition.y <= closedHeight && lightOn)
			{
				StartCoroutine(SceneChange(0));
			}
        }

		isElevatorOpening = false;
		isElevatorClosing = false;
    }

    IEnumerator SceneChange(float time)
    {
        yield return new WaitForSeconds(time);
        isElevatorClosing = false;
		isElevatorOpening = false;
		doorOpen = false;
        SceneManager.LoadScene("Level"+nextLevel);
    }
}
