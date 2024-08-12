using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapState", menuName = "ScriptableObjects/MapState", order = 1)]
public class MapState : ScriptableObject
{
    public Vector3 playerPosition;
    public Quaternion playerRotation;

    public List<Vector3> enemyPositions;
    public List<Quaternion> enemyRotations;

    public List<bool> interactableStates; // 예: 문이 열려있는지, 아이템이 수집되었는지 등
}
