using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class TurretTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var healthable = GetComponent<Healthable>();
        healthable.OnDie.AddListener(() => Destroy(gameObject));
    }

    // Update is called once per frame
    void Update()
    {
    }
}