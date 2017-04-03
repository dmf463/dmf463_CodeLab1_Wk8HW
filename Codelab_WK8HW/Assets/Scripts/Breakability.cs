using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakability : MonoBehaviour {

    [SerializeField] float breakForce = 1f;
    [SerializeField] float breakExplodeForce = 100f;

    // Copies of the individual pieces of the mesh which will fly around when the object is shattered.
    [SerializeField] List<GameObject> shatterbuddies;
    [SerializeField] List<Transform> shatterbuddyTargetPositions;

    void Awake()
    {
        // Tell all mesh pieces to ignore colliision with one another.
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child1 = transform.GetChild(i);

            if (child1.gameObject.activeSelf)
            {
                Transform child2 = null;

                for (int j = 0; j < transform.childCount; j++)
                {
                    child2 = transform.GetChild(i);
                }

                if (child1 != child2)
                {
                    Physics.IgnoreCollision(child1.GetComponent<Collider>(), child2.GetComponent<Collider>(), true);
                }
            }
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<Rigidbody>() != null)
            {
                transform.GetChild(i).GetComponent<Rigidbody>().detectCollisions = true;
            }
        }
    }

    public void SetupGameObjectForShattering()
    {
        if (shatterbuddyTargetPositions == null) shatterbuddyTargetPositions = new List<Transform>();
        if (shatterbuddies == null) shatterbuddies = new List<GameObject>();

        // Delete any existing shatterbuddies and clear the list.
        if (shatterbuddies.Count > 0)
        {
            for (int i = 0; i < shatterbuddies.Count; i++)
            {
                if (shatterbuddies[i] != null)
                {
                    shatterbuddies[i].SetActive(true);
                    shatterbuddies[i].transform.parent = null;
                    DestroyImmediate(shatterbuddies[i]);
                    shatterbuddies[i] = null;
                }
            }
        }

        shatterbuddies.Clear();

        // Setup colliders and rigidbodies, then add them to the above lists.
        SetupShatterBuddies(transform);

        // Make new shatterBuddies into my babies.
        //foreach (GameObject shatterBud in shatterbuddies)
        //{
        //    shatterBud.transform.SetParent(transform);
        //}
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.isStatic && collision.relativeVelocity.magnitude >= breakForce)
        {
            Debug.Log("Shattering");
            Shatter();
        }
    }


    void SetupShatterBuddies(Transform targetObject)
    {
        // Setup any children also.
        if (targetObject.childCount > 0)
        {
            for (int i = 0; i < targetObject.childCount; i++)
            {
                SetupShatterBuddies(targetObject.GetChild(i));
            }
        }

        // Make sure this object has a MeshRenderer and MeshFilter (i.e.make sure it's visible).
        if (targetObject.GetComponent<MeshRenderer>() != null && targetObject.GetComponent<MeshFilter>() != null)
        {
            // First give this child object (not the shatterbuddy) a mesh collilder.
            if (targetObject.GetComponent<Collider>() == null)
            {
                targetObject.gameObject.AddComponent<MeshCollider>();
                targetObject.gameObject.GetComponent<MeshCollider>().convex = true;
            }

            if (targetObject.GetComponent<Rigidbody>() != null)
            {
                DestroyImmediate(targetObject.GetComponent<Rigidbody>());
                //targetObject.GetComponent<Rigidbody>().detectCollisions = true;
                //targetObject.GetComponent<Rigidbody>().isKinematic = true;
            }

            // Also give this child a ReportCollisionToParentScript and set this object as the parent.
            if (targetObject.GetComponent<ReportCollisionToParent>() != null)
            {
                DestroyImmediate(targetObject.GetComponent<ReportCollisionToParent>());
            }

            GameObject shatterBuddy = Instantiate(targetObject.gameObject, targetObject.position, Quaternion.identity);

            // Give the shatterbuddy a rigidbody.
            if (shatterBuddy.GetComponent<Rigidbody>() == null)
            {
                Rigidbody newRigidbody = shatterBuddy.gameObject.AddComponent<Rigidbody>();
            }

            // Add and set up a mesh collider if there is no collider already, then disable it.
            if (shatterBuddy.GetComponent<Collider>() == null)
            {
                MeshCollider newCollider = shatterBuddy.gameObject.AddComponent<MeshCollider>();
            }

            shatterbuddies.Add(shatterBuddy);
            shatterBuddy.transform.localScale = targetObject.lossyScale;

            shatterBuddy.GetComponent<Rigidbody>().isKinematic = true;
            shatterBuddy.GetComponent<Rigidbody>().detectCollisions = false;
            shatterBuddy.GetComponent<Rigidbody>().useGravity = false;
            shatterBuddy.GetComponent<MeshCollider>().inflateMesh = true;
            shatterBuddy.GetComponent<MeshCollider>().convex = true;
            shatterBuddy.GetComponent<MeshCollider>().skinWidth = 0.01f;
            shatterBuddy.GetComponent<MeshCollider>().enabled = true;
            shatterBuddy.GetComponent<MeshCollider>().isTrigger = true;
            shatterBuddy.GetComponent<MeshCollider>().sharedMesh = shatterBuddy.GetComponent<MeshFilter>().sharedMesh;
            shatterBuddy.GetComponent<MeshRenderer>().sharedMaterial.color = Color.gray;
            shatterBuddy.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_Emission", Color.gray);

            shatterBuddy.transform.position = new Vector3(1000, 1000, 1000);

            shatterBuddy.transform.SetParent(GameObject.Find("Shatterbuddies").transform);

            // Remember target position of this shatterbuddy.
            shatterbuddyTargetPositions.Add(targetObject.transform);
        }
    }


    void Shatter()
    {
        // Activate all shatterbuddies.
        foreach (GameObject shatterbuddy in shatterbuddies)
        {
            //shatterbuddy.SetActive(true);
            shatterbuddy.GetComponent<Rigidbody>().isKinematic = false;
            shatterbuddy.GetComponent<Rigidbody>().useGravity = true;
            shatterbuddy.GetComponent<Rigidbody>().detectCollisions = true;
            shatterbuddy.GetComponent<MeshCollider>().isTrigger = false;
            shatterbuddy.GetComponent<MeshCollider>().enabled = true;

            shatterbuddy.transform.position = shatterbuddyTargetPositions[shatterbuddies.IndexOf(shatterbuddy)].position;
            shatterbuddy.transform.rotation = shatterbuddyTargetPositions[shatterbuddies.IndexOf(shatterbuddy)].rotation;

            shatterbuddy.GetComponent<Rigidbody>().AddExplosionForce(breakExplodeForce, transform.position, 10f);
        }

        GameObject.Find("Level Manager").GetComponent<BreakLevelManager>().statuesBroken++;

        // Deactivate self.
        gameObject.SetActive(false);
    }


    void ExplodeAllChildren(Transform target)
    {
        // Do it to all children.
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<Rigidbody>() != null)
                {
                    transform.GetChild(i).GetComponent<Rigidbody>().AddExplosionForce(breakExplodeForce, transform.position, 5f);
                }
            }
        }

        // Explode myself.
        GetComponent<Rigidbody>().AddExplosionForce(breakExplodeForce, transform.position, 10f);
    }
}


