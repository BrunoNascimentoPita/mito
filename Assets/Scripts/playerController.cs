using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidade de movimento do jogador
    public float jumpForce = 5f; // Força do pulo
    public Transform groundCheck; // Objeto usado para verificar se o jogador está no chão
    public LayerMask groundLayer; // Layer que representa o chão
    public GameObject attackArea; // Objeto da área de ataque
    public Animator animator; // Componente Animator do jogador
    public Image healthBar; // Imagem do medidor de vida
    public float healthUpdateSpeed = 1f; // Velocidade de atualização do medidor de vida

    public int maxHealth = 1000; // Vida máxima do jogador
    private int currentHealth; // Vida atual do jogador

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool canAttack = true;
    private bool isFacingRight = true;

    public GameObject gameOverPanel;
    public GameObject playerObject;

    public Image damageImage; // Referência à imagem de aviso de dano
    public float damageImageDuration = 0.1f; // Duração da exibição da imagem de aviso de dano

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
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
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
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
        if ((Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Alpha4)) && canAttack)
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

        // Detectar colisão com inimigos na área de ataque
        Collider2D[] enemies = Physics2D.OverlapBoxAll(attackArea.transform.position, attackArea.transform.localScale, 0f);

        // Aplicar dano aos inimigos atingidos
        foreach (Collider2D enemy in enemies)
        {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.TakeDamage(50);
            }
        }

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

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Verificar se o jogador está morto
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            gameOverPanel.SetActive(true);
            playerObject.SetActive(false);
        }

        healthBar.fillAmount = (float)currentHealth / maxHealth;

        // Exibir a imagem de aviso de dano
        ShowDamageImage();
    }

    private void ShowDamageImage()
    {
        damageImage.gameObject.SetActive(true);
        Invoke("HideDamageImage", damageImageDuration);
    }

    private void HideDamageImage()
    {
        damageImage.gameObject.SetActive(false);
    }
}
