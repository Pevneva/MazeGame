using System.Collections.Generic;
using UnityEngine;

public class PlayerCreating : MonoBehaviour
{
    [SerializeField] private GameObject _cubeTemplate;
    [SerializeField] private Transform _playerParent;

    private float _size;
    private float _scale;
    private List<GameObject> _cubes = new List<GameObject>();
    private float _radius = 5.0F;
    private float _power = 10.0F;

    private void Start()
    {
        _scale = _cubeTemplate.transform.localScale.x;

        CreatePlayer();
        _playerParent.position += Vector3.up * 5; 
        
        Invoke(nameof(RemoveKinematic), 3);
        Invoke(nameof(DoExplosionEffect), 3.1f);
    }

    private void CreatePlayer()
    {
        // for (float i = 0; i < _scale * 10; i += _scale)
        // for (float j = 0; j < _scale * 10; j += _scale)
        // for (float k = 0; k < _scale * 10; k += _scale)
        for (float i = -_scale * 5; i < _scale * 5; i += _scale)
        for (float j = -_scale * 5; j < _scale * 5; j += _scale)
        for (float k = -_scale * 5; k < _scale * 5; k += _scale)
        {
            var cube = Instantiate(_cubeTemplate, new Vector3(i + _scale/2, j + _scale/2, k + _scale/2), Quaternion.identity, _playerParent);
            _cubes.Add(cube);
        }
    }

    private void RemoveKinematic()
    {
        foreach (var cube in _cubes)
        {
            var rigidBogy = cube.GetComponent<Rigidbody>();
            rigidBogy.isKinematic = false;
        }
    }


    private void DoExplosionEffect()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, _radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(_power, explosionPos, _radius, 3.0F);
        }
    }
}