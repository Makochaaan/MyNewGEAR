using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneScript : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private string nextSceneName;
    public void ChangeScene()
    {
        StartCoroutine("ChangeSceneCoroutine");
    }

    IEnumerator ChangeSceneCoroutine()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(nextSceneName);
    }

}
