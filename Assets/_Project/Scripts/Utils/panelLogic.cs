using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class panelLogic : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void EnableObject() => gameObject.SetActive(true);
    public void DisableObject() => gameObject.SetActive(false);

}
