using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//ó���������� ������ �������� �ڷ�ƾ Ŭ����

//StartCoroutine([Ŭ������ ���� ���� �̸�].FadeOut()); ���� ��� ����.

public class HitUICorutine : MonoBehaviour
{
    public Image hitImage;
    public float time = 0.5f;

    public IEnumerator FadeOut() //�� �ڷ�ƾ���� ���İ� ����
    {
        Color initialColor = hitImage.color;
        initialColor.a = 230f / 255f; // Alpha ���� 230���� ���� (0~255�� ������ 0~1�� ������ ��ȯ)
        hitImage.color = initialColor;


        float startAlpha = hitImage.color.a; //��ŸƮ ���İ��� ��� ������� 230f

        float endTime = 0f;

        while (endTime < time) //0.5�ʿ��� 0�ʰ� �ɶ�����
        {
            endTime += Time.deltaTime; //�����Ӵ����� ����ָ鼭

            float newAlpha = Mathf.Lerp(startAlpha, 0f, endTime / time); //������ ��Ƽ� �ǽð� ���İ� ����

            Color newColor = hitImage.color; //���ο� Į�� ����
            newColor.a = newAlpha; //���İ��� ����ְ�...
            hitImage.color = newColor; //����

            yield return null; //GPT: �ڷ�ƾ�� �Ͻ������ϰ� �� ������ ���. �ε巯�� �ִϸ��̼��� ���� �ʿ��ϴ�.
        }

        // ���������� ������ �����ϰ� ����
        Color finalColor = hitImage.color;
        finalColor.a = 0f;
        hitImage.color = finalColor; //���İ��� 0�� ������ �÷��� �ָ� ������
    }

}
