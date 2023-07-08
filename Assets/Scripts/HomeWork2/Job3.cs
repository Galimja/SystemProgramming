using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class Job3 : MonoBehaviour
{
    [SerializeField] private Transform[] _transforms;
    [SerializeField] private float _rotationSpeed;

    private TransformAccessArray _access;

    void Start()
    {
        _access = new TransformAccessArray(_transforms);
    }

    private void Update()
    {
        RotationJob rotationJob = new RotationJob()
        {
            deltaTime = Time.deltaTime,
            rotationSpeed = _rotationSpeed
        };

        JobHandle jobHandle = rotationJob.Schedule(_access);
        jobHandle.Complete();
    }


    public struct RotationJob : IJobParallelForTransform
    {
        [ReadOnly] public float deltaTime;
        [ReadOnly] public float rotationSpeed;

        public void Execute(int index, TransformAccess transform)
        {
            Vector3 currentRotation = transform.rotation.eulerAngles;
            float newRotationAngle = currentRotation.y + rotationSpeed * deltaTime;
            transform.rotation = 
                Quaternion.Euler(new Vector3(currentRotation.x, newRotationAngle, currentRotation.z));
        }
    }

    private void OnDestroy()
    {
        if (_access.isCreated)
            _access.Dispose();
    }
}
