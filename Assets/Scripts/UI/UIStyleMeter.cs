using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class UIStyleMeter : MonoBehaviour
{
    //[SerializeField] private TMPro.TextMeshPro StyleRankText;
    [SerializeField] private float stylePoints = 0f;
    [SerializeField] private float styleDecayRate = 5f;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Sprite newSprite;
    [SerializeField] private GameObject image;
    [SerializeField] private float shakeAmount = 2f;
    public static UIStyleMeter instance;
    private string currentHitType = "";



    private void Awake()
    {
        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*if (stylePoints > 0)
        {
            stylePoints -= styleDecayRate * Time.deltaTime;
        }*/
        gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
        UpdateRank();
    }

    public void AddStyle(float amount)
    {
        stylePoints += amount;
    }

    public void WhatHit(string hitType)
    {
        currentHitType = hitType;
    }

    void UpdateRank()
    {
        /*if (stylePoints >= 1000)
        {
            StyleRankText.text = "S";
        }
        else if (stylePoints >= 500)
        {
            StyleRankText.text = "<size=70><color=orange>A</color></size>NARCHIC";
        }
        else if (stylePoints >= 250)
        {
            StyleRankText.text = "<size=70><color=yellow>B</color></size>RUTAL";
        }                              
        else if (stylePoints >= 100)   
        {                              
            StyleRankText.text = "<size=70><color=green>C</color></size>HAOTIC";
        }                             
        else                          
        {                             
            StyleRankText.text = "<size=70><color=blue>D</color></size>ESTRUCTIVE";
        }*/
        if (stylePoints >= 1000)
        {
            newSprite = sprites[4];
        }
        else if (stylePoints >= 500)
        {
            newSprite = sprites[3];
        }
        else if (stylePoints >= 250)
        {
            newSprite = sprites[2];
        }
        else if (stylePoints >= 100)
        {
            newSprite = sprites[1];
            image.transform.DOScale(new Vector3(4f, 11.7f, 4f), 0.5f);

        }
        else
        {
            newSprite = sprites[0];

        }
    }

    /*IEnumerator SizeScale()
    {
        
    }*/
}
