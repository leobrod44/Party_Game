using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBall : MonoBehaviour
{
    public float forceFactor;
    public float ballGravity;
    //[SerializeField]
    public float angle;
    public bool newBall = true;
    public float moveSpeed;
    private float timeEndPressed;
    private float timeStartPressed;
    private bool charging = false;
    private float chargeForce;
    public GameObject ball;
    public static float slideValue;
    public Interface intScript;
    GameObject currentBall = null;
    Rigidbody rb;
    public bool canThrow = true;
    public bool ballInHand = true;
    public GameObject arrow;
    private float chargeTime;
    private Vector3 shootDir;
    public Transform landPosTransform;
    private Vector3 landPos;
    private float g;
    public float arrowHeight;
    private float temp;
    public List<int> cups = new List<int>{1,2,3,4,5,6};
    public List<GameObject> cupObjs = new List<GameObject>();
    public GameObject lastCup;
    public bool secondShot=false;
    public bool collision;
    public int shotCount;
    public int countAtHit;


    //get gameobject from collision then if last coll gameoject = then take nearest? gl with that homie

    void Start()
    {
        shotCount = 0;
        countAtHit = 0;
        Transform side1 = GameObject.Find("side 1").transform;
        rb = GetComponent<Rigidbody>();
        Physics.gravity = new Vector3(0f, -ballGravity, 0f);
        currentBall = createBall();
        angle = Mathf.Deg2Rad * angle;
        landPos = landPosTransform.position;
        landPos.y += arrowHeight;
        g = Physics.gravity.y - ballGravity;
        temp = arrowHeight;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 moveX = Input.GetAxis("Horizontal") * Vector3.right * moveSpeed;
        transform.Translate(moveX);

        if (Input.GetKeyDown(KeyCode.Space) && !charging)
        {
            timeStartPressed = Time.time;
            charging = true;
        }
        if (charging)
        {
            chargeTime = Time.time - timeStartPressed;
            chargeForce = (chargeTime) * forceFactor;
            shootDir = transform.forward * chargeForce * Mathf.Cos(angle) + transform.up * chargeForce * Mathf.Sin(angle);
            float t2 = (Mathf.Sqrt(Mathf.Pow(shootDir.y, 2) + (2 * g * (landPos.y - transform.position.y))) - shootDir.y) / -g;
            float t1 = -(4 * shootDir.y) / (g);
            landPos.x = transform.position.x;
            landPos.z = transform.position.z + shootDir.z*(t1+t2);
            arrow.transform.position = landPos;
            landPos.y = temp;
            if (Input.GetKeyUp(KeyCode.Space))
            {
                shotCount++;
                if (secondShot)
                    secondShot = false;
                else
                    secondShot = true;
                timeEndPressed = Time.time;
                charging = false;
                chargeTime = 0;
                if (newBall)
                    Throw(chargeForce, angle, currentBall, shootDir);
                arrowHeight = temp+=0.1f;
                landPos.y = arrowHeight;
                arrow.transform.position = landPos;
            }
        }

        if (charging)
        {
            slideValue = Time.time - timeStartPressed;
        }
        if (newBall)
        {
            currentBall.transform.position = transform.position;
            collision = false;
        }

        //charging only changes x distance
        //constant angle and launch dir
        Debug.Log(countAtHit + " cah "+ shotCount+" so");
        if (countAtHit + 1 == shotCount && collision)
        {
            Debug.Log(countAtHit + "supposed debug");
            Debug.Log(lastCup.gameObject.name + " cup");
            StartCoroutine(intScript.DestroyObj(lastCup, 1f));
            
        }
    }
    void Throw(float force, float angle, GameObject o, Vector3 shootDir)
    {
        Rigidbody rb = o.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
        rb.velocity = shootDir;
        canThrow = true;
        newBall = false;
        ballInHand = false;

    }
    public GameObject createBall()
    {
        currentBall = Instantiate(ball, transform.position, Quaternion.identity);
        Rigidbody rb = currentBall.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
        canThrow = false;
        newBall = true;
        ballInHand = true;
        return currentBall;

    }

}
