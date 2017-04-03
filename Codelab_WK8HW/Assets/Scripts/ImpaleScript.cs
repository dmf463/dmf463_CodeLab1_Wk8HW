using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ImpaleScript : MonoBehaviour {

    public GameObject ceiling;
    float impaledCount;
    public float speed;
    public float ceilingTrigger;

    GameObject lightSource;
    GameObject room;

    public AudioClip hurt;
    public AudioClip spikeEngine;
    AudioSource audioSource;

	// Use this for initialization
	void Start () {

        impaledCount = 0;
        lightSource = GameObject.Find("Directional Light");
        room = GameObject.Find("Room");
        audioSource = room.GetComponent<AudioSource>();
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Foot Cube")
        {
            Debug.Log("OUCH");
            audioSource.PlayOneShot(hurt);
            lightSource.GetComponent<Light>().color = Color.blue;
            impaledCount += 1;
            Debug.Log("ImpaledCount = " + impaledCount);
        }

        if (other.gameObject.name == ("HeadCollider"))
        {
            Debug.Log("DEAD HEAD");
            SceneManager.LoadScene("Lobby");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Foot Cube")
        {
            Debug.Log("OUCH");
            lightSource.GetComponent<Light>().color = Color.red;
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (impaledCount > ceilingTrigger)
        {
            audioSource.PlayOneShot(spikeEngine);
            ceiling.transform.Translate(Vector3.forward * speed * Time.deltaTime);
            //StartCoroutine(WaitTillDeath(6));
        }
		
	}

    IEnumerator WaitTillDeath(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("Lobby");
    }
}
