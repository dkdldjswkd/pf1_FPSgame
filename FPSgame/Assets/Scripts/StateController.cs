using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    [SerializeField]
    GameObject Hand;
    [SerializeField]
    GameObject Gun;

    GameObject ChildHand;
    GameObject ChildGun;
    GameObject Script_Gun;
    GameObject Script_Han;
    int State;

    private void Start()
    {
        State = 2;
        ChildHand = transform.Find("Hand Holder").gameObject;
        ChildGun = transform.Find("GunHolder").gameObject;

        //처음 손에 들린게 총이라는 소리
        HandController.isActivate = false;
        GunController.isActivate = true;

    }

    private void Update()
    {
        ChangeState();
    }

    private void ChangeState()
    {
        //맨손상태
        if (State != 1 && Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("상태가 1로 바뀌었습니다.");
            State = 1;

            ChildHand.SetActive(true);
            ChildGun.SetActive(false);
            HandController.isActivate = true;
            GunController.isActivate = false;
        }
        if (State != 2 && Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("상태가 2로 바뀌었습니다.");
            State = 2;

            ChildHand.SetActive(false);
            ChildGun.SetActive(true);
            HandController.isActivate = false;
            GunController.isActivate = true;
            //코드 활성화 비활성화
            //Script_Gun.GetComponent<GunController>().enabled = true;
        }
    }


}
