using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor.ShaderGraph;
using Unity.Mathematics;

public class ShootingMechanic : MonoBehaviour
{
    

    [SerializeField] private Camera mainCamera;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private Transform BulletSpawnPoint;
    [SerializeField] private TrailRenderer BulletTrail;
    [SerializeField] private ParticleSystem hitParticle;
    [SerializeField] private float shootCooldown = 0.55f;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask _ignore;
    [Range(0f, 1f)]
    [SerializeField] private float volume = 1;

    private bool _shoot = false;

    float shakeAmount = 0.35f;
    float decreaseFactor = 1.0f;
    float shake = 0f;
    private float lastShootTime = 0f;

    private GameObject TheHitParticle;
    [SerializeField] private TextPopup feed;

    void Start()
    {
        
    }

    void Update()
    {
        if (_shoot == true && Time.time >= lastShootTime)
        {
            lastShootTime = Time.time + shootCooldown;
            Shoot();
            animator.SetBool("isShooting", true);
            animator.SetTrigger("shouldFlash");
            AudioSource.PlayClipAtPoint(shootSound, transform.position, volume);
        }
        if (shake > 0)
        {
            mainCamera.transform.localPosition = new Vector3(0, 1.5f, 0) + UnityEngine.Random.insideUnitSphere * shakeAmount;
            shake -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            mainCamera.transform.localPosition = new Vector3(0, 1.5f, 0) + Vector3.zero;
            shake = 0f;
        }
    }

        public void Shoot()
        {
            animator.SetBool("isShooting", true);
            animator.SetTrigger("shouldFlash");
            RaycastHit hit;
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, 100f))
            {
                //Debug.Log("Hit: " + hit.transform.name);
                
                
                TrailRenderer trail = Instantiate(BulletTrail, BulletSpawnPoint.position, Quaternion.identity);

                
                

            StartCoroutine(SpawnTrail(trail, hit));
                if (hit.transform.CompareTag("Enemy"))
                {
                    shake = 0.2f;
                    //UIStyleMeter.instance.AddStyle(20f);
                    //UIStyleMeter.instance.WhatHit("Enemy");
                    feed.AddEntry("+ KILL");
                    EnemyBodyPart hittingpoint = hit.transform.GetComponent<EnemyBodyPart>();

                    StartCoroutine(hittingpoint.GotHit());
                    
                }
            }
        }
    

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        Vector3 startPosition = trail.transform.position;
        Vector3 endPosition = hit.point;
        Vector3 hitAngle = hit.normal;
        
        ParticleSystem hitEffect = Instantiate(hitParticle, endPosition, Quaternion.LookRotation(hitAngle, Vector3.up));
        
        
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

        yield return new WaitForSeconds(10f);

        Destroy(hitEffect.gameObject);
        
        
    }

    public void Sootywooty(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _shoot = true; 
        }
        else 
        {
            _shoot = false; 
        }
    }
}
