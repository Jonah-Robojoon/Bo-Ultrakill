using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        if (healthSlider.value < 0.5f) return;
        _timer = 0f;
        Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        _health -= 0.1f;
        
    }
    void OnHeal()
    {
        
        Debug.Log("healed");
        _health += 0.4f;
    }
}
