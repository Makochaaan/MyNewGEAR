using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonScript : MonoBehaviour
{


    public void OnClickStartButton()
    {
        StartCoroutine("LoadStageSelectScene");
    }

    IEnumerator LoadStageSelectScene()
    {
        yield return new WaitForSeconds(0.35f);
        SceneManager.LoadScene("StageSelectScene");
    }

}
