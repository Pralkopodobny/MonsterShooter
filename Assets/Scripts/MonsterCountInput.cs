using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonsterCountInput : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    [SerializeField] Slider slider;

    private void Start()
    {
        slider.value = Options.count;
        UpdateCount();
    }

    public void UpdateCount()
    {
        string newText = slider.value.ToString();
        text.text = newText;
        Options.count = (int)slider.value;
    }

}
