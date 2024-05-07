using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

public class NoiseWeightManager : MonoBehaviour
{
    public static NoiseWeightManager Instance {get; private set;}

    public Dictionary<int, INoiseWeight> noise_objects = new Dictionary<int, INoiseWeight>();

    private void Awake() {
        Instance = this;
    }

    public void AddNoise(GameObject obj, INoiseWeight noiseWeight) {
        int id = obj.GetInstanceID();
        if(!noise_objects.ContainsKey(id)) {
            noise_objects.Add(id, noiseWeight);
        }
    }

    public void SetWeightData(GameObject obj, Vector3 sound_Pos, float sound_size) {
        int id = obj.GetInstanceID();

        float distance = Vector3.Distance(obj.transform.position, sound_Pos);

        if(distance > 100f) {
            return;
        } else {
            float sound_weight = sound_size * (1 - (distance / 100f));

            noise_objects[id].NoiseWeight = sound_weight;
            noise_objects[id].NoisePosition = sound_Pos;
        }
    }

    public void SetWeightDataAll(Vector3 sound_Pos, float sound_size) {

        foreach (var obj in noise_objects.Values) {
            float distance = Vector3.Distance(obj.MyTransform.position, sound_Pos);

            if(distance > 100f) {
                continue;
            } else {
                float sound_weight = sound_size * (1 - (distance / 100f));

                obj.NoiseWeight = sound_weight;
                obj.NoisePosition = sound_Pos;
            }
        }
    }

    public void ResetNoiseProperties(int id) {
        noise_objects[id].NoisePosition = Vector3.zero;
        noise_objects[id].NoiseWeight = 0f;
    }

}
