using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LicenseScene : MonoBehaviour
{
    private AsyncOperation loadNextScene;
    [SerializeField] float loadDelay;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadCoroutine(loadDelay));
    }
    private IEnumerator LoadCoroutine(float delay)
    {
        SEManager.SharedInstance.PlaySE("AnnounceLicense", false, Vector3.zero);
        loadNextScene = SceneManager.LoadSceneAsync("TitleScene");
        loadNextScene.allowSceneActivation = false;
        yield return new WaitForSeconds(delay);
        loadNextScene.allowSceneActivation = true;
    }
    // このくらいなら一般化した方が良いか?
}
