using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun;

    private float currentFireRate;

    private bool isReload = false;

    private AudioSource audioSource;

    public static bool isActivate = true;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        if (isActivate)
        {
            GunFireRateCalc();
            TryFire();
            TryReload();
            Walk();
            Run();
        }     
    }

    private void GunFireRateCalc()
    {
        if(currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime;
        }
    }

    private void TryFire()
    {
        if(Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        {
            Fire();
        }
    }

    //임시방편
    private void Fire()
    {
        //장전중이 아니어야하며 이동중이 아니어야함
        if (!isReload && (Input.GetAxisRaw("Horizontal")==0 && Input.GetAxisRaw("Vertical") == 0) )
        {
            Shoot();
        }
        else
        {

        }
    }

    private void Shoot()
    {
        currentGun.currentBulletCount--; // 탄환 차감
        currentFireRate = currentGun.fireRate;
        PlaySE(currentGun.Fire_Sound);
        currentGun.muzzleFlash.Play();
        Debug.Log("총알발사");
    }


    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

    // 내가 임의로 코딩함.

    //장전 구현중, 임시방편
    IEnumerator ReloadCoroutine()
    {
        isReload = true;
        currentGun.anim.SetTrigger("Reload");
        yield return new WaitForSeconds(currentGun.reloadTime);
        isReload = false;
    }

    //임시방편
    private void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && isReload == false)
        {
            Debug.Log("R키를 눌렀습니다.");
            StartCoroutine(ReloadCoroutine());
        }
        
    }

    private void Walk()
    {
        // wasd, 방향키를 눌렀다면
        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            currentGun.anim.SetBool("Walk", true);
        }
        else
        {
            currentGun.anim.SetBool("Walk", false);
        }
    }

    private void Run()
    {
        // 이동중이며 좌 쉬프트키가 눌려있다면
        if (Input.GetKey(KeyCode.LeftShift) && (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0 ) )
        {
            currentGun.anim.SetBool("Run", true);
        }
        else
        {
            currentGun.anim.SetBool("Run", false);
        }
    }

    // 상태가 총인지 맨손인지
    //private void StateChange()
    //{
    //    if (Input.GetKeyDown(KeyCode.Alpha1))
    //    {
    //        Debug.Log("맨손으로 변경");
    //    }
        
    //}


}
