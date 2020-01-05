using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Allow us to talk to UI
public class UpdateText : MonoBehaviour
{
    Text mText; //Variable to store (cache) Text Component
    void Start()
    {
        mText = GetComponent<Text>(); //Get Text component from game Object
        Debug.Assert(mText != null, "You are missing the Text Component"); //Error Message
    }

    void Update()
    {
        mText.text = string.Format("{0}", Random.Range(0, 100)); //Just print some random number 0-99 to test
    }
}



