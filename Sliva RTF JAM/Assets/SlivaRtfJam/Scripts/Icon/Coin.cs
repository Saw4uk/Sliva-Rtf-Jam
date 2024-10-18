using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int value;

    // Start is called before the first frame update
    public int PickUp()
    {
        Destroy(gameObject);
        return value;
    }
}