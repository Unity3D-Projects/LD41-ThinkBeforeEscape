using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(GameTags.Player))
        {
            var gameController = GameObject.FindObjectOfType<GameController>();
            gameController.ResetToSpawn();

            AudioManager.Play(GameAudioClip.Hurt);
        }
    }
}
