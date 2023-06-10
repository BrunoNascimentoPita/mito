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
    public GameObject damageTextPrefab;
    public float damageTextDuration = 0.4f;

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
            Die();
        }

        // Atualizar o medidor de vida
        float normalizedHealth = (float)currentHealth / maxHealth;
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, normalizedHealth, Time.deltaTime * healthUpdateSpeed);

        // Realizar o knockback
        float knockbackDirection = isFacingRight ? -1f : 1f;
        Knockback(knockbackDirection);

        // Exibir o texto de dano
        ShowDamageText(damage);
    }

    private void ShowDamageText(int damage)
    {
        GameObject damageTextObject = Instantiate(damageTextPrefab, transform.position, Quaternion.identity);
        DamageText damageText = damageTextObject.GetComponent<DamageText>();
        damageText.ShowDamage(damage);

        Destroy(damageTextObject, damageTextDuration);
    }

    public void Knockback(float knockbackDirection)
    {
        float knockbackForce = 5f; // Força de knockback

        // Aplicar força no sentido oposto ao flip
        rb.AddForce(new Vector2(knockbackDirection * knockbackForce, 0f), ForceMode2D.Impulse);
    }

    private void Die()
    {
        healthBar.gameObject.SetActive(false);
        // Desativar o jogador
        playerObject.SetActive(false);

        // Ativar o painel de derrota
        gameOverPanel.SetActive(true);

        // Pausar o jogo
        Time.timeScale = 0f;
    }
}
