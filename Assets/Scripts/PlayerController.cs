using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float radius = 0.5f;
    
    private Vector2 moveInput;
    private bool isDead = false;

    void Update()
    {
        if (isDead) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        
        Vector3 move = new Vector3(h, 0, v).normalized;
        transform.position += move * moveSpeed * Time.deltaTime;
    }

    public void Die()
    {
        isDead = true;
    }
}
