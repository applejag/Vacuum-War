using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class CannonBallExplosion : MonoBehaviour {

	public ParticleSystem particles;
	public Light pointLight;

	public AnimationCurve lightMultiplier = new AnimationCurve(new Keyframe(0, 1), new Keyframe(0.8f,0.8f), new Keyframe(1,0));
	private float lightStartIntensity;
	private float aliveTime;

	void Start () {
		lightStartIntensity = pointLight.intensity;
		Destroy(gameObject, particles.main.startLifetime.constant);
	}

	void Update() {
		aliveTime += Time.deltaTime;
		pointLight.intensity = lightStartIntensity * lightMultiplier.Evaluate(aliveTime / particles.main.startLifetime.constant);
	}
}
