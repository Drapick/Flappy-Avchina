using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyAvchinaRotation : MonoBehaviour
{
    [SerializeField] private float rotationSeed;
    [SerializeField] private float takeoffRotation, landingRotation;

    private float rotZ;


    public void ApplyRotation(float velocityY)
    {
        if (rotZ > landingRotation)
        {
            float offset = 1f;
            if (velocityY > 0.5f) offset = velocityY;
            offset = Mathf.Abs(offset);

            rotZ -= rotationSeed * Time.deltaTime / offset;
            transform.localEulerAngles = new Vector3(0, 0, rotZ);
        }
        
    }

    public void StartRotation()
    {
        rotZ = takeoffRotation;
    }
}
