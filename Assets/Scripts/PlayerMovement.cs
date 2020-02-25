using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 7f;
    public bool isOnGround = false;
    public float jumpForce = 9.5f;
    public float footOffset = 0.24f;
    public float groundDistance = 0.2f;
    public float pivotOffset = -0.39f;
    public float airInclineFactor = 5;
    public LayerMask groundLayer;

    private Animator animator;
    private Rigidbody2D rigid2D;

    private int direction = 1;
    [Header("Input Status")]
    public float horizontal;      //Float that stores horizontal input
    public bool jumpPressed;      //Bool that stores jump held
    public bool attackHeld;       //Bool that stores attack pressed

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigid2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        jumpPressed = jumpPressed || Input.GetButtonDown("Jump");
        attackHeld = Input.GetButton("Fire1");
        animator.SetFloat("speed", Mathf.Abs(horizontal));
        animator.SetBool("isAttack", attackHeld);
        animator.SetBool("isGround", isOnGround);
    }

    private void FixedUpdate()
    {
        GroundCheck();
        GroundMovement();
        MidAirMovement();
        jumpPressed = false;
    }

    private void GroundCheck()
    {
        isOnGround = false;
        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, pivotOffset), Vector2.down, groundDistance);
        RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, pivotOffset), Vector2.down, groundDistance);
        if (leftCheck || rightCheck)
        {
            isOnGround = true;
        }
    }

    private void GroundMovement()
    {
        if (isOnGround)
        {
            var xVelocity = moveSpeed * horizontal;
            rigid2D.velocity = new Vector2(xVelocity, rigid2D.velocity.y);
        }
        if (horizontal * direction < 0)
            Flip();
    }

    private void Flip()
    {
        direction *= -1;
        transform.eulerAngles = direction == 1 ? Vector3.zero : new Vector3(0, 180, 0);
    }

    private void MidAirMovement()
    {
        if (jumpPressed && isOnGround)
        {
            isOnGround = false;
            rigid2D.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
        else if (!isOnGround)
        {
            rigid2D.velocity += Vector2.right * horizontal * airInclineFactor * Time.deltaTime;
        }
    }

    private RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, groundLayer);
        Color color = hit ? Color.red : Color.green;
        //...and draw the ray in the scene view
        Debug.DrawRay(pos + offset, rayDirection * length, color);
        return hit;
    }
}
