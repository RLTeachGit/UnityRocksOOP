using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBigFP : FakePhysics {
    // Start is called before the first frame update

    float mRotation;

    protected override void Start() {
        mVelocity = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized*MaxSpeed; //Initial velocity
        mRotation = Random.Range(-4.0f, 4.0f);
        base.Start(); //Now call parent
    }

    //Rock's own movement
    protected override void DoMove() {
        transform.Rotate(0, 0, -mRotation * MaxRotation * Time.deltaTime);
        base.DoMove();
    }

    protected override void CollidedWith(FakePhysics vOtherFF) {
        Debug.LogFormat("RockBigFP hit by {0}", vOtherFF.name); //RockBigFP specifc code
        //We do not call parent as we will handle
    }

}
