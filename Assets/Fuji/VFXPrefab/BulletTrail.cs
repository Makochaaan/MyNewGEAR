using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    private LineRenderer myLine;
    [SerializeField] private float duration;
    private float elapsedTime;
    // Start is called before the first frame update
    void Start()
    {
        myLine = GetComponent<LineRenderer>();
    }
    private void OnEnable()
    {
        if (myLine != null)
        {
            myLine.startWidth = 1;
            myLine.endWidth = 1;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!myLine.gameObject.activeSelf)
        {
            return;
        }
        else
        {
            elapsedTime += Time.deltaTime;
            myLine.startWidth = 1 - (elapsedTime / duration);
            myLine.endWidth = 1 - (elapsedTime / duration);
        }
    }
    private void OnDisable()
    {
        elapsedTime = 0;
    }
}
