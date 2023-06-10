using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    public float moveSpeed = 2f; // Velocidade de movimento do texto de dano
    public float fadeDuration = 1f; // Duração do efeito de fade-out
    public Text damageText; // Componente Text do texto de dano

    private void Start()
    {
        Destroy(gameObject, fadeDuration);
    }

    private void Update()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
    }

    public void ShowDamage(int damage)
    {
        damageText.text = damage.ToString();
    }
}
