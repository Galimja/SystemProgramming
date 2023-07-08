using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private int _health;
    private int _maxHealth = 100;
    private bool _isHealing;


    private void Update()
    {
        if (Input.GetKey(KeyCode.H))
        {
            if (!_isHealing)
                StartCoroutine(ReceiveHealing());
        }
    }

    public IEnumerator ReceiveHealing()
    {
        _isHealing = true;
        for (int i = 0; i < 6; i++)
        {
            _health += 5;
            Debug.Log(_health);
            if (_health >= _maxHealth)
            {
                _health = _maxHealth;
                break;
            }

            yield return new WaitForSeconds(0.5f);
        }
        _isHealing = false;
    }
}
