using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFP : FakePhysics
{
    protected override void DoMove() {
        Vector2 tMoveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); //Get User input

        transform.Rotate(0, 0, -tMoveInput.x * MaxRotation * Time.deltaTime);

        //Rotate ship on player command, note axis reversed
        Vector2 tForce = transform.up;  //Up vector is now direction we are pointing in
        tForce *= MaxSpeed * tMoveInput.y; //Apply User input & MaxSpeed
        mVelocity += tForce;    //Calculate new velocity, scale for time
        base.DoMove(); //Call base class to update position
    }

    protected override void CollidedWith(FakePhysics vOtherFF) {
        Debug.LogFormat("Player hit by {0}", vOtherFF.name); //Player specifc code
        //We do not call parent as we will handle
    }
}
