using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 3f; // Velocidade de movimento do inimigo
    public Transform groundCheck; // Objeto usado para verificar se o inimigo está no chão
    public LayerMask groundLayer; // Layer que representa o chão
    public GameObject wallObject; // Objeto de parede que o inimigo irá verificar a colisão
    public float knockbackForce = 5f; // Força do knockback

    private Rigidbody2D rb;
    private bool isFacingRight = true;
    private bool isMovingRight = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Verificar se o inimigo está no chão
        bool isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.1f, groundLayer);

        // Movimento horizontal
        if (isMovingRight)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }

        // Verificar se o inimigo está colidindo com a parede
        bool isTouchingWall = Physics2D.Raycast(wallObject.transform.position, transform.right, 0.1f);

        // Verificar a direção do movimento
        if (isTouchingWall || !isGrounded)
        {
            Flip();
            isMovingRight = !isMovingRight;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Verificar se o inimigo está colidindo com o jogador
        if (collision.gameObject.CompareTag("Player"))
        {
            // Reduzir a vida do jogador
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.TakeDamage(1);

            // Empurrar o jogador na direção oposta ao flip do inimigo
            player.Knockback(-transform.localScale.x);
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}
