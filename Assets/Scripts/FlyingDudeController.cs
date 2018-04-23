using UnityEngine;

public class FlyingDudeController : EnemyController
{
    public float wanderingSpeed = 2;
    public float chasingSpeed = 5;
    public float minRadius = 3;
    public float maxRadius = 6;

    public Collider2D boundary;

    private PlayerController2D _player;

    private Vector2 _startPosition;
    private Vector2 _targetPosition;
    private bool _chasing;

    void Start()
    {
        _startPosition = transform.position;
        _player = GameObject.FindObjectOfType<PlayerController2D>();
    }

    void Update()
    {
        NextTarget();

        var speed = _chasing ? chasingSpeed : wanderingSpeed;
        var direction = (_targetPosition - (Vector2)transform.position).normalized;
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, _targetPosition) <= 0.1f)
        {
            _targetPosition = Vector2.zero;
        }
    }

    private void NextTarget()
    {
        if (Vector2.Distance(_player.transform.position, _startPosition) <= maxRadius)
        {
            _targetPosition = _player.transform.position;

            if (boundary != null)
            {
                var bounds = boundary.bounds;
                bounds.Expand(new Vector3(-2, -2, 0.0f));

                if (!bounds.Contains(_targetPosition))
                {
                    NextRandomTarget();
                    return;
                }
            }

            _chasing = true;
        }
        else if (_targetPosition == Vector2.zero)
        {
            NextRandomTarget();
        }
    }

    private void NextRandomTarget()
    {
        _targetPosition = _startPosition + Random.insideUnitCircle * Random.Range(minRadius, maxRadius);

        if (boundary != null)
        {
            var bounds = boundary.bounds;
            bounds.Expand(new Vector3(-2, -2, 0.0f));

            var count = 0;
            while (count < 5 && !bounds.Contains(_targetPosition))
            {
                _targetPosition = _startPosition + Random.insideUnitCircle * Random.Range(minRadius, maxRadius);
                count++;
            }
        }

        _chasing = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(GameTags.Player))
        {
            NextRandomTarget();

            var gameController = GameObject.FindObjectOfType<GameController>();
            gameController.ResetToSpawn();

            AudioManager.Play(GameAudioClip.Hurt);
        }
    }

    void OnDrawGizmos()
    {
        GizmosHelper.GizmosDrawCircle(transform.position, minRadius);
        GizmosHelper.GizmosDrawCircle(transform.position, maxRadius);
    }
}

public static class GizmosHelper
{
    public static void GizmosDrawCircle(Vector3 position, float radius)
    {
        Gizmos.color = Color.red;

        var lastPos = GetPositionInCircle(position, 0.0f, radius);
        for (var theta = 0.1f; theta <= Mathf.PI * 2; theta += 0.1f)
        {
            var newPos = GetPositionInCircle(position, theta, radius);
            Gizmos.DrawLine(lastPos, newPos);
            lastPos = newPos;
        }
        Gizmos.DrawLine(lastPos, GetPositionInCircle(position, 0.0f, radius));
    }

    private static Vector3 GetPositionInCircle(Vector3 position, float theta, float radius)
    {
        var x = radius * Mathf.Cos(theta);
        var y = radius * Mathf.Sin(theta);
        return new Vector3(position.x + x, position.y + y, 0);
    }
}

