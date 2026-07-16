using System.Collections.Generic;
using UnityEngine;

public class DeathParticlePool : MonoBehaviour
{
    public static DeathParticlePool Instance;

    [SerializeField] private GameObject _particlePrefab;
    [SerializeField] private int _poolSize = 20;

    private readonly Queue<ParticleSystem> _pool = new();

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < _poolSize; i++)
        {
            GameObject obj = Instantiate(_particlePrefab, transform);

            ParticleSystem ps = obj.GetComponent<ParticleSystem>();

            obj.SetActive(false);

            _pool.Enqueue(ps);
        }
    }

    public void Play(Vector3 position)
    {
        if (_pool.Count == 0)
            return;

        ParticleSystem ps = _pool.Dequeue();

        ps.transform.position = position;

        ps.gameObject.SetActive(true);

        ps.Play();

        StartCoroutine(ReturnToPool(ps));
    }

    private System.Collections.IEnumerator ReturnToPool(ParticleSystem ps)
    {
        yield return new WaitForSeconds(ps.main.duration);

        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        ps.gameObject.SetActive(false);

        _pool.Enqueue(ps);
    }
}