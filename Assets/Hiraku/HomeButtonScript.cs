using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeButtonScript : MonoBehaviour
{
    public void OnClickHomeButton()
    {
        StartCoroutine("LoadTitleScene");
    }

    IEnumerator LoadTitleScene()
    {
        yield return new WaitForSeconds(0.35f);
        SceneManager.LoadScene("TitleScene");
    }
}
