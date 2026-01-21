using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    private float _health = 1f;

    private float _timer;
    
    void Start()
    {
        EnemyAi.onPlayerHit += OnHit;
        EnemyAi.onPlayerHeal += OnHeal;

    }
    private void FixedUpdate()
    {
        healthSlider.value = Mathf.Lerp(healthSlider.value, _health, 0.1f);
        if (healthSlider.value <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void Update()
    {
        _timer =+ Time.deltaTime;
    }

    void OnDisable()
    {
        EnemyAi.onPlayerHit -= OnHit;
        EnemyAi.onPlayerHeal -= OnHeal;
    }
    void OnHit()
    {
        //Debug.Log("damagee");
        if (_timer > 0.5f) return;
        _timer = 0f;
        _health -= 0.1f;
        
    }
    void OnHeal()
    {
        
        //Debug.Log("healed");
        _health += 0.4f;
    }
}
