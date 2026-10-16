using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    public float delay = 3f;      // Через сколько секунд включить
    public GameObject target;     // Какой объект включить

    void Start()
    {
        if (target != null)
        {
            StartCoroutine(ActivateAfterDelay());
        }
    }

    IEnumerator ActivateAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        target.SetActive(true);
    }
}
