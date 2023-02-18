using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampCircle : MonoBehaviour
{
    // 移動範囲の最大半径
    [SerializeField] private float max_Radius = 1;

    private void Update()
    {
        var pos = transform.position;

        // 指定された半径の円内に位置を丸める
        var clampedPos = Vector3.ClampMagnitude(pos, max_Radius);

        transform.position = new Vector3(clampedPos.x, pos.y, clampedPos.z);
    }
}
