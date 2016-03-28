using UnityEngine;
using System.Collections;

public class CameraShaker : MonoBehaviour {
    public static CameraShaker S;

    Quaternion originrot;
    void Awake()
    {
        S = this;
    }

    // Use this for initialization
    void Start()
    {
        Shaking = false;
        originrot = transform.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.rotation = originrot;
        if (ShakeIntensity > 0)
        {
            transform.position = OriginalPos + Random.insideUnitSphere * ShakeIntensity;
            transform.rotation = new Quaternion(OriginalRot.x + Random.Range(-ShakeIntensity, ShakeIntensity) * .2f,
                                      OriginalRot.y + Random.Range(-ShakeIntensity, ShakeIntensity) * .2f,
                                      OriginalRot.z + Random.Range(-ShakeIntensity, ShakeIntensity) * .2f,
                                      OriginalRot.w + Random.Range(-ShakeIntensity, ShakeIntensity) * .2f);

            ShakeIntensity -= ShakeDecay;
        }
        else if (Shaking)
        {
            Shaking = false;
        }
    }

    public bool Shaking;
    private float ShakeDecay;
    private float ShakeIntensity;
    private Vector3 OriginalPos;
    private Quaternion OriginalRot;

    public void DoShake(float Intensity, float Decay)
    {
        OriginalPos = transform.position;
        OriginalRot = transform.rotation;

        ShakeIntensity = Intensity;
        ShakeDecay = 0.005f;
        Shaking = true;
    }
}
