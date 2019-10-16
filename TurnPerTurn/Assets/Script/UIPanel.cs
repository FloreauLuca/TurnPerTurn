using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Slider pv;
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private GameObject statutFreezeText;
    [SerializeField] private GameObject statutPoisonText;

    public void SetValue(string name, float maxPV, float currentPV, string type, bool freezed, bool poisonned)
    {
        nameText.text = name;
        pv.maxValue = maxPV;
        pv.value = currentPV;
        typeText.text = type;
        statutFreezeText.SetActive(freezed);
        statutPoisonText.SetActive(poisonned);
    }
}
