using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class UIFillBar : MonoBehaviour
{
    private Image _maskImage;

    public TMP_Text valueText;

    [Range(0,1)] public float value;

    private float barValue;

    public float minValue;
    public float maxValue;

    private void OnValidate()
    {
        if (_maskImage)
        {
            SetValue(value * 100);
        }
    }

    public void SetValue(float newValue)
    {

        if (_maskImage == null)
            return;
        barValue = newValue;

        value = Mathf.InverseLerp(minValue, maxValue, newValue);

        valueText.text = $"{newValue:G4}%";
        
        _maskImage.fillAmount = value;
    }
    
    // Start is called before the first frame update
    void OnEnable()
    {
        _maskImage = GetComponentInChildren<Mask>().GetComponent<Image>();
    }

}
