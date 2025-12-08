using UnityEngine;
using DG.Tweening;
public class Interaction_PipeLever : Interaction_Obj
{
    public GameObject RotatePipe;
    public GameObject UpPlatform;
    public Vector3 rotate;
    public Vector3 Up;
    public float rotateTime;
    public float upTime;
    public bool On = true;
    Sequence MySequence;
    public Sprite sprite;

    public override void OnInteract(PlayerInputHandler PlayerInput)
    {

        if (On)
        {
            sr.sprite = sprite;
            MySequence = DOTween.Sequence();

            // 2. Append: 첫 번째 동작 (회전)을 줄에 세웁니다.
            MySequence.Append(RotatePipe.transform.DORotate(rotate, rotateTime));

            // 3. Append: 두 번째 동작 (이동)을 그 뒤에 줄 세웁니다.
            // 앞의 회전이 다 끝나야 실행됩니다.
            MySequence.Append(UpPlatform.transform.DOMove(Up, upTime).SetRelative());

            On = false;
        }
    }
}
