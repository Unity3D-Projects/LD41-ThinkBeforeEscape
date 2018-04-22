using UnityEngine;

public class FlyingDudeController : EnemyController
{
    public float wanderingSpeed = 2;
    public float chasingSpeed = 5;
    public float minRadius = 3;
    public float maxRadius = 6;

    private PlayerController2D _player;

    private Vector2 _startPosition;
    private Vector2 _targetPosition;

    void Start()
    {
        _startPosition = transform.position;
        _player = GameObject.FindObjectOfType<PlayerController2D>();
    }

    void Update()
    {
        var speed = wanderingSpeed;
        if (Vector2.Distance(_player.transform.position, _startPosition) <= maxRadius)
        {
            _targetPosition = _player.transform.position;
            speed = chasingSpeed;
        }
        else if (_targetPosition == Vector2.zero)
        {
            _targetPosition = _startPosition + Random.insideUnitCircle * Random.Range(minRadius, maxRadius);
        }

        var direction = (_targetPosition - (Vector2)transform.position).normalized;
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, _targetPosition) <= 0.1f)
        {
            _targetPosition = Vector2.zero;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(GameTags.Player))
        {
            _targetPosition = _startPosition + Random.insideUnitCircle * Random.Range(1, maxRadius);

            var gameController = GameObject.FindObjectOfType<GameController>();
            gameController.ResetToSpawn();

            AudioManager.Play(GameAudioClip.Hurt);
        }
    }

    void OnDrawGizmos()
    {
        GizmosDrawCircle(transform.position, minRadius);
        GizmosDrawCircle(transform.position, maxRadius);
    }

    void GizmosDrawCircle(Vector3 position, float radius)
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

    private Vector3 GetPositionInCircle(Vector3 position, float theta, float radius)
    {
        var x = radius * Mathf.Cos(theta);
        var y = radius * Mathf.Sin(theta);
        return new Vector3(position.x + x, position.y + y, 0);
    }
}
