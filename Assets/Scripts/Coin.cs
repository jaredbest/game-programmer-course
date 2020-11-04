using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public static int CoinsCollected;

    void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (player == null)
            return;

        gameObject.SetActive(false);
        CoinsCollected++;
        Debug.Log(CoinsCollected);
    }
}
