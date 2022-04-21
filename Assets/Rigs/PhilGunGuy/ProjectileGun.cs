using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileGun : MonoBehaviour
{
    //Other animations
    Animator animator;

    //bullet
    public GameObject bullet;

    // bullet force
    public float shootForce, upwardForce;

    //Gun stats
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int bulletsPerTap, magazineSize;

    public static int bulletsShot, bulletsLeft;

    //bools
    public static bool shooting;
    bool readyToShoot, reloadNow;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;

    //bugs
    public bool allowInvoke = true;

    private void Awake()
    {
        //make sure mag is full
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();
    }
    private void MyInput()
    {
        // Allowed to hold fire button?
        shooting = Input.GetKey(KeyCode.Mouse0);

        //Reloading
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloadNow)
        {
            Reload();
        }
        if (readyToShoot && shooting && !reloadNow && bulletsLeft <= 0) Reload();

        //Shooting
        if (readyToShoot && shooting && !reloadNow && bulletsLeft > 0)
        {
            //Set bullets to 0
            bulletsShot = 0;

            Shoot();
        }
    }
    private void Shoot()
    {
        readyToShoot = false;

        //Find exact position using raycast
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        //check if ray hits something
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75); // a far away point from the player

        //calculate direction without spread
        Vector3 directionWithoutSpread = targetPoint = attackPoint.position;

        //Calculate spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate new direction with spread added
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        //Instantiate bullet/projectile
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        //Rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithSpread.normalized;

        //Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);

        bulletsLeft--;
        bulletsShot++;

        //Invoke resetShot function
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }    
    }
    private void ResetShot()
    {
        //Allow Shooting and invoking again
        readyToShoot = true;
        allowInvoke = true;
    }
    private void Reload()
    {
        reloadNow = true;
        Invoke("ReloadFinished", reloadTime); // I can't get this reload to work
        //animator.SetBool("reloading", true);
        //animator.SetBool("leftclick", false);
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloadNow = false;
        //animator.SetBool("reloading", false);
    }
}
