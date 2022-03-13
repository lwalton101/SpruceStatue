using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb2D;
    private Collider2D coll;
    private SpriteRenderer sr;

    [Header("Assaignables")]
    public float movementSpeed = 30f;
    public float jumpForce = 30f;
    [Range(0f, 1f)]
    public float jumpCutoffTime = 0.5f;
    [SerializeField] private LayerMask jumpableGround;

    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float coyoteTimeCounter;

    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float jumpBufferCounter;

    [SerializeField] private KeyBindingsManager keyBindingsManager;
    [SerializeField] private GameManager gameManager;

    // Start is called before the first frame update
    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: Put into own function
        //This gets horizontal movement and applies it to player
        float horiziontal = Input.GetAxisRaw("Horizontal");
        Vector2 vec2 = new Vector2(horiziontal * movementSpeed, rb2D.velocity.y);
        rb2D.velocity = vec2;
        
		if (IsGrounded())
		{
            coyoteTimeCounter = coyoteTime;
		}
		else
		{
            coyoteTimeCounter -= Time.deltaTime;
		}

		if (Input.GetButtonDown("Jump"))
		{
            jumpBufferCounter = jumpBufferTime;
		}
		else
		{
            jumpBufferCounter -= Time.deltaTime;
		}

        //Add initial jump force
		if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
		{
            //Debug.Log(jumpBufferCounter + " " + coyoteTimeCounter);
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);

            jumpBufferCounter = 0f;
        } 

        //When key is released multiply y velocity by amount
        if(Input.GetButtonUp("Jump") && rb2D.velocity.y > 0f)
		{
            rb2D.velocity = new Vector2(rb2D.velocity.x, rb2D.velocity.y * jumpCutoffTime);

            coyoteTimeCounter = 0f;
        }

        if(rb2D.velocity.x < 0f)
		{
            sr.flipX = true;
		}
		else if(rb2D.velocity.x > 0f)
		{
            sr.flipX = false;
		}
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        Debug.Log(collision);
		if (collision.gameObject.CompareTag("Coin"))
		{
            gameManager.AddCoinAmount(1);
            Destroy(collision.gameObject);
		}
	}
}
