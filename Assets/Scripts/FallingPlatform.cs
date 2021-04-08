using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public bool PlayerInside;
    HashSet<Player> _playersInTrigger = new HashSet<Player>();
    Coroutine _coroutine;
    Vector3 _initialPosition;
    bool _falling;
    float _wiggleTimer;

    [Tooltip("Reset the wiggle timer when no players are on the platform")]
    [SerializeField] bool _resetOnEmpty;
    [SerializeField] float _fallSpeed = 3;
    [Range(0.1f, 5)] [SerializeField] float _fallAfterSeconds = 3;
    [Range(0.005f, 0.1f)] [SerializeField] float _shakeX = 0.005f;
    [Range(0.005f, 0.1f)] [SerializeField] float _shakeY = 0.005f;

    void Start()
    {
        _initialPosition = transform.position;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (player == null)
            return;

        _playersInTrigger.Add(player);

        PlayerInside = true;

        if (_playersInTrigger.Count == 1)
            StartCoroutine(WiggleAndFall());
    }

    IEnumerator WiggleAndFall()
    {
        Debug.Log("Waiting to wiggle");
        yield return new WaitForSeconds(0.25f);
        Debug.Log("Wiggling");
        //_wiggleTimer = 0;

        while (_wiggleTimer < _fallAfterSeconds)
        {
            float randomX = UnityEngine.Random.Range(-_shakeX, _shakeX);
            float randomY = UnityEngine.Random.Range(-_shakeY, _shakeY);
            transform.position = _initialPosition + new Vector3(randomX, randomY);
            float randomDelay = UnityEngine.Random.Range(0.005f, 0.01f);
            yield return new WaitForSeconds(randomDelay);
            _wiggleTimer += randomDelay;
        }

        Debug.Log("Falling");
        _falling = true;
        foreach (var collider in GetComponents<Collider2D>())
        {
            collider.enabled = false;
        }

        float fallTimer = 0;

        while (fallTimer < 3f)
        {
            transform.position += Vector3.down * Time.deltaTime * _fallSpeed;
            fallTimer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (_falling)
            return;

        var player = collision.GetComponent<Player>();
        if (player == null)
            return;

        _playersInTrigger.Remove(player);

        if (_playersInTrigger.Count == 0)
        {
            PlayerInside = false;
            StopCoroutine(WiggleAndFall());

            if (_resetOnEmpty)
                _wiggleTimer = 0;
        }
    }
}
