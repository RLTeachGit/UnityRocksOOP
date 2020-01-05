using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Game Manager, should run first so
//Set to -50 in script execution order
public class GM : MonoBehaviour {

//Stop message about variable not being assigned, which is an error, as we assing in IDE
#pragma warning(disable : 0649) 

    public static GM sSingleton; //Make static singleton, this will be shared by all

    public int Score = 0;

    public float Health = 1.0f;

    //Make the singleton
    void Awake() {
        if(sSingleton==null) {
            sSingleton = this; //If we run the first time keep a reference to self
            DontDestroyOnLoad(gameObject);  //Stop it being unloaded
            StartGame();
        }  if(sSingleton!=this) { //If we get made a second time, destroy new instance
            Destroy(gameObject); //There can be only one
        }
    }

    //Init Game
    void StartGame() {
        CreatePlayer();
        CreateAsteroids(10);
        Score = 0;
        Health = 1.0f;
    }

    #region Spawning

    //Helper function to make random on screen position
    public  static    Vector2 RandomOnScreenPosition() {
        float tYEdge = Camera.main.orthographicSize;    //Get visible worldspace, this only works for orthographic Cameras
        float tXEdge = tYEdge * Camera.main.aspect; //Calculate with from Height
        return new Vector2(Random.Range(-tXEdge, tXEdge), Random.Range(-tYEdge, tYEdge));
    }

    [SerializeField]
    private GameObject[] SpawnablePrefabs; //Prefabs we will be able to spawn

    public enum SpawnIDs { //Just makes up nice labels starting from 0
        Player      //0
        , AsteriodBig //1
        ,Bullet //2
        , Explosion //3
        ,AsteroidMedium //4
    }
    //Function we can use to spawn Objects
    public static GameObject SpawnPrefab(SpawnIDs vID, Vector2 vLocation, float vAngle) {
        int tIndex = (int)vID; //Turn ID into index by casting to int
        Debug.AssertFormat(tIndex < sSingleton.SpawnablePrefabs.Length, "SpawnID {0} not in Array, did you forget to add prefab?", vID); //Error
        Quaternion tAngle = Quaternion.Euler(0, 0, vAngle); //Make angle something Unity understands
        return Instantiate(sSingleton.SpawnablePrefabs[tIndex],vLocation,tAngle); //Create it
    }

    void CreatePlayer() {
        SpawnPrefab(SpawnIDs.Player, RandomOnScreenPosition(), 0); //Show player
    }


    void CreateAsteroids(int vCount) {
        while(vCount-- !=0) { //Make as many asteroids as needed
            SpawnPrefab(SpawnIDs.AsteriodBig, RandomOnScreenPosition(), Random.Range(0,360)); //Show asteroid with random rotation
        }
    }

    #endregion
}
