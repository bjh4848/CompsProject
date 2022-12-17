using UnityEngine;

public class AdvancedCamRecoil : MonoBehaviour

{
    public float rotationSpeed = 6;
    public float returnSpeed = 25;
    public Vector3 RecoilRotation = new Vector3(2f, 2f, 2f);
    public Vector3 RecoilRotationAiming = new Vector3(0.5f, 0.5f, 1.5f);

    private Vector3 currentRotation;

    private Vector3 rotation;



    private void FixedUpdate()

    {
        currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        rotation = Vector3.Slerp(rotation, currentRotation, rotationSpeed * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(rotation);
    }



    public void Fire()

    {
        currentRotation += new Vector3(-RecoilRotation.x, Random.Range(-RecoilRotation.y, RecoilRotation.y), Random.Range(-RecoilRotation.z, RecoilRotation.z));
    }
}
