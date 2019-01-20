using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] float speed = 4f;

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Obstacle")
        {
            Destroy(this.gameObject);
        }
    }
}
