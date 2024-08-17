using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock_Scale : MonoBehaviour
{
    public RectTransform targetUI; // 애니메이션 적용할 UI의 RectTransform
    public Vector3 startScale = new Vector3(2f, 2f, 1f); // 초기 크기 (2, 2, 1)
    public Vector3 endScale = new Vector3(1f, 1f, 1f); // 최종 크기 (1, 1, 1)
    public float appearDuration = 1.0f; // 나타나는 애니메이션 시간
    public float stayDuration = 1.0f; // 최종 크기에서 유지되는 시간
    public float initialDelay = 8.0f; // 8초 대기 시간

    void Start()
    {
        targetUI.localScale = startScale; // UI의 초기 크기 설정
        targetUI.gameObject.SetActive(false); // 처음에 비활성화
        StartCoroutine(AnimateUI());
    }

    IEnumerator AnimateUI()
    {
        yield return new WaitForSeconds(initialDelay); // 8초 대기

        targetUI.gameObject.SetActive(true); // UI 활성화

        // 나타나는 애니메이션
        float elapsedTime = 0f;
        while (elapsedTime < appearDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / appearDuration;
            targetUI.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        targetUI.localScale = endScale; // 최종 크기로 설정

        yield return new WaitForSeconds(stayDuration); // 최종 크기에서 1초 유지
        // 이후에는 최종 크기로 유지된 상태로 남아있게 됩니다.
    }
}

