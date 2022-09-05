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
    public GameObject StyleHider;
    public TMP_Text styleRankTextMesh;
    public TMP_Text styleText;

    public List<string> stylePhrases;

    public Slider styleSlider;

    [Header("Internal Properties")]
    public float maxStylePoints = 1000;

    [SerializeField] float currentStylePoints = 0.0f;

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

        StyleHider.SetActive(true);
    }

    /// <summary>
    /// 
    /// </summary>
    public void DecreaseStylePoints()
    {
        if (currentStylePoints < 0.0f)
        {
            currentStylePoints = 0;
            StyleHider.SetActive(false);
            return;
        }
        else
        {
            currentStylePoints -= 100 * Time.deltaTime;
        }

       
    }

    /// <summary>
    /// 
    /// </summary>
    private void UIUpdate()
    {
        if (StyleHider.activeSelf == false) return;

        styleSlider.value = currentStylePoints;

        if (currentStylePoints > (maxStylePoints / 2))
        {
            styleRankTextMesh.text = "S";
            styleText.text = stylePhrases[3];
        }
        else if (currentStylePoints > (maxStylePoints / 3))
        {
            styleRankTextMesh.text = "A";
            styleText.text = stylePhrases[2];
        }
        else if(currentStylePoints > (maxStylePoints / 4))
        {
            styleRankTextMesh.text = "B";
            styleText.text = stylePhrases[1];
        }
        else
        {
            styleRankTextMesh.text = "C";
            styleText.text = stylePhrases[0];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //styleSlider = GetComponentInChildren<Slider>();

        styleSlider.maxValue = maxStylePoints;
    }

    // Update is called once per frame
    void Update()
    {
        UIUpdate();
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
            IncreaseStylePoints(100.0f);
        }
    }
}
