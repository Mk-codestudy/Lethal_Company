using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock_Scale : MonoBehaviour
{
    public RectTransform uiElement;
    public Vector3 initialScale = new Vector3(2, 2, 1);  // 초기 크기 (큰 크기)
    public Vector3 finalScale = new Vector3(1, 1, 1);    // 최종 크기 (작아진 후 유지할 크기)
    public float duration = 1f;                          // 크기가 작아지는 데 걸리는 시간
    public float delay = 8f;                             // 시작 전 지연 시간
    public float stayDuration = 1f;                      // 초기 크기 유지 시간

    void Start()
    {
        // 시작 시 UI 요소를 비활성화
        uiElement.localScale = Vector3.zero;
        uiElement.gameObject.SetActive(false);

        // 딜레이 후에 시작
        StartCoroutine(StartScaling());
    }

    IEnumerator StartScaling()
    {
        // 딜레이 적용
        yield return new WaitForSeconds(delay);

        // UI 요소 활성화
        uiElement.gameObject.SetActive(true);

        // 초기 크기에서 대기 (stayDuration 동안 유지)
        uiElement.localScale = initialScale;
        yield return new WaitForSeconds(stayDuration);

        // 최종 크기로 서서히 줄어듦
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            uiElement.localScale = Vector3.Lerp(initialScale, finalScale, time / duration);
            yield return null;
        }

        // 최종 크기로 설정
        uiElement.localScale = finalScale;
    }
}

