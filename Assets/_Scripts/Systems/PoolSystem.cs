using Cysharp.Threading.Tasks;
using UnityEngine;

public class PoolSystem<T> where T : Object
{
    protected T[] objects;

    public PoolSystem()
    {
        objects = null;
    }

    public async UniTask Create(T prefab, int count, Transform parent = null)
    {
        objects = await Object.InstantiateAsync(prefab, count, new InstantiateParameters { parent = parent, worldSpace = false });
    }

    public virtual T GetObject()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if (!(objects[i] as GameObject).activeSelf)
            {
                return objects[i];
            }
        }

        return null;
    }
}

public class PoolSystem : PoolSystem<GameObject>
{
    public override GameObject GetObject()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if (!objects[i].activeSelf)
            {
                return objects[i];
            }
        }

        return null;
    }
}