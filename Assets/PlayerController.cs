using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using MyBox;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public bool lockCursor = true;
    public float moveSpeed = 5.0f;
    public float jumpSpeed = 1.0f;
    public float sensitivity = 2.0f;
    public Transform head;
    public Weapon weapon;

    CharacterController characterController;
    private float rotationX = 0;

    Vector3 velocity;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void LateUpdate()
    {
        velocity.y += Physics.gravity.y * Time.deltaTime;

        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90, 90);

        head.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, mouseX, 0);

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            move *= 2;
        }

        velocity.x = move.x * moveSpeed;
        velocity.z = move.z * moveSpeed;

        if (characterController.isGrounded)
        {
            velocity.y = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = jumpSpeed;
        }

        if (weapon)
        {
            weapon.SetFiring(Input.GetMouseButton(0));

            if (Input.GetKeyDown(KeyCode.R))
            {
                weapon.Reload();
            }
        }

        characterController.Move(velocity * Time.deltaTime);
    }
}

public class Weapon : MonoBehaviour
{
    public float fireRate = 0.5f;
    public float power = 1;
    public float recoilPower = 1;
    public bool isFiring = false;
    float recoilValue = 0;
    public Transform firePoint;
    public Transform recoilBody;
    public UnityEvent<Bullet> onShoot;

    [Layer] public int bulletLayer;
    public Bullet bulletPrefab;

    public int maxAmmo = 10; // Maximum ammo capacity
    public int currentAmmo; // Current ammo count

    Vector3 previousfirePointPosition;
    Vector3 speedDelta;

    private void Start()
    {
        currentAmmo = maxAmmo; // Initialize ammo count to maximum at the start

        // Start the shooting coroutine when the weapon is enabled
        StartCoroutine(ShootCoroutine());
    }

    private void Update()
    {
        speedDelta = (firePoint.position - previousfirePointPosition) / Time.deltaTime;
        previousfirePointPosition = firePoint.position;
    }

    private void LateUpdate()
    {
        recoilBody.localRotation = Quaternion.Euler(recoilValue, 0, 0);
        recoilBody.localPosition = Vector3.forward * recoilValue * recoilPower;
        recoilValue = Mathf.Lerp(recoilValue, 0, Time.deltaTime * 10);
    }

    public void SetFiring(bool isFiring)
    {
        this.isFiring = isFiring;
    }

    private IEnumerator ShootCoroutine()
    {
        while (true)
        {
            if (isFiring && currentAmmo > 0)
            {
                Fire();
                currentAmmo--;
                yield return new WaitForSeconds(1f / fireRate);

            }

            if (currentAmmo <= 0)
            {
                Reload();
            }

            yield return null;
        }
    }

    public void Fire()
    {
        Bullet bullet = Instantiate(bulletPrefab.gameObject, firePoint.position, firePoint.rotation).GetComponent<Bullet>();
        bullet.Shoot(firePoint.forward * power + speedDelta);
        bullet.gameObject.layer = bulletLayer;
        recoilValue = -1f;

        onShoot.Invoke(bullet);
    }

    [ContextMenu("Reload")]
    public void Reload()
    {
        currentAmmo = maxAmmo; // Refill ammo to maximum
    }
}

public class Bullet : MonoBehaviour
{
    public float damage = 10f;
    public float lifetime = 5f;
    public GameObject impactEffect;

    float time;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Shoot(Vector3 direction)
    {
        rb.velocity = direction;
    }

    private void Update()
    {
        time += Time.deltaTime;

        if (time >= lifetime) HitDestroy();

        transform.rotation = Quaternion.LookRotation(rb.velocity);
    }

    private void OnCollisionEnter(Collision other)
    {
        HealthModule healthModule = other.gameObject.GetComponentInParent<HealthModule>();

        if (healthModule != null) healthModule.TakeDamage(damage);

        HitDestroy();
    }

    public void HitDestroy()
    {
        if (impactEffect) Instantiate(impactEffect, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}