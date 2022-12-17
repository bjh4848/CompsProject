using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunLogic : MonoBehaviour
{
    public float damage, timeBetweenShot, bulletSpread, range, reloadTime;
    public int magSize, bulletsPerShot;
    public bool allowButtonHold;

    public AIHealth AIHealthScript;

    int bulletsShot;
    int bulletsRemaining;
    bool shotReady;
    bool shooting;
    bool reloading;

    public AudioSource gunSound;
    public AudioSource reloadSound;

    public AdvancedCamRecoil camRecoil;

    public Camera playerCam;
    public Transform gunMuzzle;
    public RaycastHit shotCast;
    public LayerMask isEnemy;
    public GameObject muzzleFlash, bulletHoleGraphic;

    public Transform recoilPosition;
    public Transform rotationPoint;

    public float positionalRecoilSpeed = 8f;
    public float rotationalRecoilSpeed = 8f;
    public float positionalReturnSpeed = 18f;
    public float rotationalReturnSpeed = 38f;

    public Vector3 RecoilRotation = new Vector3(10, 5, 7);
    public Vector3 RecoilKickBack = new Vector3(0.015f, 0f, -0.2f);

    private Vector3 rotationalRecoil;
    private Vector3 positionalRecoil;
    private Vector3 rotation;

    public bool aiming;
    public float rotationSpeed = 6;
    public float returnSpeed = 25;

    private void FixedUpdate()

    {

        rotationalRecoil = Vector3.Lerp(rotationalRecoil, Vector3.zero, rotationalReturnSpeed * Time.deltaTime);
        positionalRecoil = Vector3.Lerp(positionalRecoil, Vector3.zero, positionalReturnSpeed * Time.deltaTime);



        recoilPosition.localPosition = Vector3.Slerp(recoilPosition.localPosition, positionalRecoil, positionalRecoilSpeed * Time.deltaTime);
        rotation = Vector3.Slerp(rotation, rotationalRecoil, rotationalRecoilSpeed * Time.deltaTime);
        rotationPoint.localRotation = Quaternion.Euler(rotation);
    }
    private void Start()
    {
        gunSound = GetComponent<AudioSource>();
        camRecoil = GameObject.FindObjectOfType(typeof(AdvancedCamRecoil)) as AdvancedCamRecoil;

        bulletsRemaining = magSize;
        shotReady = true;
    }

    void Update()
    {
        MyInput();
    }

    private void MyInput()
    {
        if (allowButtonHold) 
        {
            shooting = Input.GetKey(KeyCode.Mouse0);
        }
        else
        {
            shooting = Input.GetKey(KeyCode.Mouse0);
        }

        if(Input.GetKeyDown(KeyCode.R) && bulletsRemaining < magSize && !reloading)
        {
            Reload();
        }

        if(shotReady && shooting && !reloading && bulletsRemaining > 0)
        {
            bulletsShot = bulletsPerShot;
            Shoot();
        }
    }

    private void Shoot()
    {
        shotReady = false;
        
        camRecoil.Fire();

        rotationalRecoil += new Vector3(-RecoilRotation.x, Random.Range(-RecoilRotation.y, RecoilRotation.y), Random.Range(-RecoilRotation.z, RecoilRotation.z));
        rotationalRecoil += new Vector3(Random.Range(-RecoilKickBack.x, RecoilKickBack.x), Random.Range(-RecoilKickBack.y, RecoilKickBack.y), RecoilKickBack.z);



        gunSound.Play();

        float xSpread = Random.Range(-bulletSpread, bulletSpread);
        float ySpread = Random.Range(-bulletSpread, bulletSpread);

        Vector3 direction = playerCam.transform.forward + new Vector3(xSpread, ySpread, 0);

        Debug.Log("Firing");
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out shotCast, range, isEnemy))
        {
            Debug.Log(shotCast.collider.name);
            if (shotCast.collider.CompareTag("Enemy"))
            {
                shotCast.collider.GetComponent<AIHealth>().TakeDamage(damage);
            }
        }

        Instantiate(muzzleFlash, gunMuzzle.position, Quaternion.identity);
        bulletsShot--;
        bulletsRemaining--;
        Invoke("ResetShot", timeBetweenShot);
        if(bulletsShot > 0 && bulletsRemaining > 0)
        {
            Invoke("Shoot", timeBetweenShot);
        }
    }

    private void ResetShot()
    {
        shotReady = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsRemaining = magSize;
        reloadSound.Play();
        reloading = false;
    }

}
