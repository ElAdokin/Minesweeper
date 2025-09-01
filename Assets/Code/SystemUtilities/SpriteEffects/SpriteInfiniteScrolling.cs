using UnityEngine;

public class SpriteInfiniteScrolling : MonoBehaviour
{
    [SerializeField] private GameObject _first, _second;

    private const float _minScrollSpeed = 0.535f;
    private float _scrollSpeed;

    [SerializeField] private float _secondBeginPositionFactor;

    private Vector3 _secondBeginPosition;
    private Vector3 _firstTargetPosition;

    [SerializeField] private Direction _direction;

    private DirectionData _directionData;
    private Vector3 _destination; 

    private void Awake()
    {
        Setting();    
    }

    private void Setting()
    {
        switch (_direction)
        {
            case Direction.Up:
                _directionData = new DirectionData(_direction, Vector3.up);
                _secondBeginPosition = new Vector3(0, -_secondBeginPositionFactor, 0);
                break;
            case Direction.Down:
                _directionData = new DirectionData(_direction, Vector3.down);
                _secondBeginPosition = new Vector3(0, _secondBeginPositionFactor, 0);
                break;
            case Direction.Left:
                _directionData = new DirectionData(_direction, Vector3.left);
                _secondBeginPosition = new Vector3(_secondBeginPositionFactor, 0, 0);
                break;
            case Direction.Right:
                _directionData = new DirectionData(_direction, Vector3.right);
                _secondBeginPosition = new Vector3(-_secondBeginPositionFactor, 0, 0);
                break;
        }

        _firstTargetPosition = -1 * _secondBeginPosition;

        _first.transform.localPosition = Vector3.zero;
        _second.transform.localPosition = _secondBeginPosition;
    }

    private void Update()
    {
        //if (LooperController.PauseGame) return;

        SpriteMovement();
    }

    public float GetScrollSpeed()
    {
        return _scrollSpeed;
    }

    public void SetScrollSpeed(float factor)
    {
        _scrollSpeed = factor * _minScrollSpeed;
    }

    private void SpriteMovement()
    {
        if (CanMove())
        {
            _first.transform.localPosition += _directionData.DirectionVector * _scrollSpeed * Time.deltaTime;
            _second.transform.localPosition += _directionData.DirectionVector * _scrollSpeed * Time.deltaTime;
            return;
        }

        _first.transform.localPosition = _firstTargetPosition;
        _second.transform.localPosition = Vector3.zero;

        ResetScroll();
    }

    private bool CanMove()
    {
        switch (_direction)
        {
            case Direction.Up:
                return _first.transform.localPosition.y < _firstTargetPosition.y ? true : false;
            case Direction.Down:
                return _first.transform.localPosition.y > _firstTargetPosition.y ? true : false;
            case Direction.Left:
                return _first.transform.localPosition.x > _firstTargetPosition.x ? true : false;
            case Direction.Right:
                return _first.transform.localPosition.x < _firstTargetPosition.x ? true : false;
        }

        return false;
    }

    private void ResetScroll()
    {
        _first.transform.localPosition = Vector3.zero;
        _second.transform.localPosition = _secondBeginPosition;
    }
}
