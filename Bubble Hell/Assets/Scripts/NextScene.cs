using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{

    private void Update()
    {
        //either player can start the game using X or CTRL
        if(Input.GetButton("p1_Fire1") || Input.GetButton("p2_Fire1"))
        {
            LevelScene();
        }
    }

    public void LevelScene()
    {
        SceneManager.LoadScene("Level", LoadSceneMode.Single);
        
    }
}
