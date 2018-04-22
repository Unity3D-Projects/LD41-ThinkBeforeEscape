using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class GoombaController : EnemyController
{
    private CharacterController2D _controller;
    private SpriteRenderer _spriteRenderer;

    public float speed = 6.0f;
    public float maxJumpHeight = 4.0f;
    public float minJumpHeight = 1.0f;
    public float jumpTime = 0.4f;

    public float accelerationTimeAir = 0.2f;
    public float accelerationTimeGrounded = 0.1f;

    private float _gravity;

    private Vector2 _velocity;
    private float _velocityXSmoothing;

    private float _dirX = -1.0f;

    void Awake()
    {
        _controller = GetComponent<CharacterController2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        _gravity = (-2 * maxJumpHeight) / (jumpTime * jumpTime);
    }

    void Update()
    {
        var input = new Vector2(_dirX, 0);

        var scale = _spriteRenderer.transform.localScale;
        scale.x = _controller._facingDirection >= 0 ? 1 : -1;
        _spriteRenderer.transform.localScale = scale;

        var targetVelocityX = input.x * speed;
        var smoothTime = _controller.collisions.below ? accelerationTimeGrounded : accelerationTimeAir;
        _velocity.x = Mathf.SmoothDamp(_velocity.x, targetVelocityX, ref _velocityXSmoothing, smoothTime);

        _velocity.y += _gravity * Time.deltaTime;

        _controller.Move(_velocity * Time.deltaTime);

        if (_controller.collisions.above || _controller.collisions.below)
        {
            if (_controller.collisions.slidingDownMaxSlope)
            {
                _velocity.y += _controller.collisions.slopeNormal.y * -_gravity * Time.deltaTime;
            }
            else
            {
                _velocity.y = 0;
            }
        }

        if (_controller.collisions.left || _controller.collisions.right)
        {
            _velocity.x = 0;
            _dirX = -_dirX;
        }

        if (_velocity.x != 0)
        {
            var rayOrigin = _controller._facingDirection < 0 ? _controller._raycastOrigins.bottomLeft : _controller._raycastOrigins.bottomRight;
            var hit = Physics2D.Raycast(rayOrigin, new Vector2(_controller._facingDirection, -1), 1.0f, _controller.collisionMask);
            if (!hit)
            {
                _velocity.x = 0;
                _dirX = -_dirX;
            }
        }
    }

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

