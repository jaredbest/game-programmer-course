using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] KeyLock _keyLock;

    void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();

        if (player != null)
        {
            transform.SetParent(player.transform);
            transform.localPosition = Vector3.up;
        }

        var keyLock = collision.GetComponent<KeyLock>();

        if (keyLock != null && keyLock == _keyLock)
        {
            keyLock.Unlock();
            Destroy(gameObject);
        }
    }
}
