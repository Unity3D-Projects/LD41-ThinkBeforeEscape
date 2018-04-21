using UnityEngine;

public class CameraController2D : MonoBehaviour
{
    public CharacterController2D target;
    public Vector2 focusAreaSize;

    public bool followTargetX = true, followTargetY;

    private FocusArea _focusArea;
    private float _cameraWidth;
    private float _cameraHeight;

    private Vector2 _targetPosition;
    private Vector2 _dampVelocity;
    private bool _changing;

    void Start()
    {
        var collider = target.GetComponent<BoxCollider2D>();
        _focusArea = new FocusArea(collider.bounds, focusAreaSize);

        var camera = Camera.main;
        _cameraHeight = camera.orthographicSize * 2;
        _cameraWidth = _cameraHeight * camera.aspect;

        _targetPosition = transform.position;
    }

    void LateUpdate()
    {
        var position = (Vector2)transform.position;

        var collider = target.GetComponent<BoxCollider2D>();
        _focusArea.Update(collider.bounds);

        if (!_changing)
        {
            var targetPosition = target.transform.position;
            
            if (targetPosition.x >= position.x + _cameraWidth * 0.5f)
            {
                _targetPosition.x = position.x + _cameraWidth + 0.68f;
                target.Move(new Vector2(0.68f, 0.0f));
                _changing = true;
            }
            else if (targetPosition.x <= position.x - _cameraWidth * 0.5f)
            {
                _targetPosition.x = position.x - _cameraWidth - 0.68f;
                target.Move(new Vector2(-0.68f, 0.0f));
                _changing = true;
            }
            else if (targetPosition.y >= position.y + _cameraHeight * 0.5f)
            {
                _targetPosition.y = position.y + _cameraHeight;
                _changing = true;
            }
            else if (targetPosition.y <= position.y - _cameraHeight * 0.5f)
            {
                _targetPosition.y = position.y - _cameraHeight;
                _changing = true;
            }

            if (followTargetX)
            {
                _targetPosition.x = _focusArea.center.x;
                _changing = true;
            }

            if (followTargetY)
            {
                _targetPosition.y = _focusArea.center.y;
                _changing = true;
            }
        }

        if (_changing)
        {
            position = Vector2.SmoothDamp(position, _targetPosition, ref _dampVelocity, 0.1f, 30.0f, Time.deltaTime);
            transform.position = (Vector3)position + Vector3.forward * -10;

            if (Vector2.Distance(position, _targetPosition) <= Vector2.kEpsilon)
            {
                _changing = false;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
        Gizmos.DrawCube(_focusArea.center, focusAreaSize);
    }

    struct FocusArea
    {
        public Vector2 center;
        public Vector2 velocity;

        public float left, right;
        public float top, bottom;

        public FocusArea(Bounds targetBounds, Vector2 size)
        {
            left = targetBounds.center.x - size.x * 0.5f;
            right = targetBounds.center.x + size.x * 0.5f;
            top = targetBounds.center.y + size.y * 0.5f;
            bottom = targetBounds.center.y - size.y * 0.5f;
            center = new Vector2((left + right) * 0.5f, (top + bottom) * 0.5f);
            velocity = Vector2.zero;
        }

        public void Update(Bounds targetBounds)
        {
            var shiftX = 0.0f;
            if (targetBounds.min.x < left)
            {
                shiftX = targetBounds.min.x - left;
            }
            else if (targetBounds.max.x > right)
            {
                shiftX = targetBounds.max.x - right;
            }

            var shiftY = 0.0f;
            if (targetBounds.min.y < bottom)
            {
                shiftY = targetBounds.min.y - bottom;
            }
            else if (targetBounds.max.y > top)
            {
                shiftY = targetBounds.max.y - top;
            }

            left += shiftX;
            right += shiftX;
            top += shiftY;
            bottom += shiftY;
            center = new Vector2((left + right) * 0.5f, (top + bottom) * 0.5f);
            velocity = new Vector2(shiftX, shiftY);
        }
    }
}
