using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class WeaponTitleDisplay : MonoBehaviour
{
    TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    public void ShowTitle(string newName)
    {
        text.text = newName;
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        Color endColor = new Color(text.color.r, text.color.g, text.color.b, 1);
        text.DOColor(endColor, 0.5f).SetEase(Ease.OutSine).SetLoops(2, LoopType.Yoyo);
    }



}
