using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;
public class TaskExample : MonoBehaviour
{
    async void Start()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        Task task1 = Task1(cancellationToken, 1);
        Task task2 = Task2(cancellationToken, 60);

        await Task.WhenAll(task1, task2);

        cancellationTokenSource.Cancel();
        cancellationTokenSource.Dispose();
    }

    async Task Task1(CancellationToken ct, int time)
    {
        await Task.Delay(time);
        Debug.Log("Task1 завершила свою работу");
    }

    async Task Task2(CancellationToken ct, int frames)
    {
        for (int i = 0; i < frames; i++)
        {
            await Task.Yield();
        }
        Debug.Log("Task2 завершила свою работу");
    }


}
