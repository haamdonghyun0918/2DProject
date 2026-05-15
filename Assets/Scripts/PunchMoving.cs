using UnityEngine;

public class PunchMoving : MonoBehaviour
{
    [SerializeField] private Animator Punch_Entity;

    [SerializeField] private float maxRunSpeed = 10.0f;
    [SerializeField] private float acceleration = 2.0f;
    [SerializeField] private float deceleration = 10.0f;
    [SerializeField] private float jumpForce = 0.5f;
    [SerializeField] private Rigidbody2D rb;

    private float currentSpeed = 0.0f;
    
    private void Update()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.A))
        {
            Punch_Entity.SetTrigger("isAttack");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Punch_Entity.SetTrigger("isJump");
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.Rotate(0, 180f, 0);
        }
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.E))
        {
            currentSpeed += acceleration * Time.deltaTime;

            if (currentSpeed > maxRunSpeed)
            {
                currentSpeed = maxRunSpeed;
            }
        }
        else
        {
            currentSpeed -= deceleration * Time.deltaTime;
            if (currentSpeed < 0)
            {
                currentSpeed = 0;
            }
        }
        Vector2 moveDir = new Vector2(1, 0);
        transform.Translate(moveDir * currentSpeed * Time.deltaTime);

        Punch_Entity.SetFloat("Speed", currentSpeed);
    }
}