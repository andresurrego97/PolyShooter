using Cysharp.Threading.Tasks;
using UnityEngine;

public class HitVfxParticle : MonoBehaviour
{
    [SerializeField] private Transform decal;

    private float time = 1;

    private void OnEnable()
    {
        decal.localScale = Vector3.one;
        decal.localEulerAngles = new Vector3(90, Random.Range(0, 360), 0);
    }

    private void OnParticleSystemStopped()
    {
        time = 1;

        ScaleDestroy().Forget();
    }

    private async UniTaskVoid ScaleDestroy()
    {
        while (time > 0)
        {
            time -= Time.deltaTime * 2;

            decal.localScale = Vector3.one * time;

            await UniTask.Yield();
        }

        gameObject.SetActive(false);
    }
}