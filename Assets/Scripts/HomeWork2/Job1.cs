using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
public class Job1 : MonoBehaviour
{
    void Start()
    {
        NativeArray<int> array = new NativeArray<int> (10, Allocator.Persistent);
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = i + 10 * (i % 2);
        }
        
        MyJob myJob = new MyJob()
        {
            array = array,
        };

        JobHandle jobHandle = myJob.Schedule();
        jobHandle.Complete();

        for (int i = 0; i < array.Length; i++)
        {
            Debug.Log(array[i]);
        }

        array.Dispose();
    }

    public struct MyJob : IJob
    {
        public NativeArray<int> array;

        public void Execute()
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] > 10) array[i] = 0;
            }
        }
    }
}
