using System.Collections;
using UnityEngine;

[AddComponentMenu("Global Game Jam 2021/Graphics/Gun Head Graphics")]
public class GunHeadGraphics : HeadGraphics
{
    [Header("Gun Head Graphics")]
    [SerializeField] Transform objectToRotate = default;

    Coroutine rotateCannonCoroutine;

    #region private API

    protected override void OnPickHead()
    {
        base.OnPickHead();

        if (rotateCannonCoroutine != null)
            StopCoroutine(rotateCannonCoroutine);

        rotateCannonCoroutine = StartCoroutine(RotateCannonCoroutine());
    }

    IEnumerator RotateCannonCoroutine()
    {
        while(headPlayer.Owner)
        {
            //rotate to aim position
            float angle = Vector2.SignedAngle(Vector2.right, headPlayer.Owner.DirectionPlayer);
            objectToRotate.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            yield return null;
        }
    }

    #endregion
}
