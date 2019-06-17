using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public enum EffectType
    {
        Common,
        Flesh
    }

    private static EffectManager m_Instance;

    public ParticleSystem commonHitEffectPrefab;
    public ParticleSystem fleshHitEffectPrefab;

    public static EffectManager Instance
    {
        get
        {
            if (m_Instance == null) m_Instance = FindObjectOfType<EffectManager>();
            return m_Instance;
        }
    }

    public void PlayHitEffect(Vector3 pos, Vector3 normal, Transform parent = null,
        EffectType effectType = EffectType.Common)
    {
        var targetPrefab = commonHitEffectPrefab;

        if (effectType == EffectType.Flesh) targetPrefab = fleshHitEffectPrefab;

        var effect = Instantiate(targetPrefab, pos, Quaternion.LookRotation(normal));

        if (parent != null) effect.transform.SetParent(parent);
        effect.Play();
    }
}