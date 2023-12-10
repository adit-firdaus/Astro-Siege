using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using MyBox;

public class PlayerController : MonoBehaviour
{
    public bool lockCursor = true;
    public float moveSpeed = 5.0f;
    public float sensitivity = 2.0f;
    public Transform head;
    public Weapon weapon;
    public GameObject MeleePrefabs;
    public float MeleeCooldown;
    private bool isMelee;
    private float rotationX = 0;
    public AudioSource AttackSFX;
    public AudioClip PunchSound;
    public Animator anim;
    public bool busy = false;
    private float slideTime;
    private Vector3 baseXY;
    public GameObject Slide;


    private void Start()
    {
        weapon.onReload.AddListener(OnReload);

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        baseXY = transform.position;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal, 0, vertical) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = 7.5f;
            anim.SetTrigger("sprint");
            busy = true;
        }
        else
        {
            moveSpeed = 5f;

            busy = false;
        }
        if (horizontal != 0 || vertical != 0)
        {
            anim.SetBool("isWalk", true);
        }
        else
        {
            anim.SetBool("isWalk", false);
        }
        /*
        if (Input.GetKey(KeyCode.LeftControl) && !busy && slideTime > 0)
        {
            anim.SetTrigger("melee");
            slideTime -= Time.deltaTime;
            busy = true;
            Slide.SetActive(true);
            moveSpeed = 15f;
            transform.position = new Vector3(transform.position.x, baseXY.y - 0.5f, transform.position.z);
        }
        else
        {
            moveSpeed = 5f;
            slideTime = 1f;
            Slide.SetActive(false);
            transform.position = new Vector3(transform.position.x, baseXY.y, transform.position.z);
        }
        */
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90, 120);

        head.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, mouseX, 0);

        if (weapon)
        {
            if (!busy)
            {
                weapon.SetFiring(Input.GetMouseButton(0));
            }


            if (Input.GetKeyDown(KeyCode.R) && !busy)
            {
                weapon.Reload();
                busy = true;

            }
            if (Input.GetMouseButton(1) && isMelee == false)
            {
                StartCoroutine("MeleeAttack");
                isMelee = true;
            }
        }
        else
        {
            ///KLO GK PUNYA WEAPON
        }
    }

    IEnumerator MeleeAttack()
    {
        anim.SetTrigger("melee");
        yield return new WaitForSeconds(0.7f);
        AttackSFX.PlayOneShot(PunchSound);
        MeleePrefabs.SetActive(true);
        yield return new WaitForSeconds(MeleeCooldown);
        MeleePrefabs.SetActive(false);
        isMelee = false;
    }

    public void OnReload(bool state)
    {
        PlayerController PC = FindObjectOfType<PlayerController>();
        PC.anim.SetBool("Reload", state);
        PC.busy = state;
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
    public UnityEvent<bool> onReload;
    public float reloadSpeed = 2f;

    [Layer] public int bulletLayer;
    public Bullet bulletPrefab;
    public AudioSource As;
    public AudioClip RifleSound;
    public AudioClip ReloadSound;

    public int maxAmmo = 30; // Maximum ammo capacity
    public int currentAmmo; // Current ammo count
    private bool isReloading;
    private void Start()
    {


        currentAmmo = maxAmmo; // Initialize ammo count to maximum at the start

        // Start the shooting coroutine when the weapon is enabled
        StartCoroutine(ShootCoroutine());
    }

    private void Update()
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
            if (currentAmmo <= 0 && isReloading == false)
            {
                Reload();
            }

            yield return null;
        }
    }

    public void Fire()
    {
        Bullet bullet = Instantiate(bulletPrefab.gameObject, firePoint.position, firePoint.rotation).GetComponent<Bullet>();
        bullet.Shoot(power);
        bullet.gameObject.SetLayerRecursively(bulletLayer);
        recoilValue = -1f;
        onShoot.Invoke(bullet);
        As.PlayOneShot(RifleSound);
    }

    [ContextMenu("Reload")]
    public void Reload()
    {
        Debug.Log("Reloading...");
        isReloading = true;
        onReload.Invoke(true);
        As.PlayOneShot(ReloadSound);
        Invoke("FullAmmo", reloadSpeed);

    }
    public void FullAmmo()
    {
        Debug.Log("Reloaded!");
        isReloading = false;
        currentAmmo = maxAmmo;
        onReload.Invoke(false);
    }
}

public class Bullet : MonoBehaviour
{
    public float damage = 10f;
    public float lifetime = 5f;
    public GameObject impactEffect;
    public GameObject bloodEffect;

    float time;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Shoot(float power)
    {
        rb.velocity = transform.forward * power;
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

        if (healthModule != null)
        {
            healthModule.TakeDamage(damage);
            healthModule.playImpactSound();
        };
        if (other.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            HitEnemy();
        }
        else
        {
            HitDestroy();
        }
        
    }

    public void HitDestroy()
    {
        if (impactEffect) Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
    public void HitEnemy()
    {
        Instantiate(bloodEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}