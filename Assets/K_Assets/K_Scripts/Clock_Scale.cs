using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock_Scale : MonoBehaviour
{
    public RectTransform uiElement;
    public Vector3 initialScale = new Vector3(2, 2, 1);  // �ʱ� ũ�� (ū ũ��)
    public Vector3 finalScale = new Vector3(1, 1, 1);    // ���� ũ�� (�۾��� �� ������ ũ��)
    public float duration = 1f;                          // ũ�Ⱑ �۾����� �� �ɸ��� �ð�
    public float delay = 8f;                             // ���� �� ���� �ð�
    public float stayDuration = 1f;                      // �ʱ� ũ�� ���� �ð�

    void Start()
    {
        // ���� �� UI ��Ҹ� ��Ȱ��ȭ
        uiElement.localScale = Vector3.zero;
        uiElement.gameObject.SetActive(false);

        // ������ �Ŀ� ����
        StartCoroutine(StartScaling());
    }

    IEnumerator StartScaling()
    {
        // ������ ����
        yield return new WaitForSeconds(delay);

        // UI ��� Ȱ��ȭ
        uiElement.gameObject.SetActive(true);

        // �ʱ� ũ�⿡�� ��� (stayDuration ���� ����)
        uiElement.localScale = initialScale;
        yield return new WaitForSeconds(stayDuration);

        // ���� ũ��� ������ �پ��
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            uiElement.localScale = Vector3.Lerp(initialScale, finalScale, time / duration);
            yield return null;
        }

        // ���� ũ��� ����
        uiElement.localScale = finalScale;
    }
}

