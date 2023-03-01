using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class HPFoundation : MonoBehaviour
{
    public int maxHP, currentHP;
    [SerializeField] private GameObject hpBarPrefab;
    private GameObject myHPBar;
    private RectTransform hpBarRectTransform;
    private Slider hpBarSlider;
    [SerializeField] private bool showHPBar;
    private float hpBarTimer;

    public virtual void Damage(float damage, Vector3 position)
    {
        currentHP = Mathf.RoundToInt(currentHP - damage);
        ShowDamage(Mathf.RoundToInt(damage), position);
        if (showHPBar) ShowHPBar();
        if (currentHP <= 0)
        {
            OnHPZero();
        }
    }
    protected virtual void OnHPZero()
    {
        Debug.Log("hp zero from base class");
    }

    
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
        foreach (PooledUI pooledUI in ObjectPoolUI.sharedInstance.pooledUIs)
        {
            if (pooledUI.uiObject.activeInHierarchy)
            {
                switch (pooledUI.uiObject.name)
                {
                    case "DamageText":
                        pooledUI.myTransform.position = Camera.main.WorldToScreenPoint(pooledUI.positionTemp);
                        break;
                    default:
                        break;
                }
                
            }
        }
        if (myHPBar != null && myHPBar.activeInHierarchy)
        {
            hpBarTimer += Time.deltaTime;
            if (hpBarTimer >= 3) StartCoroutine(DeactivateWithDelay(0, myHPBar));
            hpBarRectTransform.position = Camera.main.WorldToScreenPoint(transform.position);
        }
    }
}
