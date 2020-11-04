using UnityEngine;
using UnityEngine.UI;

public class UICoinsCollected : MonoBehaviour
{
    Text _text;

    void Start()
    {
        _text = GetComponent<Text>();
    }

    void Update()
    {
        _text.text = Coin.CoinsCollected.ToString();
    }
}
