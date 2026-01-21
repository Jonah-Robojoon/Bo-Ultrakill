using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class WaveScript : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;

    private List<GameObject> _Enemies = new List<GameObject>();
    private int _currentWave = 1;
    private bool _waveCompleted = false;
    private bool _AllEnemiesDead = false;
    private bool _waveInProgress = false;

    [SerializeField] private GameObject _door;
    private Animator _anim;

    private float _timer = 0f;

    void Start()
    {
        _anim = _door.GetComponent<Animator>();
        WaveOne();
    }

    void Update()
    {
        if (_waveInProgress)
        {
            foreach (var enemy in _Enemies)
            {
                if (enemy == null)
                {
                    return;
                }
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
        }

        if (_AllEnemiesDead)
        {
            _timer += Time.deltaTime;

            if (_timer <= 1) return;


            _waveInProgress = false;
            _currentWave += 1;
            _waveCompleted = true;
        }

        if (_waveCompleted && _currentWave == 2)
        {
            Debug.Log("Starting Wave 2");
            WaveTwo();
            _waveCompleted = false;
        }


        if (_AllEnemiesDead && _currentWave == 3)
        {
            Debug.Log("All Waves Completed!");
            _anim.SetBool("wonWave", true);
        }

    }

    private void WaveOne()
    {
        _Enemies.Add(Instantiate(_enemyPrefab, new Vector3(transform.position.x - 6, transform.position.y, transform.position.z), Quaternion.identity));
        _Enemies.Add(Instantiate(_enemyPrefab, new Vector3(transform.position.x - 8, transform.position.y, transform.position.z + 5), Quaternion.identity));
        _Enemies.Add(Instantiate(_enemyPrefab, new Vector3(transform.position.x - 8, transform.position.y, transform.position.z - 5), Quaternion.identity));
        _waveInProgress = true;
    }

    private void WaveTwo()
    {
        _Enemies.Add(Instantiate(_enemyPrefab, new Vector3(transform.position.x + 8, transform.position.y, transform.position.z + 7), Quaternion.identity));
        _Enemies.Add(Instantiate(_enemyPrefab, new Vector3(transform.position.x + 2, transform.position.y, transform.position.z + 7), Quaternion.identity));
        _Enemies.Add(Instantiate(_enemyPrefab, new Vector3(transform.position.x - 8, transform.position.y, transform.position.z + 7), Quaternion.identity));
        _Enemies.Add(Instantiate(_enemyPrefab, new Vector3(transform.position.x - 2, transform.position.y, transform.position.z + 7), Quaternion.identity));

        _Enemies.Add(Instantiate(_enemyPrefab, new Vector3(transform.position.x + 8, transform.position.y, transform.position.z - 7), Quaternion.identity));
        _Enemies.Add(Instantiate(_enemyPrefab, new Vector3(transform.position.x + 2, transform.position.y, transform.position.z - 7), Quaternion.identity));
        _Enemies.Add(Instantiate(_enemyPrefab, new Vector3(transform.position.x - 8, transform.position.y, transform.position.z - 7), Quaternion.identity));
        _Enemies.Add(Instantiate(_enemyPrefab, new Vector3(transform.position.x - 2, transform.position.y, transform.position.z - 7), Quaternion.identity));
        _waveInProgress = true;
    }
}
