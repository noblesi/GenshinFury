using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseItemData : MonoBehaviour
{
    [SerializeField] private Image ItemSprite;
    [SerializeField] private Text ItemCount;

    private void Awake()
    {
        ItemSprite.color = Color.clear;
        ItemCount.text = string.Empty;
    }
}
