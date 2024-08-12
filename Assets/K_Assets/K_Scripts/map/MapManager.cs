using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MapManager : MonoBehaviour
{
    public MapState currentMapState;

    public Transform player;
    public List<Transform> enemies;


    public void SaveStartPlayerPosition()
    {
        currentMapState.playerPosition = new Vector3(-69.5f, 5.14f, -53.3f);
        currentMapState.playerRotation = Quaternion.Euler(0, -90, 0);
    }

    public void SaveMapState()
    {
        // �÷��̾� ��ġ�� ȸ�� ����
        currentMapState.playerPosition = player.position;
        currentMapState.playerRotation = player.rotation;

        // ������ ��ġ�� ȸ�� ����
        currentMapState.enemyPositions.Clear();
        currentMapState.enemyRotations.Clear();
        foreach (Transform enemy in enemies)
        {
            currentMapState.enemyPositions.Add(enemy.position);
            currentMapState.enemyRotations.Add(enemy.rotation);
        }


    }

    public void LoadMapState()
    {
        // �÷��̾� ��ġ�� ȸ�� �ҷ�����
        player.position = currentMapState.playerPosition;
        player.rotation = currentMapState.playerRotation;

        // ������ ��ġ�� ȸ�� �ҷ�����
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].position = currentMapState.enemyPositions[i];
            enemies[i].rotation = currentMapState.enemyRotations[i];
        }

    }


}
