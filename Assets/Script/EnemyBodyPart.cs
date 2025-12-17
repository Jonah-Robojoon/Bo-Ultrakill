using System.Collections;
using UnityEditor;
using UnityEngine;

public class EnemyBodyPart : MonoBehaviour
{
    private ShootingMechanic _shooting;
    private BoxCollider _boxCollider;

    [SerializeField] private float stylepoints;
    [SerializeField] private bool canExplode = true;
    [SerializeField] GameObject bodypart;
    [SerializeField] private ParticleSystem _particleSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator GotHit() 
    {
        if (canExplode)
        {
            Destroy(bodypart);
        }
        _boxCollider.enabled = false;
        _particleSystem.Play();
        UIStyleMeter.instance.AddStyle(stylepoints);
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
        
        
    }
}
