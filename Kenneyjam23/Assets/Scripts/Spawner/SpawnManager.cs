using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameTimer _timer;

    /// <summary>
    /// Rarity should be 100 in total
    /// </summary>
    [SerializeField] private List<Enemy> _spawnList = new List<Enemy>();
    [SerializeField] private float _spawnFreq = 10f;
    [SerializeField] private float _minSpawnFreq = 10f;
    [SerializeField] private float _increaseRatio = 0.1f;
    [SerializeField] private int _spawnAmount = 3;

    private SpawnPoint[] _spawnPoints;

    private float _currentSpawnFreq;
    private float _spawnCooldown;

    private void Start()
    {
        _currentSpawnFreq = _spawnFreq;
        _spawnCooldown = _spawnFreq;

        int check = 0;
        foreach (var enemy in _spawnList)
        {
            check += enemy.Rarity;
        }

        if (check != 100)
        {
            Debug.LogError($"Spawn rarity is {check} and should be 100");
            enabled = false;
        }

        _spawnPoints = FindObjectsOfType<SpawnPoint>();
        if (_spawnPoints.Length == 0)
        {
            Debug.LogError($"No spawnpoints found!");
            enabled = false;
        }
    }

    private void Update()
    {
        if (!GameTimer.IsPaused)
        {
            _spawnCooldown -= Time.deltaTime;
        }

        if (_spawnCooldown <= 0f)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        _currentSpawnFreq -= _increaseRatio;
        if (_currentSpawnFreq < _minSpawnFreq)
        {
            _currentSpawnFreq = _minSpawnFreq;
        }
        _spawnCooldown = _currentSpawnFreq;

        for (int i = 0; i < _spawnAmount; ++i)
        {
            var position = _spawnPoints[Random.Range(0, _spawnPoints.Length)].transform.position;
            int randomNumber = Random.Range(0, 100);
            int number = 0;
            foreach (var enemy in _spawnList)
            {
                number += enemy.Rarity;
                if (number > randomNumber)
                {
                    var go = Instantiate(enemy.Mob, position, Quaternion.identity);

                    //Increase health overtime
                    go.GetComponent<Health>().SetHealth(enemy.Health + Mathf.FloorToInt(enemy.IncreaseHealth * _timer.TotalTime));

                    break;
                }
            }
        }
    }
}

[System.Serializable]
struct Enemy
{
    public GameObject Mob;

    /// <summary>
    /// Between 0 - 100
    /// </summary>
    public int Rarity;

    public int Health;
    /// <summary>
    /// Spawn health increase every second
    /// </summary>
    public float IncreaseHealth;
}