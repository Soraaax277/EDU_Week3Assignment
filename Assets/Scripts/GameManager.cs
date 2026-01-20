using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game References")]
    public PlayerController player;
    public Transform winLocation;
    public float winRadius = 1f;
    public GameObject winUI;

    [Header("Turret References")]
    public List<BaseTurret> turrets = new List<BaseTurret>();
    
    private List<Projectile> activeProjectiles = new List<Projectile>();
    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (winUI != null) winUI.SetActive(false);

        if (player == null) player = FindFirstObjectByType<PlayerController>();
        if (turrets == null || turrets.Count == 0)
        {
            turrets = new List<BaseTurret>(FindObjectsByType<BaseTurret>(FindObjectsSortMode.None));
            Debug.Log($"Auto-found {turrets.Count} turrets.");
        }
    }

    void Update()
    {
        if (isGameOver) return;

        float deltaTime = Time.deltaTime;

        foreach (var turret in turrets)
        {
            turret.UpdateTurret(player.transform.position, deltaTime);
        }

        for (int i = activeProjectiles.Count - 1; i >= 0; i--)
        {
            Projectile proj = activeProjectiles[i];
            if (proj == null)
            {
                activeProjectiles.RemoveAt(i);
                continue;
            }

            proj.UpdateProjectile(deltaTime);

            if (MathCollision.CheckCircleCollision(player.transform.position, player.radius, proj.transform.position, proj.radius))
            {
                GameOver(false);
                return;
            }

            if (proj.transform.position.magnitude > 100f)
            {
                Destroy(proj.gameObject);
                activeProjectiles.RemoveAt(i);
            }
        }

        if (player.transform.position.x >= winLocation.position.x && 
            Mathf.Abs(player.transform.position.z - winLocation.position.z) < winRadius)
        {
            GameOver(true);
        }
    }

    public void RegisterProjectile(Projectile proj)
    {
        activeProjectiles.Add(proj);
    }

    void GameOver(bool win)
    {
        isGameOver = true;
        if (win)
        {
            Debug.Log("YOU WIN!");
            if (winUI != null) winUI.SetActive(true);
            foreach (var turret in turrets) turret.SetTurretActive(false);
        }
        else
        {
            Debug.Log("GAME OVER - PLAYER HIT");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
