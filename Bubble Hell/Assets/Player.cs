using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 3, turnSpeed = 200, bubbleDelay = 1f, prisonDuration = 2f;
    [SerializeField] private GameObject bulletPrefab, bubblePrisonPrefab, gameoverSprite, playerWinSprite, explosionGIF;
    private stateManager stateScript; //refer to script called "stateScript"
    private float nextBubble = 0f; //timing when the next bubble bullet shoots out
    private float prisonEnd = 0f; //timing when the bubble prison disappears

    private enum PlayerState //creating a list of states
    {
        ALIVE,
        STUNNED,
        DEAD
    };
    PlayerState myPlayerState; //creating an object of type PlayerState called myPlayerState


    private void Start()
    {
        stateScript = GameObject.FindWithTag("GameManager").gameObject.GetComponent<stateManager>();
    }

    private void Update()
    {
        switch (myPlayerState)
        {
            case PlayerState.ALIVE:
                movement();
                shooting();
                this.transform.GetComponent<CircleCollider2D>().enabled = false; //disable collider
                break;
            case PlayerState.STUNNED:
                bubblePrison();
                this.transform.GetComponent<CircleCollider2D>().enabled = true;
                break;
            case PlayerState.DEAD:
                rip();
                break;
        }
    }

    void movement()
    {
        if (this.name == "Player1")
        {
            //player 1 - WASD to move
            transform.Translate(Input.GetAxis("Vertical2") * Vector3.up * speed * Time.deltaTime);
            //turn
            if (Input.GetKey(KeyCode.A))
                transform.Rotate(0f, 0f, turnSpeed * Time.deltaTime);
            if (Input.GetKey(KeyCode.D))
                transform.Rotate(0f, 0f, -turnSpeed * Time.deltaTime);
        }
        else
        {
            //player 2 - Arrows keys to move
            transform.Translate(Input.GetAxis("Vertical") * Vector3.up * speed * Time.deltaTime);
            //turn
            if (Input.GetKey(KeyCode.LeftArrow))
                transform.Rotate(0f, 0f, turnSpeed * Time.deltaTime);
            if (Input.GetKey(KeyCode.RightArrow))
                transform.Rotate(0f, 0f, -turnSpeed * Time.deltaTime);
        }
    }

    void shooting()
    {
        //player 1 - space to shoot
        //player 2 - ctrl to shoot
        if (this.name == "Player1" && Input.GetKey(KeyCode.Space) ||
            this.name == "Player2" && Input.GetKey(KeyCode.RightControl))
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
        this.transform.Rotate(0f, 0f, 50f);
        

        if (Time.time > prisonEnd)
        {
            bubblePrisonPrefab.SetActive(false);
            myPlayerState = PlayerState.ALIVE;
        }
    }

    void rip()
    {
        //player disappears
        //this.transform.GetComponent<SpriteRenderer>().enabled = false;

        //ui text
        gameoverSprite.SetActive(true);
        playerWinSprite.SetActive(true);
    }
}

