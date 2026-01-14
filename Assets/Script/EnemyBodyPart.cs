using System.Collections;
using UnityEditor;
using UnityEngine;

public class EnemyBodyPart : MonoBehaviour
{
    private ShootingMechanic _shooting;
    private CapsuleCollider _collider;
    private MeshRenderer _meshRenderer;
    private GameObject _bloodExplosion;

    [SerializeField] private float stylepoints;
    [SerializeField] GameObject bodypart;
    [SerializeField] private GameObject _particleSystem;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _collider = GetComponent<CapsuleCollider>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator GotHit() 
    {
        
        if (_meshRenderer != null)
        {
            _meshRenderer.enabled = false;
        }
        
        _collider.enabled = false;
        _bloodExplosion = Instantiate(_particleSystem, transform.position, Quaternion.identity);
        ParticleSystem bloodparticle = _bloodExplosion.GetComponent<ParticleSystem>();
        bloodparticle.Play();
        
           
            EnemyAi _ai = gameObject.GetComponentInParent<EnemyAi>();
            _ai._isDeing = true;
            yield return new WaitForSeconds(0.02f);
            _ai.enabled = false;
        
        UIStyleMeter.instance.AddStyle(stylepoints);
        Destroy(gameObject);
        yield return new WaitForSeconds(2);
        //Destroy(gameObject);
        
        
    }
}
