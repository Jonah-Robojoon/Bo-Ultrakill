using UnityEngine;
using System.Collections;

public class ShootingMechanic : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform BulletSpawnPoint;
    [SerializeField] private TrailRenderer BulletTrail;
    [SerializeField] private float shootCooldown = 0.55f;
    [SerializeField] private Animator animator;
    private float lastShootTime = 0f;
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= lastShootTime)
        {
            lastShootTime = Time.time + shootCooldown;
            Shoot();
            animator.SetBool("isShooting", true);
            animator.SetTrigger("shouldFlash");

        }
    }

    void Shoot()
    {

        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit))
        {
            Debug.Log("Hit: " + hit.transform.name);
            TrailRenderer trail = Instantiate(BulletTrail, BulletSpawnPoint.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, hit));
        }
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        Vector3 startPosition = trail.transform.position;
        Vector3 endPosition = hit.point;
        float distance = Vector3.Distance(startPosition, endPosition);
        float travelTime = distance / 1000000f; // Adjust speed here
        float elapsedTime = 0f;
        while (elapsedTime < travelTime)
        {
            trail.transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime / travelTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        animator.SetBool("isShooting", false);

        trail.transform.position = endPosition;
        Destroy(trail.gameObject, trail.time);
    }
}
