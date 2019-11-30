using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePhysics : MonoBehaviour
{

    [SerializeField] //Show in IDE
    [Range(0.5f, 5f)]
    protected float MaxSpeed = 5.0f;

    [SerializeField] //Show in IDE
    protected float MaxRotation = 360.0f; //Now protected

    public Vector2 mVelocity = Vector2.zero; //Now public as we need to use it in Gun

    private Collider2D mCollider = null;        //Added in IDE but found with code
    private Rigidbody2D mRB2 = null;    //We will set this up in code

    protected virtual void Start() {    //Now a virtual function

        mCollider = GetComponent<Collider2D>(); //Get BoxCollider2 added via IDE
        Debug.Assert(mCollider != null, "Please assign a Collider2D in IDE");

        mCollider.isTrigger = true;      //Set this in code in case we forget in IDE

        mRB2 = gameObject.AddComponent<Rigidbody2D>(); //Use code to add a component
        Debug.Assert(mRB2 != null, "Could not make RB2, make sure you dont have one already attached"); //Error
        mRB2.isKinematic = true;    //Set it to Kinematic in code
    }


    //Wrap Go position
    protected Vector2 WrappedPosition(Vector2 tCurrentPosition) {
        float tYEdge = Camera.main.orthographicSize;    //Get visible worldspace, this only works for orthographic Cameras
        float tXEdge = tYEdge * Camera.main.aspect; //Calculate with from Height
        Vector2 tPosition = tCurrentPosition;
        if (tCurrentPosition.x > tXEdge) { //Check for Edge
            tCurrentPosition.x -= 2 * tXEdge; //If beyond , move it back to opposite side
        } else if (tCurrentPosition.x < -tXEdge) {
            tCurrentPosition.x += 2 * tXEdge;
        }
        if (tCurrentPosition.y > tYEdge) {
            tCurrentPosition.y -= 2 * tYEdge;
        } else if (tCurrentPosition.y < -tYEdge) {
            tCurrentPosition.y += 2 * tYEdge;
        }
        return tCurrentPosition;
    }

    protected virtual void DoMove() {
        transform.position += (Vector3)mVelocity * Time.deltaTime;  //transform is 3D, but we are 2D
        transform.position = WrappedPosition(transform.position); //Keep player on screen
    }

    //Whilst we are using RB's we will do all the movement , as we don't use Physics we dont need FixedUpdate
    void Update() {
        DoMove(); //Move Object
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        FakePhysics tFF = collision.GetComponent<FakePhysics>(); //Get FakePhysics component
        Debug.AssertFormat(tFF != null, "Other object {0} is not FakePhysics",collision.name); //Ensure its valid
        CollidedWith(tFF);  //Pass to collision handler
    }

    //Default Collision handler
    protected virtual void CollidedWith(FakePhysics vOtherFF) {
        Debug.LogFormat("Collision between {0} and {1}", name, vOtherFF.name); //Print Message
    }


    //This may be useful for other children, so add to base class
    protected Vector2 ClampVelocity(Vector2 vVelocity, float vMaxSpeed) {
        float tSpeed = vVelocity.magnitude; //Lenght of Velocity vector is the speed
        if (tSpeed > vMaxSpeed) //Are we speeding?
        {
            vVelocity *= vMaxSpeed / tSpeed; //Slow down, by amount of overspeed
        }
        return vVelocity;
    }
}
