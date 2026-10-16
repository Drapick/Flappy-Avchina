using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimationControler : MonoBehaviour
{
    public Animator anim;
    public List<Animator> allAnimators;

    public void PlayCLICKAnimation() {
        if (anim != null) anim.SetTrigger("Click");
    }

    public void AnimatorsCLICKALL()
    {
        // Перебираем каждый аниматор в списке
        foreach (Animator a in allAnimators)
        {
            if (a != null) // Проверка, чтобы не было ошибки, если слот пустой
            {
                a.SetTrigger("Click");
            }
        }
    }
}
