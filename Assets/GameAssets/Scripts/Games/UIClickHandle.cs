using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIClickHandle : MonoBehaviour
{
    public GameObject pointClick;
    private Image m_imgPoint;
    private Sequence m_pointAnimSeq;

    private void Awake()
    {
        m_pointAnimSeq = DOTween.Sequence();
        m_imgPoint = pointClick.GetComponent<Image>();
        pointClick.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pointClick.transform.position = Input.mousePosition;
            PointClickAnim();
        }
    }


    void PointClickAnim()
    {
        pointClick.SetActive(true);
        Color c = m_imgPoint.color;
        c.a = 1f;
        pointClick.transform.localScale = Vector3.zero;
        m_imgPoint.color = c;
        c.a = 0f;
        if(m_pointAnimSeq != null)
            m_pointAnimSeq.Kill();
        m_pointAnimSeq = DOTween.Sequence();
        m_pointAnimSeq.Join(pointClick.transform.DOScale(Vector3.one, 0.5f));
        m_pointAnimSeq.Join(m_imgPoint.DOColor(c, 0.5f));
        m_pointAnimSeq.OnComplete(() =>
        {
            pointClick.SetActive(false);
        });
    }
}
