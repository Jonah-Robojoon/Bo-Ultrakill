using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WaveScript : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;

    private List<GameObject> _Enemies = new List<GameObject>();
    private int _currentWave = 0;
    private bool _waveCompleted = false;
    private bool _AllEnemiesDead = false;
    private bool _waveTwoStarted = false;

    private float _timer = 0f;

    void Start()
    {
        WaveOne();
    }

    void Update()
    {
        foreach (var enemy in _Enemies)
        {
            if (enemy.GetComponentInChildren<EnemyAi>()._isDeing == true)
            {
                _AllEnemiesDead = true;
                continue;
            }
            else
            {
                _AllEnemiesDead = false;
                return;
            }
        }
        if (_AllEnemiesDead)
        {
            _currentWave = 2;
            _waveCompleted = true;

        }
        if (_currentWave == 2)
        {
            _timer += Time.deltaTime;

            if (_timer >= 1f && !_waveTwoStarted)
            {
                _waveTwoStarted = true;
                _waveCompleted = false;
                WaveTwo();
            }

            if (_timer >= 3f)
            {
                foreach (var enemy in _Enemies)
                {
                    Destroy(enemy);
                }
            }
        }

    }

    private void WaveOne()
    {
        _Enemies.Add(Instantiate(_enemyPrefab, new Vector3(transform.position.x - 6, transform.position.y, transform.position.z), Quaternion.identity));
        _Enemies.Add(Instantiate(_enemyPrefab, new Vector3(transform.position.x - 8, transform.position.y, transform.position.z + 5), Quaternion.identity));
        _Enemies.Add(Instantiate(_enemyPrefab, new Vector3(transform.position.x - 8, transform.position.y, transform.position.z - 5), Quaternion.identity));
    }

    private void WaveTwo()
    {
        Instantiate(_enemyPrefab, new Vector3(transform.position.x + 8, transform.position.y, transform.position.z + 7), Quaternion.identity);
        Instantiate(_enemyPrefab, new Vector3(transform.position.x + 2, transform.position.y, transform.position.z + 7), Quaternion.identity);
        Instantiate(_enemyPrefab, new Vector3(transform.position.x - 8, transform.position.y, transform.position.z + 7), Quaternion.identity);
        Instantiate(_enemyPrefab, new Vector3(transform.position.x - 2, transform.position.y, transform.position.z + 7), Quaternion.identity);

        Instantiate(_enemyPrefab, new Vector3(transform.position.x + 8, transform.position.y, transform.position.z - 7), Quaternion.identity);
        Instantiate(_enemyPrefab, new Vector3(transform.position.x + 2, transform.position.y, transform.position.z - 7), Quaternion.identity);
        Instantiate(_enemyPrefab, new Vector3(transform.position.x - 8, transform.position.y, transform.position.z - 7), Quaternion.identity);
        Instantiate(_enemyPrefab, new Vector3(transform.position.x - 2, transform.position.y, transform.position.z - 7), Quaternion.identity);
    }
}
