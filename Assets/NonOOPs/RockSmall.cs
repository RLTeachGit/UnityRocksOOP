using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSmall : MonoBehaviour
{

    [SerializeField] //Show in IDE
    [Range(0.5f, 5f)]
    private float MaxSpeed = 5.0f;

    [SerializeField] //Show in IDE
    private float MaxRotation = 360.0f;

    private Vector2 mVelocity = Vector2.zero;

    private Collider2D mCollider = null;        //Added in IDE but found with code
    private Rigidbody2D mRB2 = null;    //We will set this up in code
    private float mRotation = 0.0f;

    void Start()
    {

        mCollider = GetComponent<Collider2D>(); //Get BoxCollider2 added via IDE
        Debug.Assert(mCollider != null, "Please assign BoxCollider2D in IDE");

        mCollider.isTrigger = true;      //Set this in code in case we forget in IDE

        mRB2 = gameObject.AddComponent<Rigidbody2D>(); //Use code to add a component
        Debug.Assert(mRB2 != null, "Could not make RB2, make sure you dont have one already attached"); //Error
        mRB2.isKinematic = true;    //Set it to Kinematic in code

        mVelocity = new Vector2(Random.Range(-5.0f, 5.0f), Random.Range(-1.0f, 1.0f));
        mRotation = Random.Range(-360.0f, 360.0f);
    }


    //Wrap Go position
    Vector2 WrappedPosition(Vector2 tCurrentPosition)
    {
        float tYEdge = Camera.main.orthographicSize;    //Get visible worldspace, this only works for orthographic Cameras
        float tXEdge = tYEdge * Camera.main.aspect; //Calculate with from Height
        Vector2 tPosition = tCurrentPosition;
        if (tCurrentPosition.x > tXEdge)
        { //Check for Edge
            tCurrentPosition.x -= 2 * tXEdge; //If beyond , move it back to opposite side
        }
        else if (tCurrentPosition.x < -tXEdge)
        {
            tCurrentPosition.x += 2 * tXEdge;
        }
        if (tCurrentPosition.y > tYEdge)
        {
            tCurrentPosition.y -= 2 * tYEdge;
        }
        else if (tCurrentPosition.y < -tYEdge)
        {
            tCurrentPosition.y += 2 * tYEdge;
        }
        return tCurrentPosition;
    }

    void MovePlayer(Vector2 vMoveInput)
    {
        transform.Rotate(0, 0, mRotation * Time.deltaTime);

        //Rotate ship on player command, note axis reversed
        Vector2 tForce = transform.up;  //Up vector is now direction we are pointing in
        tForce *= MaxSpeed * vMoveInput.y; //Apply User input & MaxSpeed
        mVelocity += tForce;    //Calculate new velocity, scale for time
        transform.position += (Vector3)mVelocity * Time.deltaTime;  //transform is 3D, but we are 2D
        transform.position = WrappedPosition(transform.position); //Keep player on screen
    }

    //Whilst we are using RB's we will do all the movement , as we don't use Physics we dont need FixedUpdate
    void Update()
    {
        Vector2 tMoveInput = Vector2.zero;
        MovePlayer(tMoveInput); //Move Player
    }
}