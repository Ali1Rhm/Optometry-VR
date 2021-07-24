using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleColor : MonoBehaviour
{
    [SerializeField] private Color mainColor;
    [SerializeField] private Color secondColor;

    public void ChangeColor()
    {
        Color _currentColor = this.GetComponent<Image>().color;

        if (_currentColor == mainColor)
            this.GetComponent<Image>().color = secondColor;
        else
            this.GetComponent<Image>().color = mainColor;
    }
}
