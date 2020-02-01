using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Game Manager, should run first so
//Set to -50 in script execution order
public class GM : MonoBehaviour {

//Stop message about variable not being assigned, which is an error, as we assing in IDE
#pragma warning(disable : 0649) 

    public static GM sSingleton; //Make static singleton, this will be shared by all

    public int Score = 0;

    public float Health = 1.0f;

    public int StartingAsteroids = 10;


    //Make the singleton
    void Awake() {
        if(sSingleton==null) {
            sSingleton = this; //If we run the first time keep a reference to self
            DontDestroyOnLoad(gameObject);  //Stop it being unloaded
            GameState = GameStates.Init;    //Init the game with the state machine
            StartCoroutine(GameStateCoRoutine());   //Run state machine CoRoutine
        }  if(sSingleton!=this) { //If we get made a second time, destroy new instance
            Destroy(gameObject); //There can be only one
        }
    }

    //Init Game
    void StartGame() {
        CreatePlayer();
        CreateAsteroids(StartingAsteroids);
        Score = 0;
        Health = 1.0f;
        mPlayerDead = false;
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
        , AsteroidBig //1
        ,Bullet //2
        , Explosion //3
        ,AsteroidMedium //4
        , AsteroidSmall //5
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
            SpawnPrefab(SpawnIDs.AsteroidBig, RandomOnScreenPosition(), Random.Range(0,360)); //Show asteroid with random rotation
        }
    }

    #endregion


    #region StateMachine
    [SerializeField]
    private GameObject GameOverText;    //Set in IDE

    [SerializeField]
    private GameObject PressPlayText;    //Set in IDE

    [SerializeField]
    private GameObject NextLevelText;    //Set in IDE

    [SerializeField]
    private Text DebugText;    //Set in IDE

    bool mPlayerDead = false; //Flag set when player is dead

    float mCountDown = 0;   //Used for delays

    int mLevel = 0;

    public static bool isPlayerDead {
        get {
            return sSingleton.mPlayerDead;
        }
    }

    public enum GameStates
    {   //State the game is in
        None        //Pre start
        , Init
        , Startup
        , PressPlay
        , Play
        , Playing
        , NextLevelText
        , NextLevelPlay
        , GameOver
    }

    GameStates mCurrentState = GameStates.None;    //Pre First State in initialisation
    //State machine handler
    static public GameStates GameState {   //This may call itself recursivly
        private set {
            if (value != sSingleton.mCurrentState)
            {  //Only change state if different from last one, or its first time its used
                sSingleton.ExitState(sSingleton.mCurrentState);  //Exit last state
                GameStates tNextState = sSingleton.EnterState(value); //Enter new state
                if (value == tNextState)
                { //If return state is final state, set it
                    sSingleton.mCurrentState = tNextState;
                }
                else
                {
                    sSingleton.mCurrentState = value;  //State we are in now
                    GameState = tNextState; //If not we need to change state again, until we reach the final one
                }
            }
        }
        get {
            return sSingleton.mCurrentState;   //Get Current State
        }
    }

    //Used to clear up after a state is exited
    private void ExitState(GameStates vState)
    {
        Debug.LogFormat("Exit State {0}", vState);
        switch (vState)
        {
            case GameStates.PressPlay:
                PressPlayText.SetActive(false); //Turn of User Prompt
                break;

            case GameStates.NextLevelText:
                NextLevelText.SetActive(false); //Remove UI
                break;

            default:    //No Action
                break;
        }
    }

    //Used to set up a new state
    private GameStates EnterState(GameStates vState)
    {
        Debug.LogFormat("Enter State {0}", vState);
        switch (vState)
        {
            case GameStates.Init:   //Initialise game, and put up press play
                Debug.Assert(GameOverText != null && PressPlayText != null && NextLevelText != null, "Please set Text fields"); //Ensure Text fields are set
                GameOverText.SetActive(false);  //Turn off all GameState UI Text
                NextLevelText.SetActive(false);
                PressPlayText.SetActive(false);
                GameClear();    //Remove old GameObjects
                return GameStates.PressPlay;    //Also trigger new state on exit

            case GameStates.PressPlay:
                PressPlayText.SetActive(true); //Show Press play
                SpawnAsteroids(StartingAsteroids);
                break;

            case GameStates.Play:
                SpawnPlayer();
                mPlayerDead = false;
                return GameStates.Playing;

            case GameStates.NextLevelText:
                mLevel++;
                mCountDown = 1.0f;
                NextLevelText.SetActive(true); //Show Next Level Message
                break;

            case GameStates.NextLevelPlay:
                SpawnAsteroids(Mathf.Min(mLevel + StartingAsteroids-1, 30));   //More Asteroids each level, but limit at 30
                return GameStates.Playing;

            case GameStates.GameOver:
                GameOverText.SetActive(true);  //Turn Game Over
                break;

            default:    //No Action
                break;
        }
        return vState;  //Default just return state we entered
    }

    //Run this as a CoRoutine every 10th of a second as running it in Update() would be overkill
    IEnumerator GameStateCoRoutine()
    {
        do
        {
            DebugText.text = GameState.ToString();
            switch (GameState)
            {

                case GameStates.PressPlay:
                    if (Input.GetKey(KeyCode.Space))
                    {
                        GameState = GameStates.Play;    //Go to new state
                    }
                    break;

                case GameStates.Playing:
                    {
                        RockBase[] tAsteroids = FindObjectsOfType<RockBase>();  //Get all the asteroids in the scene
                        if (tAsteroids.Length == 0)
                        {
                            GameState = GameStates.NextLevelText;   //Next Level Text
                        }
                    }
                    break;

                case GameStates.NextLevelText:
                    if(mCountDown>0)
                    {
                        mCountDown -= Time.deltaTime;
                        NextLevelText.GetComponent<Text>().text = string.Format("Level {0}\nGet Ready {1:f1}", mLevel + 1,mCountDown);   //Set Level Text
                    }
                    else
                    {
                        GameState = GameStates.NextLevelPlay;    //Play
                    }
                    break;

                case GameStates.GameOver:
                    if (Input.GetKey(KeyCode.Space))
                    {
                        GameState = GameStates.Init;    //Go to new state
                    }
                    break;

                default:    //No Action
                    break;
            }
            yield return new WaitForSeconds(0.1f);  //Wait for a 10th of a second before runnign again, lets other stuff process
        } while (true); //Never End
    }


    static public void InitGame()
    {
        sSingleton.Score = 0;  //Reset Score
        sSingleton.mLevel = 1;  //Start at Level 1
        GameState = GameStates.Init;
    }


    void SpawnPlayer()
    {
        sSingleton.Health = 100.0f;
        SpawnPrefab(SpawnIDs.Player, RandomOnscreenPosition(), 0); //Spawn player
    }

    void SpawnAsteroids(int vCount = 10)
    {
        for (int i = 0; i < vCount; i++)
        {
            SpawnPrefab(SpawnIDs.AsteroidBig, RandomOnscreenPosition(), Random.Range(0.0f,360.0f)); //Spawn Rocks
        }
    }

    void GameClear()
    {
        FakePhysics[] tFFObjects = FindObjectsOfType<FakePhysics>(); //Find all Fake Physics objects in Scene
        foreach (var tFF in tFFObjects)
        {
            Destroy(tFF.gameObject);        //Destroy all current FakePhysics objects
        }
    }

    #region HelperFunctions
    //Moved from FakePhysics and turned into a static function
    public static Vector2 WrappedPosition(Vector2 tCurrentPosition)
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
    public static Vector2 RandomOnscreenPosition()
    {
        float tYEdge = Camera.main.orthographicSize;    //Get visible worldspace, this only works for orthographic Cameras
        float tXEdge = tYEdge * Camera.main.aspect; //Calculate with from Height
        return new Vector2(Random.Range(-tXEdge, tXEdge), Random.Range(-tYEdge, tYEdge)); //Random on screen position
    }

    #endregion



    #endregion
}
