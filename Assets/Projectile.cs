using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float lifespan = 5.0f;
    Vector3 movementVector = Vector3.zero;
    float speed = 3.0f;
    
    public void InitializeProjectile(float newLifespan, Vector3 newMovementVector, float newSpeed)
    {
        lifespan = newLifespan;
        newMovementVector.Normalize();
        movementVector = newMovementVector;
        speed = newSpeed;
    }

    public void SetLifespan(float newLifespan)
    {
        lifespan = newLifespan;
    }
    public void SetMovementVector(Vector3 newMovementVector)
    {
        newMovementVector.Normalize();
        movementVector = newMovementVector;
    }
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    // Update is called once per frame
    void Update()
    {
        lifespan -= Time.deltaTime;
        if (lifespan <= 0) Destroy(gameObject);

        transform.Translate(movementVector * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Environment")
            Destroy(gameObject);
        if (collision.gameObject.tag == "Player Weapon")
        {
            // Destroy(gameObject);
            Vector3 hitbackVector = transform.position - collision.ClosestPoint(transform.position);
            hitbackVector.Normalize();

            movementVector = hitbackVector;
            gameObject.tag = "Player Weapon";
            
        }
        if (collision.gameObject.tag == "Player")
            Destroy(gameObject);
    }
}
