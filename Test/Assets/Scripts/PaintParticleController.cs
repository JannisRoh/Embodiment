using UnityEngine;

public class PaintParticleController : MonoBehaviour
{
    public ParticleSystem paintParticles;

    public void EmitPaint()
    {
        paintParticles.Play();
    }
}
