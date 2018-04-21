using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public PlayerController2D player;

    public float turnTime = 5.0f;

    private float _elapsedTime;

    void Start()
    {

    }

    void Update()
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= turnTime)
        {
            //player.enabled = !player.enabled;

            _elapsedTime = 0.0f;
        }
    }
}
