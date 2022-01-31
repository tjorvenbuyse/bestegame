using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 200f;

    public void Start()
    {
        health = 200;
    }

    private void OnEnable()
    {
        health = 200;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if(health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
