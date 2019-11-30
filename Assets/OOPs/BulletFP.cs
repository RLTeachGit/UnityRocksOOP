using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFP : FakePhysics
{
    protected override void Start() { //Start Bullet
        base.Start();
        Destroy(gameObject, 2.0f);      //Destroy bullet after 2 seconds
    }
}


