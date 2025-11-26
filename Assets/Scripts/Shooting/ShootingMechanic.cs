using UnityEngine;
using System.Collections;

public class ShootingMechanic : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform BulletSpawnPoint;
    [SerializeField] private TrailRenderer BulletTrail;
    [SerializeField] private float shootCooldown = 0.55f;
    [SerializeField] private Animator animator;
    float shakeAmount = 0.35f;
    float decreaseFactor = 1.0f;
    float shake = 0f;
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
        }

        if (shake > 0)
        {
            mainCamera.transform.localPosition = Random.insideUnitSphere * shakeAmount;
            shake -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            mainCamera.transform.localPosition = Vector3.zero;
            shake = 0f;
        }
    }

    void Shoot()
    {

        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                shake = 0.2f;
            }

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
