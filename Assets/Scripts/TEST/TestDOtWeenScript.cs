using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDOtWeenScript : MonoBehaviour
{
    public CanvasGroup UIcanvas;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            UIcanvas.DOFade(0, 1);
    }
}
