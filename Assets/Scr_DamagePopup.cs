using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scr_DamagePopup : MonoBehaviour
{

    private TMP_Text text; // Access text mesh properties
    public Color textColor; // change alpha -> fade out
    private Transform playerTransform; // look towards the player..

    private float m_Lifetime = 1f;
    private Vector3 MoveDir = Vector3.zero;
    private Vector3 CurMoveDir = Vector3.zero;
    
    [SerializeField] AnimationCurve MoveUpCurve;
    [SerializeField] AnimationCurve ColorCurve;
    [SerializeField] Color m_Color;
    [SerializeField] Color m_Transparent;
    public void Init(int _Damage)
    {
       
        text = GetComponentInChildren<TMP_Text>();
        playerTransform = Camera.main.transform;
       
        //textColor = text.color;
        MoveDir = new Vector3(Random.Range(-1, 1), 1, 0);
        CurMoveDir = Vector3.zero;
        text.SetText(_Damage.ToString());
        transform.localPosition = new Vector3(transform.localPosition.x + 1, transform.localPosition.y + 1, 0.0f);
        transform.parent = null;
    }
    private void LateUpdate()
    {
        m_Lifetime -= Time.deltaTime;


        transform.LookAt(2 * transform.position - playerTransform.position);

        transform.rotation *= Quaternion.Euler(Random.Range(-6,6) * m_Lifetime,Random.Range(-6,6) * m_Lifetime,Random.Range(-6,6) * m_Lifetime);
        text.color = Color.Lerp(m_Transparent,m_Color,m_Lifetime);
        transform.position += Vector3.up * MoveUpCurve.Evaluate(m_Lifetime);

        if (m_Lifetime <= 0)
        {
            Destroy(gameObject);
        }

        // dissapearTimer -= Time.deltaTime;
        // if (dissapearTimer <= 0f)
        // {
        //     textColor.a -= fadeOutSpeed * Time.deltaTime;
        //     text.color = textColor;
        //     if (textColor.a <= 0f)
        //     {
        //         Destroy(gameObject);
        //     }
        // }


        
    }
   
}
