using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public int number_of_bullets = 100;

    [SerializeField]
    private Sprite[] _idleSprites;

    [SerializeField]
    private Sprite[] _runSprites;

    [SerializeField]
    private Sprite[] _jumpSprites;

    [SerializeField]
    private Sprite[] _currentAnimation;

    SpriteRenderer sr;

    private float speed = 0.005f;

    private int characterState = 0; // 0: idle   1: run

    public float FramesPerSecond = 20;

    private Camera playerCamera;

    private Rigidbody2D rb2d;

    private float delay = 0.0f;

    private float lastY;

    private bool isJumping;

    private bool isFalling;

    public GameObject Player;

    public Text PointText;

    public Text HighscoreText;

    public float peakPlayerPosition;

    int points;

    int highscore;

    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        Jump();

        //This value grabs the saved integer and sets it to the value of highscore when player runs the game !!!FOR THE PERSON WHO IS WORKING ON THE MENU!!! This is the stored highscore use this variable when showing the highscore on the menu.
        highscore = PlayerPrefs.GetInt("highScore", 0);
        //Sets the PointsText to the converted points integer
        PointText.text = "Points: " + points.ToString();
        //Sets the HighscoreText to the converted highscore integer
        HighscoreText.text = "Highscore: " + highscore.ToString();
        peakPlayerPosition = 0;
    }

    void Update()
    {
        UpdateJumpControls();
        playerCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        
        playerCamera.transform.position = new Vector3(this.transform.position.x,
        this.transform.position.y, playerCamera.transform.position.z);
        
        float horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput != 0)
        {
            characterState = 1;
            transform.Translate(new Vector3(speed * horizontalInput, 0f, 0f));
        }

        if (isJumping)
        {
            sr.sprite = _jumpSprites[1];
            
            //When peakPlayerPosition is not less than Player's Y Value it will set a new peakPlayerPosition. The player will go reach a height, if the player falls below that height the condition will not be met. Therefore it will not add points. Only when the player reaches above the peakPlayerPosition will it add points.
            if(peakPlayerPosition ! < Player.transform.position.y)
            {
            peakPlayerPosition = transform.position.y;  
                points += 1;
                PointText.text = "Points: " + points.ToString();
            }

            //This will set & save a new highscore if the current highscore is less than points
            if(highscore < points)
                {   
                    //PlayerPrefs will save the value points into the value to "highScore"
                    PlayerPrefs.SetInt("highScore", points);
                }

        }

        if (isFalling)
        {
            sr.sprite = _jumpSprites[2];
        }

        delay += Time.deltaTime;
        lastY = transform.position.y;

        //This is just for testing purpose or maybe even a potential highscore reset button?
        if (Input.GetKeyDown("backspace"))
        {
            PlayerPrefs.SetInt("highScore", 0);
        }
    }

    private void UpdateJumpControls()
    {
        if (transform.position.y < lastY)
        {
            isJumping = false;
            isFalling = true;
        }
    }

    private void Jump()
    {
        isFalling = false;
        isJumping = true;
        rb2d.AddForce(new Vector3(0f, 8f, 0.0f), ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (delay > 0.5f)
        {
            if (collision.gameObject.tag == "Platform")
            {
                Debug.Log("Jump");
                Jump();
            }
            delay = 0.0f;
        }
    }

}
