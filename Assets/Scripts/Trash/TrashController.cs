using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashController : MonoBehaviour
{
    [SerializeField] float speed;

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
        
        if (transform.position.x > 3.35f)
        {
            Debug.Log("yup");
            transform.position = new Vector3(-8.15f, transform.position.y, transform.position.z);
        }
    }
}
