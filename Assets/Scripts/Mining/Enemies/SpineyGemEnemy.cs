using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineyGemEnemy : BasicMiningEnemy
{
    bool isRotating;
    public float rotationSpeed;
    [SerializeField] float startRotation;
    public float DirectionA;
    public float DirectionB;
    bool isGrounded=true;
    float attackCooldown;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        DirectionA = transform.localEulerAngles.y;
        if(DirectionA==90)
        {
            DirectionB = 270;
        }
        if (DirectionA == 0)
        {
            DirectionB = 180;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!isGrounded)
            DetectObstacle();
        Rotate();
        Move();
        if (attackCooldown > 0)
            attackCooldown -= Time.deltaTime;
    }
    private void Move()
    {
        if(!isRotating)
        transform.position += transform.forward * moveSpeed*Time.deltaTime;
    }
    private void Rotate()
    {
        if(isRotating)
        {
            transform.localEulerAngles += new Vector3(0, 1, 0) * Time.deltaTime * rotationSpeed;
            
            if(startRotation==DirectionB)
            {
                if (transform.localEulerAngles.y<DirectionA+2&& transform.localEulerAngles.y>DirectionA-2)
                {
                    transform.localEulerAngles = new Vector3(0,DirectionA,0);
                    isRotating = false;
                    isGrounded = true;
                }
            }
            if (startRotation == DirectionA)
            {
                if (transform.localEulerAngles.y > DirectionB-2 && transform.localEulerAngles.y < DirectionB+2)
                {
                    transform.localEulerAngles = new Vector3(0, DirectionB, 0);
                    isRotating = false;
                    isGrounded = true;
                }
            }

        }
    }
    public override void DetectObstacle()
    {
        if (isRotating)
            return;
        startRotation = transform.localEulerAngles.y;
        if (startRotation > DirectionB-10 && startRotation < DirectionB+10)
            startRotation = DirectionB;
        else
        {
            startRotation = DirectionA;
        }
        isRotating = true;
    }
    public override void DetectNoGround()
    {
        if(!isRotating)
        isGrounded = false;
    }
    public override void DetectGround()
    {
        if (!isRotating)
            isGrounded = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            if (attackCooldown > 0)
                return;
            attackCooldown = 0.5f;
            other.gameObject.GetComponent<MiningPlayer>().TakeDamage(damage);
            if (attackAudio)
                MMSoundManager.Instance.PlaySound(attackAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
            false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.95f, 1.05f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
            1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
        }
    }
}
