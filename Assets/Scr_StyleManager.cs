using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Scr_StyleManager : MonoBehaviour
{
    public static Scr_StyleManager i;

    public void Awake()
    {
        if (i == null)
        {
            i = this;
        }
        else {
            Destroy(gameObject);
        }
    }
    [Header("UI Elements")]
    public TMP_Text styleRankText;
    public TMP_Text styleText;

    public List<string> stylePhrases;

    public Slider styleSlider;

    [Header("Internal Properties")]
    public float maxStylePoints = 1000;

    private float currentStylePoints;

    private float maxWaitTime = 5.0f;
    private float currentWaitTime = 0.0f;

   
    /// <summary>
    /// 
    /// </summary>
    /// <param name="_increaseAmount"></param>
    public void IncreaseStylePoints(float _increaseAmount)
    {
        currentStylePoints += _increaseAmount;
        currentWaitTime = maxWaitTime;
    }

    public void DecreaseStylePoints()
    {
        if (currentStylePoints < 0.0f)
        {
            currentStylePoints = 0; 
            return;
        }

        currentStylePoints -= 3 * Time.deltaTime;
    }


    private void UIUpdate()
    {
        styleSlider.value = currentStylePoints;


    }
    // Start is called before the first frame update
    void Start()
    {
        styleSlider = GetComponentInChildren<Slider>();

        styleSlider.maxValue = maxStylePoints;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWaitTime > 0.0f)
        {
            currentWaitTime -= Time.deltaTime;
        }
        else if (currentWaitTime <= 0.0f)
        {
            DecreaseStylePoints();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            IncreaseStylePoints(10.0f);
        }
    }
}
