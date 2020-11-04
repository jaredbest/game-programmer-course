using UnityEngine;
using UnityEngine.Events;

public class PushButtonSwitch : MonoBehaviour
{
    [SerializeField] Sprite _pressedSprite;
    [SerializeField] UnityEvent _onPressed;
    [SerializeField] UnityEvent _onReleased;
    [SerializeField] int _playerNumber = 1;

    SpriteRenderer _spriteRenderer;
    Sprite _releasedSprite;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _releasedSprite = _spriteRenderer.sprite;
        BecomeReleased();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (player == null || player.PlayerNumber != _playerNumber)
            return;
        BecomePressed();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (player == null || player.PlayerNumber != _playerNumber)
            return;

        BecomeReleased();
    }

    void BecomePressed()
    {
        _spriteRenderer.sprite = _pressedSprite;
        _onPressed?.Invoke();
    }

    void BecomeReleased()
    {
        _spriteRenderer.sprite = _releasedSprite;
        _onReleased?.Invoke();
    }
}
