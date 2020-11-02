using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    [SerializeField] Transform _leftSensor;
    [SerializeField] Transform _rightSensor;

    Rigidbody2D _rigidbody2D;
    float _direction = -1;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _rigidbody2D.velocity = new Vector2(_direction, _rigidbody2D.velocity.y);
        
        if(_direction < 0)
        {
            Debug.DrawRay(_leftSensor.position, Vector2.down * 0.1f, Color.red);
            var result = Physics2D.Raycast(_leftSensor.position, Vector2.down, 0.1f);
            if (result.collider == null)
                TurnAround();
        }
        else
        {
            Debug.DrawRay(_rightSensor.position, Vector2.down * 0.1f, Color.red);
            var result = Physics2D.Raycast(_rightSensor.position, Vector2.down, 0.1f);
            if (result.collider == null)
                TurnAround();
        }
    }

    void TurnAround()
    {
        _direction *= -1;
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = _direction > 0;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        var player = collision.collider.GetComponent<Player>();
        if (player == null)
            return;

        player.ResetToStart();
    }
}
