using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class HPFoundation : MonoBehaviour
{
    // 最大HP、現在HP
    public int maxHP, currentHP;
    // HPバー関係
    [SerializeField] private GameObject hpBarPrefab;
    private GameObject myHPBar;
    private RectTransform hpBarRectTransform;
    private Slider hpBarSlider;
    [SerializeField] private bool showHPBar;
    private float hpBarTimer;

    /// <summary>
    /// ダメージ関数。ダメージ数値とそれを表示するための位置を与える
    /// </summary>
    /// <param name="damage">ダメージ</param>
    /// <param name="position">ダメージ表示のための位置情報</param>
    public virtual void Damage(int damage, Vector3 position)
    {
        currentHP -= damage;
        ShowDamage(damage, position);
        if (showHPBar) ShowHPBar();
        if (currentHP <= 0) OnHPZero();
    }
    /// <summary>
    /// HP0の時の関数
    /// </summary>
    protected virtual void OnHPZero()
    {
        if (myHPBar != null)
        {
            myHPBar.SetActive(false);
        }
        Debug.Log("hp zero from base class");
    }
    /// <summary>
    /// 時間差でSetActive(false)する。
    /// </summary>
    /// <param name="delay">何秒後にSetActive(false)するか</param>
    /// <param name="obj">何をSetActive(false)するか</param>
    /// <returns></returns>
    private IEnumerator DeactivateWithDelay(float delay, GameObject obj)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }

    /// <summary>
    /// ダメージ表示関数。UIにダメージを表示する。
    /// </summary>
    /// <param name="damage">ダメージ</param>
    /// <param name="position">ヒット場所</param>
    public void ShowDamage(int damage, Vector3 position)
    {
        PooledUI damageText = ObjectPoolUI.sharedInstance.GetPooledObject("DamageText");
        damageText.uiObject.GetComponent<TextMeshProUGUI>().text = damage.ToString();
        damageText.positionTemp = position;
        damageText.uiObject.SetActive(true);
        StartCoroutine(DeactivateWithDelay(damageText.duration, damageText.uiObject));
    }
    /// <summary>
    /// ダメージ時にHPバーを表示する
    /// </summary>
    public void ShowHPBar()
    {
        //まだ自分のHPバーを持っていないなら生成する
        if(myHPBar==null)
        {
            myHPBar = Instantiate(hpBarPrefab);
            myHPBar.transform.SetParent(ObjectPoolUI.sharedInstance.transform);
            hpBarRectTransform = myHPBar.GetComponent<RectTransform>();
            hpBarSlider = myHPBar.GetComponent<Slider>();
        }
        hpBarSlider.maxValue = maxHP;
        hpBarSlider.minValue = 0;
        hpBarSlider.value = currentHP;
        myHPBar.SetActive(true);
        hpBarTimer = 0;
    }
    protected virtual void Update()
    {
        if (myHPBar != null && myHPBar.activeInHierarchy)
        {
            hpBarTimer += Time.deltaTime;
            //StartCoroutine(Deact...3,myHPBar)とすると、3秒に1回一瞬消える感じになる
            if (hpBarTimer >= 3) StartCoroutine(DeactivateWithDelay(0, myHPBar));
            hpBarRectTransform.position = Camera.main.WorldToScreenPoint(transform.position);
        }
    }
}
