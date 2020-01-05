using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateHealth : MonoBehaviour
{

    RectTransform mImageRT; //Cached Rect Transform Component

    // Start is called before the first frame update
    void Start()
    {
        mImageRT = GetComponent<RectTransform>();    //Get Image Component
        Debug.Assert(mImageRT != null, "No RectTransform component found");
    }

    void    UpdateImage(float vHealth)
    {
        Vector3 tScale = mImageRT.localScale;
        tScale.x = Mathf.Clamp(vHealth, 0.0f, 1.0f); //Keep it between 0-1
        mImageRT.localScale = tScale;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateImage(GM.sSingleton.Health);
    }
}
