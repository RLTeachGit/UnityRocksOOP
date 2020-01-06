using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMediumFP : FakePhysics
{

    float mRotation;

    protected override void Start() {
        mVelocity = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized * MaxSpeed*1.5f; //Initial velocity
        mRotation = Random.Range(-4.0f, 4.0f) * 1.5f;
        base.Start(); //Now call parent
    }

    //Rock's own movement
    protected override void DoMove() {
        transform.Rotate(0, 0, -mRotation * MaxRotation * Time.deltaTime);
        base.DoMove();
    }

    protected override void CollidedWith(FakePhysics vOtherFF) {
        if (vOtherFF is BulletFP)
        {  //Were we hit by a bullet
            Destroy(gameObject);    //Destroy Asteroid
            Destroy(vOtherFF.gameObject);    //Destroy Bullet
            GM.SpawnPrefab(GM.SpawnIDs.Explosion, transform.position, Random.Range(-180, 180));// Exposion
            GM.SpawnPrefab(GM.SpawnIDs.AsteroidSmall, transform.position, Random.Range(-180, 180)); //Medium Rock
            GM.SpawnPrefab(GM.SpawnIDs.AsteroidSmall, transform.position, Random.Range(-180, 180)); //Medium Rock
            GM.SpawnPrefab(GM.SpawnIDs.AsteroidSmall, transform.position, Random.Range(-180, 180)); //Medium Rock

            GM.sSingleton.Score += 150; //Give player score
        }
        else if (vOtherFF is PlayerFP)
        {

            Destroy(gameObject);    //Destroy Asteroid
            GM.SpawnPrefab(GM.SpawnIDs.Explosion, transform.position, Random.Range(-180, 180));// Exposion
            GM.SpawnPrefab(GM.SpawnIDs.AsteroidSmall, transform.position, Random.Range(-180, 180)); //Medium Rock
            GM.SpawnPrefab(GM.SpawnIDs.AsteroidSmall, transform.position, Random.Range(-180, 180)); //Medium Rock
            GM.SpawnPrefab(GM.SpawnIDs.AsteroidSmall, transform.position, Random.Range(-180, 180)); //Medium Rock
            GM.sSingleton.Score += 150; //Give less player score for crashing
            GM.sSingleton.Health = Mathf.Clamp(GM.sSingleton.Health - 0.025f, 0.0f, 1.0f);  //Reduce Health, but clamp
        }
    }

}
