using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Follower : MonoBehaviour
{

    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;

    float defaultSmoothTime = 0.8f;
    [SerializeField] float smoothTime = 0.8f;
    [SerializeField] bool allowSmoothnes = true;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        StartCoroutine(followSmoothly());
    }
    private void Update()
    {
        if (allowSmoothnes)
        {
            smoothTime = defaultSmoothTime;
        }
        else
        {
            smoothTime = 0;
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            allowSmoothnes = !allowSmoothnes;
        }
    }
    IEnumerator followSmoothly()
    {
        while (true)
        {
            // Vector3 middlePosition = Vector3.Lerp(transform.position, target.position + offset, followSpeed);
            transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, ref velocity, smoothTime);


            yield return null;
        }
    }

}

