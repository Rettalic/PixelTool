using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PixelizerShowcase : MonoBehaviour
{
    [SerializeField] private Material mat;
    [SerializeField] private Texture texture;
    [SerializeField] private int pixelSize;
    private Pixelizer pixelizer;

    public Slider slider;
    public TMP_Text text;

    private void Awake()   
    {
        pixelizer = new Pixelizer();
    }

    private void Start()
    {
        UpdateText(slider.value);
        slider.onValueChanged.AddListener(UpdateText);
    }

    void Update()
    {
    }

    private void UpdateText(float _val)
    {
        mat.SetTexture("_MainTex", pixelizer.Pixelize(texture, pixelSize));
        text.text = slider.value.ToString("0,00");
        pixelSize = (int)_val;
    }
}
