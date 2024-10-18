using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [SerializeField] private Beat beat;
    [SerializeField] private bool canGoThrowTarget;
    // [SerializeField] private float damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && beat is Beat.Player or Beat.Both ||
            other.CompareTag("Enemy") && beat is Beat.Enemy or Beat.Both)
        {
            // other.GetComponent<Healthable>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private enum Beat
    {
        Player,
        Enemy,
        Both
    }
}