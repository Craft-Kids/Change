using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    public Transform[] path;
    public List<Transform[]> list = new List<Transform[]>();
    public GameObject move;
    public float horizonspeed; //좌우이동 속도
    public float speed = 50; //현재 속도

    bool conercheck = false; //코너 체크
    bool speciallinecheck = false;  //특수주행 체크

    Rigidbody rigid;

    float MoverotateY = 0;
    int lineState = 0;  // 0 : 기본주행, 1: 코너주행, 2:특수주행

    Vector3 pos;
    float delta = 0;
    float echo = 0;
    iTween time;

    void OnDrawGizmos()
    {
        iTween.DrawPath(path);
    }


    void Start()
    {
        rigid = move.GetComponent<Rigidbody>();
        //delta = 20;
        pos = new Vector3();

        // 라인따라 이동함
        //iTween.MoveTo()
        iTween.MoveTo(gameObject, iTween.Hash("path", path, "time", speed, "easetype", iTween.EaseType.linear,
            "orienttopath", true, "looktime", .6, "looptype", iTween.LoopType.loop, "movetopath", false));
        //stop 사용
        //스피드를 조정하려면 스피드를 불러와야 하는데 불러오는 방법??
    }

    
    private void Update()
    {
        //echo += 10f;
        //delta += echo;
        //time.time = delta;
        time = GetComponent<iTween>();

        if (Input.GetKey(KeyCode.UpArrow))
        {
            time.time -= 0.01f;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            time.time += 0.01f;
        }

        #region ConerLine
        /*********** 코너라인 ****************************************************************/
        if (conercheck == true)
        {
            lineState = 1;
            if(speed < 0)
            {
               // move.transform.localPosition.x = 1.0f;
            }
        }
        #endregion


        #region SpecialLine
        /*********** 특수주행 ****************************************************************/
        else if (speciallinecheck == true)
        {
            lineState = 2;
        }
        #endregion


        #region StraightLine
        /*********** 일자라인 ****************************************************************/
        else if (conercheck == false && speciallinecheck == false)
        {
            lineState = 0;
            RightLeftMove();  //좌우이동
        }
        #endregion
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "ConerEnter")
        {
            conercheck = true;
        }
        if(other.gameObject.tag == "ConerExit")
        {
            conercheck = false;
        }
        if(other.gameObject.tag == "SpecialEnter")
        {
            speciallinecheck = true;
        }
        if(other.gameObject.tag == "SpecialExit")
        {
            speciallinecheck = false;
        }
    }

    //현재속도에 따라 라인변경 speed 다르게 수정해야함
    void RightLeftMove()
    {
        #region RightKey
        if (Input.GetKeyDown(KeyCode.RightArrow)) //오른쪽 키 눌렀을때
        {
            pos = move.transform.localPosition;
            MoverotateY = move.transform.rotation.eulerAngles.y;
        }
        if (Input.GetKey(KeyCode.RightArrow)) //오른쪽 키 누르고 있을때 (오른쪽 이동, y축 회전)
        {
            if (move.transform.localPosition.x < 3.0f)
            {
                pos.x += horizonspeed * Time.deltaTime;
                move.transform.localPosition = pos;

                //move.transform.Translate(Vector3.right * speed * Time.deltaTime);

                if (MoverotateY - move.transform.rotation.eulerAngles.y > -20.0f)
                    move.transform.Rotate(0, 5f, 0);
            }
        }
        if (Input.GetKeyUp(KeyCode.RightArrow)) //키 땠을때 : 다시 앞에 봄
        {
            move.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime);
            //Quaternion.Euler(0, 0, 0) 을 넣으면 회전하고 바라보는 방향이 바껴서 slerp를 사용함 - 유니티 홈페이지에서 참고
        }
        #endregion

        #region LeftKey
        if (Input.GetKeyDown(KeyCode.LeftArrow)) //왼쪽키눌렀을때
        {
            pos = move.transform.localPosition;
            MoverotateY = move.transform.rotation.eulerAngles.y;
        }
        if (Input.GetKey(KeyCode.LeftArrow)) //왼쪽 키 누르고 있을때 (왼쪽 이동, y축 회전)
        {
            if (move.transform.localPosition.x > -3.0f)
            {
                pos.x -= horizonspeed * Time.deltaTime;
                move.transform.localPosition = pos;

                if (MoverotateY - move.transform.rotation.eulerAngles.y < 20.0f)
                    move.transform.Rotate(0, -5f, 0);
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow)) //키 땠을때 : 다시 앞에 봄
        {
            move.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime);
        }
        #endregion
    }


    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Slipstream")
        {
            Debug.Log("슬립스트림");
        }

        if(other.gameObject.tag == "blocking")
        {
            Debug.Log("경로차단");
            //time.time = other.transform.parent.gameObject.GetComponent<OtherMoveScript>().Tween.time;
            //time.time = other.gameObject.transform.parent.gameObject.GetComponent<OtherMoveScript>().Tween.time; //앞의것과 같게
            //// 경로차단 콜라이더의 부모(otherplayer)에 접근하려고 시도
            Debug.Log(other.transform.parent.gameObject.GetComponent<OtherMoveScript>().Tween.time);
        }
    }
}