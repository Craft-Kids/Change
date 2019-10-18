using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherMoveScript : MonoBehaviour
{
    public Transform[] path;
    public List<Transform[]> list = new List<Transform[]>();

    public GameObject move;

    public iTween Tween;
    Rigidbody rigid;

    float MoverotateY = 0;

    Vector3 pos;

    void OnDrawGizmos()
    {
        iTween.DrawPath(path);
    }

    void Start()
    {
        Tween = GetComponent<iTween>();
        rigid = move.GetComponent<Rigidbody>();

        pos = new Vector3();

        // 라인따라 이동함
        iTween.MoveTo(gameObject, iTween.Hash("path", path, "time", 50, "easetype", iTween.EaseType.linear,
            "orienttopath", true, "looktime", .6, "looptype", iTween.LoopType.loop, "movetopath", false));
    }

    private void Update()
    {

    }
}
