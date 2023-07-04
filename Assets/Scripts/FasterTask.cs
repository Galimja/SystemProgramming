using UnityEngine;
using System.Threading.Tasks;
using System.Threading;

public class FasterTask : MonoBehaviour
{
    [SerializeField] private int _task1Time = 1000;
    [SerializeField] private int _task2Frames = 60;
    async void Start()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        Task<bool> task1 = Task1(cancellationToken, _task1Time);
        Task<bool> task2 = Task2(cancellationToken, _task2Frames);

        bool result = await WhatTaskFasterAsync(cancellationToken, task1, task2);
        Debug.Log(result);

        cancellationTokenSource.Cancel();
        cancellationTokenSource.Dispose();
    }

    public static async Task<bool> WhatTaskFasterAsync(CancellationToken ct, Task<bool> task1, Task<bool> task2)
    {
        using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct))
        {
            CancellationToken cancellationToken = linkedCts.Token;
            Task<bool> finishedTask = await Task.WhenAny(task1, task2);
            linkedCts.Cancel();
            return finishedTask.Result;
        }

    }

    async Task<bool> Task1(CancellationToken ct, int time)
    {
        await Task.Delay(time);
        if (ct.IsCancellationRequested)
        {
            Debug.Log("Task1 остановлена");
            return true;
        }
        Debug.Log("Task1 завершила свою работу");
        return true;
    }

    async Task<bool> Task2(CancellationToken ct, int frames)
    {
        for (int i = 0; i < frames; i++)
        {
            if (ct.IsCancellationRequested)
            {
                Debug.Log("Task2 остановлена");
                return false;
            }
            await Task.Yield();
        }
        Debug.Log("Task2 завершила свою работу");
        return false;
    }


}
