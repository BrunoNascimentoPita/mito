using UnityEngine;

public class playerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidade de movimento do jogador
    public float jumpForce = 5f; // Força do pulo
    public Transform groundCheck; // Objeto usado para verificar se o jogador está no chão
    public LayerMask groundLayer; // Layer que representa o chão
    public GameObject attackArea; // Objeto da área de ataque
    public Animator animator; // Componente Animator do jogador

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool canAttack = true;
    private bool isFacingRight = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Verificar se o jogador está no chão
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Movimento horizontal
        float moveX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        // Verificar a direção do movimento
        if (moveX > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveX < 0 && isFacingRight)
        {
            Flip();
        }

        // Iniciar pulo
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            animator.SetTrigger("Jump");
        }

        // Verificar se o jogador retornou ao chão
        if (isGrounded)
        {
            animator.SetBool("IsJumping", false);
        }

        // Ataque
        if (Input.GetKeyDown(KeyCode.J) && canAttack)
        {
            Attack();
        }
    }

    private void Attack()
    {
        canAttack = false;
        attackArea.SetActive(true);
        animator.SetTrigger("Attack");
        animator.SetBool("IsAttacking", true);

        Invoke("ResetAttack", 0.1f);
    }

    private void ResetAttack()
    {
        attackArea.SetActive(false);
        animator.SetBool("IsAttacking", false);
        canAttack = true;
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}
