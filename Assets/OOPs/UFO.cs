using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : FakePhysics {

    float mTimeout = 0.0f;

    protected override void DoMove() {
        mTimeout -= Time.deltaTime;
        if (mTimeout <= 0.0f) {
            mVelocity = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized * MaxSpeed; //Initial velocity
            mTimeout = Random.Range(1.0f, 5.0f);
        }

        base.DoMove();
    }

}