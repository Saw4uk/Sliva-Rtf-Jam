using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaweInfoDrawer : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject onStartWawe;
    [SerializeField] private GameObject onEndWawe;
    [SerializeField] private TMP_Text waweNumber;

    public void DrawWawe(int waweNumber)
    {
        onStartWawe.gameObject.SetActive(true);
        onEndWawe.gameObject.SetActive(false);
        this.waweNumber.text = waweNumber.ToString();
    }

    public void DrawNoWawe(float timeInSeconds)
    {
        onEndWawe.gameObject.SetActive(true);
        onStartWawe.gameObject.SetActive(false);
        slider.maxValue = timeInSeconds;
        slider.value = timeInSeconds;
        StartCoroutine(DrawProgress());
    }
    
    private IEnumerator DrawProgress() 
    {
        while (slider.value >= 0.3)
        {
            slider.value -= Time.deltaTime;
            yield return null;
        }
    }
    
}
