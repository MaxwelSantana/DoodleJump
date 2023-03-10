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

    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        Jump();
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
        }

        if (isFalling)
        {
            sr.sprite = _jumpSprites[2];
        }

        delay += Time.deltaTime;
        lastY = transform.position.y;
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
