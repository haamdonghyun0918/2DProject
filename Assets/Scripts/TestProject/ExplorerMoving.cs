using System.Collections;
using UnityEngine;

public class ExplorerMoving : MonoBehaviour
{
    [SerializeField] private Animator Explorer_Entity;

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
            Explorer_Entity.SetTrigger("isAttack");
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Explorer_Entity.SetTrigger("isJump");
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

        Explorer_Entity.SetFloat("Speed", currentSpeed);
    }
    private void Attack()
    {
        Vector2 attackPoint = (Vector2)transform.position + ((Vector2)transform.right * 0.8f) + (Vector2.up * 0.8f);
        float attackRange = 1.0f;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint, attackRange);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject == gameObject) continue;
            if (enemy.CompareTag("Monster"))
            {
                Monster monster = enemy.GetComponent<Monster>();
                if (monster != null)
                {
                    monster.TakeDamage();
                    Debug.Log("몬스터를 공격하였습니다!");
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap") || collision.gameObject.CompareTag("Monster"))
        {
            Explorer_Entity.SetBool("isDamaged", true);
            StartCoroutine(CoResetDamageAnim());
        }
    }

    IEnumerator CoResetDamageAnim()
    {
        yield return new WaitForSeconds(0.5f);
        Explorer_Entity.SetBool("isDamaged", false);
    }
}