using UnityEngine;

public class PlayerSpawnController : MonoBehaviour
{
    public bool active;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(GameTags.Player))
        {
            if (!active)
            {
                var spawnControllers = GameObject.FindObjectsOfType<PlayerSpawnController>();
                for (int i = 0; i < spawnControllers.Length; i++)
                {
                    spawnControllers[i].active = false;
                }

                active = true;
            }
            
        }
    }
}
