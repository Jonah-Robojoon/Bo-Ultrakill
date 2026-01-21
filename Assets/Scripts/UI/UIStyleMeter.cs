using DG.Tweening;
using System.Collections;
using System.Runtime.CompilerServices;
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
    [SerializeField] private Slider styleSlider;
    [SerializeField] private GameObject[] gameObjects;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Canvas Canvas;
    public static UIStyleMeter instance;
    private string currentHitType = "";
    private bool IsScaling = false;
    private int lastRank = -1;




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
        if (stylePoints > 0)
        {
            stylePoints -= styleDecayRate * Time.deltaTime;
        }
        gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
        UpdateRank();

        if (styleSlider != null)
        {
            styleSlider.value = stylePoints;
        }

        if (stylePoints <= 0)
        {
            sr.enabled = false;
            Canvas.enabled = false;
            foreach (GameObject go in gameObjects)
            {
                var meshRenderer = go.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRenderer.enabled = false;
                    
                }
            }
        } 
        else
        {
            foreach (GameObject go in gameObjects)
            { 
                sr.enabled = true;
                Canvas.enabled = true;
                var meshRenderer = go.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRenderer.enabled = true;
                }
            }
        }

        //if (IsScaling) { StartCoroutine(SizeScale()); }

        /*if (stylePoints >= 1000)
        {
            
        }
        else if (stylePoints >= 500)
        {
            IsScaling = true;

        }
        else if (stylePoints >= 250)
        {
            IsScaling = true;

        }
        else if (stylePoints >= 100)
        {
            IsScaling = true;
        }
        else
        {

            

        }*/


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
        int currentRank = -1;
        if (stylePoints >= 1000)
        {
            newSprite = sprites[4];
            currentRank = 4;
        }
        else if (stylePoints >= 500)
        {
            newSprite = sprites[3];
            currentRank = 3;
        }
        else if (stylePoints >= 250)
        {
            newSprite = sprites[2];
            currentRank = 2;
        }
        else if (stylePoints >= 100)
        {
            newSprite = sprites[1];
            currentRank = 1;

        }
        else
        {
            newSprite = sprites[0];
            currentRank = 0;

        }

        if (currentRank != lastRank && currentRank != -1)
        {
            if (!IsScaling)
            {
                StartCoroutine(SizeScale());
            }
        }

        lastRank = currentRank;
    }

    IEnumerator SizeScale()
    {

        IsScaling = true;
        image.transform.DOScale(new Vector3(4.5f, 15f, 4.5f), 0.1f);
        yield return new WaitForSeconds(0.1f);
        image.transform.DOScale(new Vector3(3.2f, 11f, 3.2f), 0.1f);

        yield return new WaitForSeconds(0.1f);

        IsScaling = false;
    }
}
