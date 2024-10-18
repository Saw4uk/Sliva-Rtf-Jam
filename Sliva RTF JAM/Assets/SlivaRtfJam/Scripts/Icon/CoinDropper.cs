using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CoinDropper : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform coin;

    public void DropCoin()
    {
        Instantiate(coin, transform.position, Quaternion.identity);
    }
}
