using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    AudioSource audio;
    public AudioClip clip;
    public ParticleSystem particles;
    private Overlay intScript;
    private ThrowBall playerScript;
    private int ballcount=0;
    private int cup;
    private int lastCup;
    private float distance;
    private float closestDistance;
    private GameObject closestCup;


   
    void Start()
    {
        audio = GetComponent<AudioSource>();
        intScript = GameObject.Find("Canvas").GetComponent<Overlay>();
        playerScript = GameObject.Find("Player").GetComponent<ThrowBall>();

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
            SpawnBall();
            playerScript.collision = true;

        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8)
        {
            if (!playerScript.collision)
            {
                playerScript.collision = true;
            }
            audio.PlayOneShot(clip);
            audio.volume -= 0.1f;
            if (!playerScript.ballInHand)
            {
                StartCoroutine(WaitSpawnBall(1f));
                playerScript.ballInHand = true;
            }
            StartCoroutine(intScript.DestroyObj(gameObject, 4f));
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        playerScript.collision = true;

        if (other.gameObject.layer == 10)
        {
            playerScript.countAtHit = playerScript.shotCount;
           
            if (other.gameObject.transform.parent.gameObject == playerScript.lastCup && playerScript.shotCount == playerScript.countAtHit+1)
            {
                
                distance = 0;
                closestCup = null;
                foreach(GameObject o in playerScript.cupObjs)
                {
                    distance = o.transform.position.magnitude - other.transform.position.magnitude;
                    if(distance< closestDistance && distance>0)
                    {
                        closestCup = o;
                    }
                }
                if (playerScript.cupObjs.Count <2)
                {
                    ParticleSystem part = Instantiate(particles, closestCup.transform.position, Quaternion.Euler(-90f, 0f, 0f));
                    StartCoroutine(intScript.DestroyObj(closestCup, 1f));

                }    
            }
            Debug.Log("cup destroyed" + other.transform.parent.gameObject.name);
            StartCoroutine(DestroyCup(2f, other.transform.parent.gameObject));
            DestroyImmediate(other.transform.parent.gameObject);

            playerScript.lastCup = other.gameObject.transform.parent.gameObject;
            ParticleSystem parts = Instantiate(particles, transform.position,Quaternion.Euler(-90f,0f,0f));
            SpawnBall();
            //StartCoroutine(intScript.DestroyObj(other.transform.parent.gameObject, 3f));
            StartCoroutine(WaitSpawnBall(2f));
            parts.transform.parent = null;
            cup = int.Parse(other.gameObject.transform.parent.gameObject.name);
            //Debug.Log(cup + "shit snaps!");
            //playerScript.cups.Remove(cup);
            //lastCup = cup;
            Destroy(gameObject);
            

        }
    }
    void SpawnBall()
    {
        playerScript.collision =false;

        if (playerScript.canThrow && !playerScript.newBall)
        {
            ballcount++;
            playerScript.createBall();
        }
    }
    public IEnumerator WaitSpawnBall(float t)
    {
        yield return new WaitForSeconds(t);
        SpawnBall();

    }

    public IEnumerator DestroyCup(float t, GameObject o)
    {
        yield return new WaitForSeconds(t);
        DestroyImmediate(o);

    }
    private int selectCup(List<int> cups, int cup)
    {
        return 1;
    }
}
