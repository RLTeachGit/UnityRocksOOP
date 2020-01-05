using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UpdateScore : MonoBehaviour
{
    Text mText; //Variable to store (cache) Text Component
    void Start()
    {
        mText = GetComponent<Text>(); //Get Text component from game Object
        Debug.Assert(mText != null, "You are missing the Text Component"); //Error Message
    }
    void Update()
    {
        mText.text = string.Format("Score:{0}", GM.sSingleton.Score); //Get Score from GM
    }
}
