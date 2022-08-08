using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scr_DamageIndicator : MonoBehaviour
{
    public float lifetime = 0.6f;
    public TMP_Text text;

    public float minDist = 2f;
    public float maxDist = 5f;

    private Vector3 iniPos;
    private Vector3 targetPos;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TMP_Text>();
        transform.LookAt(2 * transform.position - Camera.main.transform.position);

        float direction = Random.rotation.eulerAngles.z;
        iniPos = transform.position;
        float dis = Random.Range(minDist, maxDist);
        targetPos = iniPos + (Quaternion.Euler(0, 0, direction) * new Vector3(dis, dis, 0f));
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        float fraction = lifetime / 2;

        if (timer > lifetime)
        {
            Destroy(gameObject);
        }
        else if (timer > fraction)
        {
            text.color = Color.Lerp(text.color, Color.clear, (timer - fraction) / (lifetime - fraction));
        }
        transform.position = Vector3.Lerp(iniPos, targetPos, Mathf.Sin(timer / lifetime));
        transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, Mathf.Sin(timer / lifetime));
        
    }

    public void SetDamageText(int _Damage)
    {
        text.text = _Damage.ToString() ;
    }
}
