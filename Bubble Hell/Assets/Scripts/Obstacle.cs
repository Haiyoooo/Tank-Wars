using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float angle = 0f;

    void Start()
    {
        angle = Random.Range(-5f, 5f);
    }

    void Update()
    {
       transform.Translate(Vector3.up * speed * Time.deltaTime, Space.Self);
       transform.Rotate(0f, 0f, angle * Time.deltaTime);
    }
}
