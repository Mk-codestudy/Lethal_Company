using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock_Scale : MonoBehaviour
{
    public RectTransform targetUI; // �ִϸ��̼� ������ UI�� RectTransform
    public Vector3 startScale = new Vector3(2f, 2f, 1f); // �ʱ� ũ�� (2, 2, 1)
    public Vector3 endScale = new Vector3(1f, 1f, 1f); // ���� ũ�� (1, 1, 1)
    public float appearDuration = 1.0f; // ��Ÿ���� �ִϸ��̼� �ð�
    public float stayDuration = 1.0f; // ���� ũ�⿡�� �����Ǵ� �ð�
    public float initialDelay = 8.0f; // 8�� ��� �ð�

    void Start()
    {
        targetUI.localScale = startScale; // UI�� �ʱ� ũ�� ����
        targetUI.gameObject.SetActive(false); // ó���� ��Ȱ��ȭ
        StartCoroutine(AnimateUI());
    }

    IEnumerator AnimateUI()
    {
        yield return new WaitForSeconds(initialDelay); // 8�� ���

        targetUI.gameObject.SetActive(true); // UI Ȱ��ȭ

        // ��Ÿ���� �ִϸ��̼�
        float elapsedTime = 0f;
        while (elapsedTime < appearDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / appearDuration;
            targetUI.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        targetUI.localScale = endScale; // ���� ũ��� ����

        yield return new WaitForSeconds(stayDuration); // ���� ũ�⿡�� 1�� ����
        // ���Ŀ��� ���� ũ��� ������ ���·� �����ְ� �˴ϴ�.
    }
}

