using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetReadyAnimatorController : MonoBehaviour
{
    public List<Animator> allAnimators;

    public void PlayClickAnimationOnAll()
    {
        foreach (Animator currentAnimator in allAnimators)
        {
            if (currentAnimator != null) 
            {
                currentAnimator.SetTrigger("Click");
            }
        }
    }
}
