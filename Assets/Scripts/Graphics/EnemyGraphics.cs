using UnityEngine;

[AddComponentMenu("Global Game Jam 2021/Graphics/Enemy Graphics")]
public class EnemyGraphics : CharacterGraphics
{
    [Header("Gun Head Graphics")]
    [SerializeField] Transform objectToRotate = default;

    Enemy enemy;

    protected override void Awake()
    {
        base.Awake();

        enemy = character as Enemy;
    }

    void Update()
    {
        //rotate to aim position
        float angle = Vector2.SignedAngle(Vector2.right, enemy.DirectionPlayer);
        objectToRotate.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
