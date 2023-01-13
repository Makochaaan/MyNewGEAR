using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectScript : MonoBehaviour
{

    public void OnClickStageSelectButton()
    {
        SceneManager.LoadScene("GameScene");
    }

}