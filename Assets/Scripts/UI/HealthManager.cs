using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    private int _health = 0;
    private TextMeshProUGUI _scoreText;
    void Start()
    {
        EnemyAi.playerHit += OnHit;
        _scoreText = GetComponent<TextMeshProUGUI>();
    }
    void OnDisable()
    {
        EnemyAi.playerHit -= OnHit;

    }
    void OnHit()
    {
        _health -= 1;
        _scoreText.text = "Score: " + _health;
    }
}
