using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectScript : MonoBehaviour
{

    public void OnClickStageSelectButton()
    {
        StartCoroutine("LoadGameScene");
    }

    IEnumerator LoadGameScene()
    {
        yield return new WaitForSeconds(0.35f);
        SceneManager.LoadScene("GameScene");
    }

}