using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 3f;

    private Rigidbody2D rb;
    private bool movingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(movingRight ? moveSpeed : -moveSpeed, rb.velocity.y);
    }

    private void Flip()
    {
        movingRight = !movingRight;
        transform.localScale = new Vector3(movingRight ? 1 : -1, 1, 1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 壁に当たったら反転
        if (collision.gameObject.CompareTag("Wall"))
        {
            Flip();
            return;
        }

        // プレイヤーに当たった場合
        if (!collision.gameObject.CompareTag("Player"))
            return;

        foreach (ContactPoint2D point in collision.contacts)
        {
            if (point.normal.y < -0.5f)
            {
                Destroy(gameObject);
                collision.gameObject
                    .GetComponent<Rigidbody2D>()
                    .AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
                return;
            }
        }

        GameManager.Instance.GameOver();
    }
}
