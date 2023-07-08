using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class Job2 : MonoBehaviour
{
    public NativeArray<Vector3> _position;
    public NativeArray<Vector3> _velocity;
    public NativeArray<Vector3> _finalPosition;

    void Start()
    {
        _position = new NativeArray<Vector3>(new Vector3[] { Vector3.back, Vector3.down, Vector3.left }, Allocator.Persistent);
        _velocity = new NativeArray<Vector3>(new Vector3[] { Vector3.forward, Vector3.up, Vector3.right }, Allocator.Persistent);
        _finalPosition = new NativeArray<Vector3>(3, Allocator.Persistent);


        MyJob myJob = new MyJob()
        {
            position = _position,
            velocity = _velocity,
            finalPosition = _finalPosition
        };

        JobHandle jobHandle = myJob.Schedule(_finalPosition.Length, 0);
        jobHandle.Complete();

        for (int i = 0; i < _finalPosition.Length; i++)
            Debug.Log(_finalPosition[i]);


    }

    public struct MyJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<Vector3> position;
        [ReadOnly] public NativeArray<Vector3> velocity;
        [WriteOnly] public NativeArray<Vector3> finalPosition;

        public void Execute(int index)
        {
            finalPosition[index] = position[index] + velocity[index];
        }
    }

    private void OnDestroy()
    {
        if (_position.IsCreated)
        {
            _position.Dispose();
            _velocity.Dispose();
            _finalPosition.Dispose();
        }
    }
}
