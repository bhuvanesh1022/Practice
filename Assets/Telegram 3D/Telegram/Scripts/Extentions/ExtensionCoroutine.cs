using System;
using UnityEngine;
using System.Collections;

public class ExtensionCoroutine : Singleton<ExtensionCoroutine>
{
    Coroutine _lastRoutine = null;

    public IEnumerator StartExtendedCoroutine(IEnumerator enumerator)
    {
        yield return StartCoroutine(enumerator);
    }

    public IEnumerator StartExtendedCoroutine(IEnumerator enumerator, Action cb)
    {
        yield return StartCoroutine(enumerator);

        cb?.Invoke();
    }

    public void StartExtendedCoroutineNoWait(IEnumerator enumerator)
    {
        _lastRoutine = StartCoroutine(enumerator);
    }

    public void StartExtendedCoroutineNoWait(IEnumerator enumerator, Action cb)
    {
        _lastRoutine = StartCoroutine(enumerator);
    }

    public void StopExtendedCoroutine()
    {
        if (_lastRoutine != null)
            StopCoroutine(_lastRoutine);
    }
}
