using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampTransform : MonoBehaviour
{
    // x軸方向の移動範囲の最小値
    [SerializeField] private float min_X = -1;

    // x軸方向の移動範囲の最大値
    [SerializeField] private float max_X = 1;

    // y軸方向の移動範囲の最小値
    [SerializeField] private float min_Y = -1;

    // y軸方向の移動範囲の最大値
    [SerializeField] private float max_Y = 1;

    // z軸方向の移動範囲の最小値
    [SerializeField] private float min_Z = -1;

    // z軸方向の移動範囲の最大値
    [SerializeField] private float max_Z = 1;

    private void Update()
    {
        var pos = transform.position;

        // x軸方向の移動範囲制限
        pos.x = Mathf.Clamp(pos.x, min_X, max_X);

        // y軸方向の移動範囲制限
        pos.y = Mathf.Clamp(pos.y, min_Y, max_Y);

        // z軸方向の移動範囲制限
        pos.z = Mathf.Clamp(pos.z, min_Z, max_Z);

        transform.position = pos;
    }
}
