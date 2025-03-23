using DG.Tweening;
using UnityEngine;
using Util;

public class ShakeCamera : MonoBehaviour
{
    private void Start()
    {
        IntEventSystem.Register(GameEventEnum.GenerateShapeDebris, DoShakeCamera);
    }

    private void DoShakeCamera(object _)
    {
        transform.DOShakePosition(0.3f, 0.2f);
    }
}