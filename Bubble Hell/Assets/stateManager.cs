using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class stateManager : MonoBehaviour
{

    public enum state { START, RUNNING, GAMEOVER };
    public state gameState;
    // Start is called before the first frame update
    void Start()
    {
        gameState.Equals(state.START);
        Debug.Log("Gamestate = " + gameState);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            gameState++;
            Debug.Log("Gamestate = " + gameState);
        }
            
    }

    
}
