using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    [SerializeField] float duration = 1f;

    private void OnEnable()
    {
        StartCoroutine(WaitThenDestroyEnum());
    }

    private IEnumerator WaitThenDestroyEnum()
    {
        yield return new WaitForSeconds(duration);

        Destroy(this.gameObject);

    }
}
