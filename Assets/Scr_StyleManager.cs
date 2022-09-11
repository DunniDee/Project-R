using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Scr_StyleManager : MonoBehaviour
{
    public static Scr_StyleManager i;

   
    [Header("UI Elements")]
    public GameObject StyleHider;
    public TMP_Text styleRankTextMesh;
    public TMP_Text styleText;

    public List<string> stylePhrases;

    public Slider styleSlider;

    [Header("Internal Properties")]
    public float maxStylePoints = 1000;

    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioClip> styleAudioClips;

    [SerializeField] float currentStylePoints = 0.0f;

    private float maxWaitTime = 5.0f;
    private float currentWaitTime = 0.0f;

    bool hasPlayedC = false;
    bool hasPlayedB = false;
    bool hasPlayedA = false;
    bool hasPlayedS = false;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_increaseAmount"></param>
    public void IncreaseStylePoints(float _increaseAmount)
    {
        if ((currentStylePoints + _increaseAmount) > maxStylePoints)
        {
            currentWaitTime = maxWaitTime;
            return;
        }
        if (StyleHider.activeSelf == false)
        {
            StyleHider.SetActive(true);
        }

        currentStylePoints += _increaseAmount;
        currentWaitTime = maxWaitTime;



        if (currentStylePoints > (maxStylePoints / 2) && !hasPlayedS)
        {
            audioSource.PlayOneShot(styleAudioClips[3]);
            hasPlayedS = true;
            hasPlayedB = false;
            hasPlayedA = false;
            hasPlayedC = false;
        }
        else if (currentStylePoints > (maxStylePoints / 3) && !hasPlayedA)
        {
            audioSource.PlayOneShot(styleAudioClips[2]);
            hasPlayedS = false;
            hasPlayedB = false;
            hasPlayedA = true;
            hasPlayedC = false;
        }
        else if (currentStylePoints > (maxStylePoints / 4) && !hasPlayedB)
        {

            audioSource.PlayOneShot(styleAudioClips[1]);
            hasPlayedS = false;
            hasPlayedB = true;
            hasPlayedA = false;
            hasPlayedC = false;
        }
        else if(currentStylePoints < (maxStylePoints / 4) && !hasPlayedC)
        {

            audioSource.PlayOneShot(styleAudioClips[0]);
            hasPlayedS = false;
            hasPlayedB = false;
            hasPlayedA = false;
            hasPlayedC = true;
        }
    }

    void PlayStyleAudio(ref bool _hasPlayedAudio)
    { 
        
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
        else if (currentStylePoints > (maxStylePoints / 4))
        {
            styleRankTextMesh.text = "B";
            styleText.text = stylePhrases[1];
        }
        else if (styleRankTextMesh.text != "C")
        {
            styleRankTextMesh.text = "C";
            styleText.text = stylePhrases[0];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (i == null)
        {
            i = this;
            //audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Awake()
    {
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
