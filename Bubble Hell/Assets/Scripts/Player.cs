using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 3, bubbleDelay = 1f, prisonDuration = 2f;
    [SerializeField] private GameObject bulletPrefab, bubblePrisonPrefab, gameoverSprite, playerWinSprite, explosionGIF, canvas;
    private float nextBubble = 0f; //timing when the next bubble bullet shoots out
    private float prisonEnd = 0f; //timing when the bubble prison disappears
    private Vector3 input = new Vector3(0, 0, 0);
    private Vector3 inputshoot = new Vector3(0, 0, 0);
    private enum PlayerState { ALIVE, STUNNED, DEAD }; //possible statess
    private PlayerState myPlayerState; //my current state
    private Camera sceneCam;


    private void Start()
    {
        sceneCam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    private void Update()
    {
        switch (myPlayerState)
        {
            case PlayerState.ALIVE:
                movement();
                shooting();
                wrap();
                break;
            case PlayerState.STUNNED:
                bubblePrison();
                wrap();
                break;
            case PlayerState.DEAD:
                rip();
                break;
        }
    }

    void movement()
    {
        //player 1 - WASD to move, or JoyCon joystick
        //player 2 - arrowkeys to move, or JoyCon joystick
        if (this.name == "Player1")
        {
            input = new Vector3(Input.GetAxis("p1_Horizontal"), Input.GetAxis("p1_Vertical"), 0f);
        }
        else
        {
            input = new Vector3(Input.GetAxis("p2_Horizontal"), Input.GetAxis("p2_Vertical"), 0f);
        }

        //movement in worldspace
        //eg. right key always moves player towards screen's right, regardless of which way the player sprite is currently facing
        transform.Translate(input * speed * Time.deltaTime, Space.World);

        //rotate player to face movement direction
        //credit https://answers.unity.com/questions/1387259/joystick-character-movement.html
        if (input != Vector3.zero)
        {
            transform.up = input;
        }
    }

    void shooting()
    {
        //player 1 - left ctrl to shoot, or JoyCon X
        //player 2 - right ctrl to shoot, or JoyCon X
        if (this.name == "Player1" && Input.GetButton("p1_Fire1") ||
            this.name == "Player2" && Input.GetButton("p2_Fire1"))
        {
            if (Time.time > nextBubble)
            {
                nextBubble = Time.time + bubbleDelay;
                Instantiate(bulletPrefab, transform.position, transform.rotation);
                //bubbleAudio.Play();
            }
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        //_____________Player killed by shuriken_____________
        if (other.gameObject.tag == "Obstacle")
        {
            if (myPlayerState == PlayerState.ALIVE)
            {
                Instantiate(explosionGIF, this.transform.position, this.transform.rotation); //explosion
            }
            this.transform.GetComponent<PolygonCollider2D>().enabled = false; //disable collider
            myPlayerState = PlayerState.DEAD;
        }

        //_____________Player trapped by opponent's bubble_____________
        if (this.name == "Player1" && other.gameObject.tag == "BubbleP2" ||
           this.name == "Player2" && other.gameObject.tag == "BubbleP1")
        {
            Destroy(other.gameObject); //destroy bullet
            //var tempBubblePrison = Instantiate(bubblePrisonPrefab, this.transform.position, Quaternion.identity); //bubble prision
            //tempBubblePrison.transform.parent = this.gameObject.transform; //as child of player
            bubblePrisonPrefab.SetActive(true); //bubble prison

            prisonEnd = Time.time + prisonDuration;
            myPlayerState = PlayerState.STUNNED;
        }
    }

    private void bubblePrison()
    {
        //rotate
        this.transform.Rotate(0f, 0f, 50f); //spin the player round and round
        this.transform.GetComponent<CircleCollider2D>().enabled = true; //enable bubble prison's collider

        if (Time.time > prisonEnd)
        {
            bubblePrisonPrefab.SetActive(false); //disable bubble prison's image
            this.transform.GetComponent<CircleCollider2D>().enabled = false; //disable bubble prison's collider
            myPlayerState = PlayerState.ALIVE;
        }
    }

    void rip()
    {
        //ui text
        gameoverSprite.SetActive(true);
        playerWinSprite.SetActive(true);
        canvas.SetActive(true);

        //reset game by reloading this scene
        if (Input.GetButton("p1_Fire1") || Input.GetButton("p2_Fire1"))
        {
            SceneManager.LoadScene("Level", LoadSceneMode.Single);
        }
    }

    void wrap()
    {
        Vector2 normPos2D = sceneCam.WorldToViewportPoint(this.transform.position);  // 0.5 = screen center
        Vector2 newPos = this.transform.position; //created to manipulate position vector because can't do: transform.position.x *= -1

        //if player sprite goes offscreen, wrap round
        if(normPos2D.x < 0 || normPos2D.x > 1)
        {
            newPos.x *= -0.99f; //not -1 because there'll be infinite wrap bug
        }
        if(normPos2D.y < 0 || normPos2D.y > 1)
        {
            newPos.y *= -0.99f;
        }
        this.transform.position = newPos;
    }
}

