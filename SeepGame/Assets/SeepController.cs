using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeepController : MonoBehaviour
{
    [SerializeField, Tooltip("ˆÚ“®‘¬“x")] float speed = 3f;
    [SerializeField, Tooltip("ƒWƒƒƒ“ƒv—Í")] float jumpPower = 3f;

    Vector2 direction = default;
    Rigidbody2D _rb2D = default;

    [SerializeField]Kind kind = Kind.Normal;
    float boundTime = 0;
    int boundCount = 0 ;
    bool isGorounded = true;
    bool isBound = true;
    bool isGoal = false;
    bool isJump = false;

    float tempSpeed = 0;
    GameManager gameManager = null;
    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();

        if(kind == Kind.Normal)
        {
            speed = 3f;
        }
        else if(kind == Kind.Slow)
        {
            speed /= 2;
        }
        else if(kind == Kind.Fast)
        {
            speed *= 2;
        }

        direction = Vector2.right.normalized * speed;
        
    }

    void Update()
    {
        if(kind == Kind.Bound && isBound == true && isGorounded == true)
        {
            boundTime += Time.deltaTime;
        }

        float verticalVelocity = _rb2D.velocity.y;

        if (isGorounded == true)
        {

            if (kind == Kind.Bound && boundTime > 1.0f && boundCount < 2 && isGoal == false && isJump == false)
            {
                isBound= false;
                isGorounded = false;
                direction *= -1;
                _rb2D.velocity = direction + Vector2.up * verticalVelocity;
                _rb2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

            }
            else
            {
                _rb2D.velocity = direction + Vector2.up * verticalVelocity;
            }

            if (Input.GetKeyDown(KeyCode.Space) && isJump == true)
            {
                tempSpeed = speed;
                speed = 4.5f;
                direction = Vector2.left.normalized * speed;
                if (direction.x < 0) direction *= -1;
                _rb2D.AddForce(Vector2.up * jumpPower * 3, ForceMode2D.Impulse);
                isJump = false;
                isGorounded = false;
            }
        }
        else 
        {
            _rb2D.velocity = direction + Vector2.up * verticalVelocity;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            if (isBound == false)
            {
                boundCount++;
                if(boundCount > 1)
                {
                    isBound = true;
                    boundCount = 0;
                    boundTime = 0;

                }
            }
            isGorounded = true;
            if(isGoal)
            {
                speed = tempSpeed;
            }
        }

        if(collision.gameObject.name == "Fence" )
        {
            gameManager.AddCombo(false);
            Destroy(gameObject);
        }
        if(collision.gameObject.name == "DestroyArea")
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            isGoal = true;
            gameManager.AddScore();
            gameManager.AddCombo(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Jump" && isGorounded == true)
        {
            Debug.Log("a");
            isJump = true;
        }
    }

    public void SetKind(Kind k)
    {
        kind = k;
    }

    public enum Kind
    {
        Normal,
        Slow,
        Fast,
        Bound,
    }
}
