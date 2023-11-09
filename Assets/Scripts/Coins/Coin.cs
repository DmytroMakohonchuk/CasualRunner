using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    public float turnSpeed = 90f;
    private float throwForce = 30;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name != "Player")
        {
            return;
        }

        Destroy(gameObject);
    }

    public Vector3 GetRandomVectorUp()
    {

        float randomAngle = Random.Range(0f, 360f);

        Vector3 randomVector = Quaternion.Euler(0f, randomAngle, 0f) * Vector3.forward;

        return randomVector;
    }

    public void AddRandomImpulse()
    {
        var forceDirection = GetRandomVectorUp() * throwForce;
        rb.AddForce(forceDirection, ForceMode.Impulse);
    }

    private void Update()
    {
        transform.Rotate(0, 0, turnSpeed * Time.deltaTime);
    }
}
