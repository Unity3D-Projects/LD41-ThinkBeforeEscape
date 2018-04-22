using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public PlayerController2D player;

    public bool playerTurn;
    public float playerTurnTime = 3.0f;
    public float enemiesTurnTime = 3.0f;

    public Text timeText;

    private float _elapsedTime;

    void Start()
    {
        ResetToSpawn();

        playerTurn = true;
    }

    void Update()
    {
        _elapsedTime += Time.deltaTime;

        if (playerTurn)
        {
            if (_elapsedTime >= playerTurnTime)
            {
                //player.enabled = false;

                var enemies = GameObject.FindGameObjectsWithTag(GameTags.Enemy);
                for (int i = 0; i < enemies.Length; i++)
                {
                    var enemyController = enemies[i].GetComponent<EnemyController>();
                    enemyController.enabled = true;
                }

                playerTurn = false;
                _elapsedTime = 0.0f;
            }

            timeText.text = string.Format("{0:F}", playerTurnTime - _elapsedTime);
        }
        else
        {
            if (_elapsedTime >= enemiesTurnTime)
            {
                var enemies = GameObject.FindGameObjectsWithTag(GameTags.Enemy);
                for (int i = 0; i < enemies.Length; i++)
                {
                    var enemyController = enemies[i].GetComponent<EnemyController>();
                    enemyController.enabled = false;
                }

                //player.enabled = true;

                playerTurn = true;
                _elapsedTime = 0.0f;
            }

            timeText.text = string.Format("{0:F}", enemiesTurnTime - _elapsedTime);
        }
    }

    public void ResetToSpawn()
    {
        var playerSpawns = GameObject.FindObjectsOfType<PlayerSpawnController>();
        for (int i = 0; i < playerSpawns.Length; i++)
        {
            if (playerSpawns[i].active)
            {
                player.transform.position = playerSpawns[i].transform.position;
                break;
            }
        }
    }
}

public static class GameTags
{
    public const string Player = "Player";
    public const string Enemy = "Enemy";
    public const string Through = "Through";
}