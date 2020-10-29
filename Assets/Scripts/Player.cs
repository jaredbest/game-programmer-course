using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _speed = 1;
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
        return Input.GetButton("Fire1") && _jumpTimer <= _maxJumpDuration;
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
        return Input.GetButtonDown("Fire1") && _jumpsRemaining > 0;
    }

    void MoveHorizontal()
    {
        if (Mathf.Abs(_horizontal) >= 1)
        {
            _rigidbody2D.velocity = new Vector2(_horizontal, _rigidbody2D.velocity.y);
            Debug.Log($"Velocity = {_rigidbody2D.velocity}");
        }
    }

    void ReadHorizontalInput()
    {
        _horizontal = Input.GetAxis("Horizontal") * _speed;
    }

    void UpdateSpriteDirection()
    {
        if (_horizontal != 0)
        {
            Debug.Log("Moving Right");
            _spriteRenderer.flipX = _horizontal < 0;
        }
    }

    void UpdateAnimator()
    {
        bool walking = _horizontal != 0;
        _animator.SetBool("isWalking", walking);
    }

    void UpdateIsGrounded()
    {
        var hit = Physics2D.OverlapCircle(_feet.position, 0.1f, LayerMask.GetMask("Default"));
        _isGrounded = hit != null;
    }

    internal void ResetToStart()
    {
        transform.position = _startPosition;
    }
}