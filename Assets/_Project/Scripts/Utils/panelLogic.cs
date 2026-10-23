using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class panelLogic : MonoBehaviour
{
    public void OnFadeCompleteLoadScene(string sceneName)
    {
        GameManager.Instance.ChangeScene(sceneName);
    }
    
    public void RestartThisScene()
    {
        GameManager.Instance.RestartScene();
    }

    public void EnableObject() => gameObject.SetActive(true);
    public void DisableObject() => gameObject.SetActive(false);

}
