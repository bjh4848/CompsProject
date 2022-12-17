using System.Collections;

using System.Collections.Generic;

using UnityEngine;
public class AdvancedWeaponRecoil : MonoBehaviour

{
    public Transform recoilPosition;
    public Transform rotationPoint;
    public float positionalRecoilSpeed = 8f;
    public float rotationalRecoilSpeed = 8f;
    public float positionalReturnSpeed = 18f;
    public float rotationalReturnSpeed = 38f;

    public Vector3 RecoilRotation = new Vector3(10, 5, 7);
    public Vector3 RecoilKickBack = new Vector3(0.015f, 0f, -0.2f);
    public Vector3 RecoilRotationAim = new Vector3(10, 4, 6);
    public Vector3 RecoilKickBackAim = new Vector3(0.015f, 0f, -0.2f);

    private Vector3 rotationalRecoil;
    private Vector3 positionalRecoil;
    private Vector3 rotate;
    private void FixedUpdate()

    {

        rotationalRecoil = Vector3.Lerp(rotationalRecoil, Vector3.zero, rotationalReturnSpeed * Time.deltaTime);
        positionalRecoil = Vector3.Lerp(positionalRecoil, Vector3.zero, positionalReturnSpeed * Time.deltaTime);



        recoilPosition.localPosition = Vector3.Slerp(recoilPosition.localPosition, positionalRecoil, positionalRecoilSpeed * Time.deltaTime);
        rotate = Vector3.Slerp(rotate, rotationalRecoil, rotationalRecoilSpeed * Time.deltaTime);
        rotationPoint.localRotation = Quaternion.Euler(rotate);

    }



    void Update()

    {

        if (Input.GetKeyDown(KeyCode.Mouse0))

        {

            Fire();

        }
    }



    public void Fire()

    {
            rotationalRecoil += new Vector3(-RecoilRotation.x, Random.Range(-RecoilRotation.y, RecoilRotation.y), Random.Range(-RecoilRotation.z, RecoilRotation.z));
            rotationalRecoil += new Vector3(Random.Range(-RecoilKickBack.x, RecoilKickBack.x), Random.Range(-RecoilKickBack.y, RecoilKickBack.y), RecoilKickBack.z);

    }
}