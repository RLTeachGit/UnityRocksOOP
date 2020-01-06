using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSmallFP : FakePhysics
{
    float mRotation;

    protected override void Start() {
        mVelocity = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized * MaxSpeed * 1.5f * 1.5f; //Initial velocity
        mRotation = Random.Range(-4.0f, 4.0f) * 1.5f * 1.5f;
        base.Start(); //Now call parent
    }

    //Rock's own movement
    protected override void DoMove() {
        transform.Rotate(0, 0, -mRotation * MaxRotation * Time.deltaTime);
        base.DoMove();
    }

    protected override void CollidedWith(FakePhysics vOtherFF) {
        Debug.LogFormat("RockSmall hit by {0}", vOtherFF.name); //RockMedium specifc code
        //We do not call parent as we will handle
        if (vOtherFF is BulletFP)
        {  //Were we hit by a bullet
            Destroy(gameObject);    //Destroy Asteroid
            Destroy(vOtherFF.gameObject);    //Destroy Bullet
            GM.SpawnPrefab(GM.SpawnIDs.Explosion, transform.position, Random.Range(-180, 180));// Exposion

            GM.sSingleton.Score += 200; //Give player score
        }
        else if (vOtherFF is PlayerFP)
        {

            Destroy(gameObject);    //Destroy Asteroid
            GM.SpawnPrefab(GM.SpawnIDs.Explosion, transform.position, Random.Range(-180, 180));// Exposion
            GM.sSingleton.Score += 20; //Give less player score for crashing
            GM.sSingleton.Health = Mathf.Clamp(GM.sSingleton.Health - 0.015f, 0.0f, 1.0f);  //Reduce Health, but clamp
        }
    }
}
