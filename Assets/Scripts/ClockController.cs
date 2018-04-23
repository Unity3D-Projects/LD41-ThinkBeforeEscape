using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockController : MonoBehaviour
{
    public float increateTimeBy = 1.0f;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(GameTags.Player))
        {
            var gameController = GameObject.FindObjectOfType<GameController>();
            gameController.IncreaseTimes(increateTimeBy);

            AudioManager.Play(GameAudioClip.Clock);

            Destroy(gameObject);
        }
    }
}
