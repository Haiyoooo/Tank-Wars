using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Rotation : MonoBehaviour
{
    void Update()
    {
        //shuriken spinning animation
        transform.Rotate(0f, 0f, 500f * Time.deltaTime);
    }

    private void OnBecameInvisible()
    {
        //destroy shuriken GFX and it's parent when offscreen
        //note: In editor, if scene view is open, object will be visible. Hide scene view.
        Destroy(transform.parent.gameObject);
    }

}
