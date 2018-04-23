using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class GameController : MonoBehaviour
{
    public PlayerController2D player;
    public GameObject PlayerTurnImage;
    public GameObject EnemiesTurnImage;

    public float playerTurnTime = 3.0f;
    public float enemiesTurnTime = 3.0f;

    public Text timeText;

    private bool _playerTurn;
    private float _elapsedTime;

    void Start()
    {
        ResetToSpawn(false);

        player.enabled = true;

        var enemies = GameObject.FindGameObjectsWithTag(GameTags.Enemy);
        for (int i = 0; i < enemies.Length; i++)
        {
            var enemyController = enemies[i].GetComponent<EnemyController>();
            enemyController.enabled = false;
        }

        _playerTurn = true;
    }

    void Update()
    {
        _elapsedTime += Time.deltaTime;

        if (_playerTurn)
        {
            if (_elapsedTime >= playerTurnTime)
            {
                player.enabled = false;

                var enemies = GameObject.FindGameObjectsWithTag(GameTags.Enemy);
                for (int i = 0; i < enemies.Length; i++)
                {
                    var enemyController = enemies[i].GetComponent<EnemyController>();
                    enemyController.enabled = true;
                }

                PlayerTurnImage.SetActive(false);
                EnemiesTurnImage.SetActive(true);

                _playerTurn = false;
                _elapsedTime = 0.0f;

                AudioManager.Play(GameAudioClip.ChangeTurn);
            }

            timeText.text = string.Format("TIME: {0:F}", playerTurnTime - _elapsedTime);
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

                player.enabled = true;

                PlayerTurnImage.SetActive(true);
                EnemiesTurnImage.SetActive(false);

                _playerTurn = true;
                _elapsedTime = 0.0f;

                AudioManager.Play(GameAudioClip.ChangeTurn2);
            }

            timeText.text = string.Format("TIME: {0:F}", enemiesTurnTime - _elapsedTime);
        }
    }

    public void IncreaseTimes(float increaseBy)
    {
        playerTurnTime += increaseBy;
        enemiesTurnTime += increaseBy * 0.5f;

        _playerTurn = true;
        _elapsedTime = 0.0f;
    }

    public void ResetToSpawn(bool vibrate = true)
    {
        if (vibrate)
        {
            GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
            Invoke("ClearGamePadVibration", 0.2f);
        }

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

    void ClearGamePadVibration()
    {
        GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
    }
}

public static class GameTags
{
    public const string Player = "Player";
    public const string Enemy = "Enemy";
    public const string Through = "Through";
}