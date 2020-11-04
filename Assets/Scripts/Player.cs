using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] int _playerNumber = 1;
    [Header("Movement")]
    [SerializeField] float _speed = 1;
    [SerializeField] float _slipFactor = 1;
    [Header("Jump")]
    [SerializeField] float _jumpVelocity = 10;
    [SerializeField] int _maxJumps = 2;
    [SerializeField] Transform _feet;
    [SerializeField] float _downPull = 0.1f;
    [SerializeField] float _maxJumpDuration = 0.1f;

    Vector3 _startPosition;
    int _jumpsRemaining;
    float _fallTimer;
    float _jumpTimer;
    Rigidbody2D _rigidbody2D;
    Animator _animator;
    SpriteRenderer _spriteRenderer;
    float _horizontal;
    bool _isGrounded;
    bool _isOnSlipperySurface;

    public int PlayerNumber => _playerNumber;

    void Start()
    {
        _startPosition = transform.position;
        _jumpsRemaining = _maxJumps;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        UpdateIsGrounded();
        ReadHorizontalInput();

        if (_isOnSlipperySurface)
            SlipHorizontal();
        else
            MoveHorizontal();

        UpdateAnimator();
        UpdateSpriteDirection();

        if (ShouldStartJump())
            Jump();
        else if (ShouldContinueJump())
            ContinueJump();

        _jumpTimer += Time.deltaTime;
        if (_isGrounded && _fallTimer > 0)
        {
            _fallTimer = 0;
            _jumpsRemaining = _maxJumps;

        }
        else
        {
            _fallTimer += Time.deltaTime;
            var downForce = _downPull * _fallTimer * _fallTimer;
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y - downForce);
        }
    }

    void ContinueJump()
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpVelocity);
        _fallTimer = 0;
    }

    bool ShouldContinueJump()
    {
        return Input.GetButton($"P{_playerNumber}Jump") && _jumpTimer <= _maxJumpDuration;
    }

    void Jump()
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpVelocity);
        _jumpsRemaining--;
        Debug.Log($"Jumps remaining {_jumpsRemaining}");
        _fallTimer = 0;
        _jumpTimer = 0;
    }

    bool ShouldStartJump()
    {
        return Input.GetButtonDown($"P{_playerNumber}Jump") && _jumpsRemaining > 0;
    }

    void MoveHorizontal()
    {
        _rigidbody2D.velocity = new Vector2(_horizontal * _speed, _rigidbody2D.velocity.y);
    }

    void SlipHorizontal()
    {
        var desiredVelocity = new Vector2(_horizontal * _speed, _rigidbody2D.velocity.y);
        var smoothedVelocity = Vector2.Lerp(
            _rigidbody2D.velocity,
            desiredVelocity,
            Time.deltaTime / _slipFactor);
        _rigidbody2D.velocity = smoothedVelocity;
    }

    void ReadHorizontalInput()
    {
        _horizontal = Input.GetAxis($"P{_playerNumber}Horizontal") * _speed;
    }

    void UpdateSpriteDirection()
    {
        if (_horizontal != 0)
        {
            _spriteRenderer.flipX = _horizontal < 0;
        }
    }

    void UpdateAnimator()
    {
        bool walking = _horizontal != 0;
        _animator.SetBool("isWalking", walking);
        _animator.SetBool("isJumping", ShouldContinueJump());
    }

    void UpdateIsGrounded()
    {
        var hit = Physics2D.OverlapCircle(_feet.position, 0.1f, LayerMask.GetMask("Default"));
        _isGrounded = hit != null;

        if (hit != null)
            _isOnSlipperySurface = hit.CompareTag("Slippery");
        else
            _isOnSlipperySurface = false;
    }

    internal void ResetToStart()
    {
        _rigidbody2D.position = _startPosition;
    }

    internal void TeleportTo(Vector3 position)
    {
        _rigidbody2D.position = position;
        _rigidbody2D.velocity = Vector2.zero;
    }
}