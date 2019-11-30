using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    float BulletSpeed=5.0f; //Can be set in IDE
    void Update()
    {
        if(Input.GetButton("Fire1")) {
            BulletFP tBullet = GM.SpawnPrefab(GM.SpawnIDs.Bullet, transform.position, 0).GetComponent<BulletFP>();
            FakePhysics tParentFP = GetComponentInParent<FakePhysics>();
            tBullet.mVelocity = (Vector2)transform.up * BulletSpeed;  //Initial Velocity
            tBullet.mVelocity += tParentFP.mVelocity; //Player ship velocity
        }
    }
}


