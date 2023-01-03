using UnityEngine;
using System.Collections;

public static class Noise {

	public enum NormalizeMode {Local, Global};

	public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, NoiseSettings settings, Vector2 sampleCentre) {
		// creates the Noisemap variable to be passed for generating terrain
		float[,] noiseMap = new float[mapWidth,mapHeight];

		// Generates a random seed to each of the NoiseMaps
		System.Random prng = new System.Random (settings.seed);
		Vector2[] octaveOffsets = new Vector2[settings.octaves];

		// intializing varaiables to be used for creating terrain height
		float maxPossibleHeight = 0;
		float amplitude = 1;
		float frequency = 1;

		// using the octaves to determine the frequency at which the height of each offset will be
		for (int i = 0; i < settings.octaves; i++) {
			float offsetX = prng.Next (-100000, 100000) + settings.offset.x + sampleCentre.x;
			float offsetY = prng.Next (-100000, 100000) - settings.offset.y - sampleCentre.y;
			octaveOffsets [i] = new Vector2 (offsetX, offsetY);

			maxPossibleHeight += amplitude;
			amplitude *= settings.persistance;
		}

		// setting the constraints on the noise creation to prevent
		float maxLocalNoiseHeight = float.MinValue;
		float minLocalNoiseHeight = float.MaxValue;

		// centers the creation of the noise map
		float halfWidth = mapWidth / 2f;
		float halfHeight = mapHeight / 2f;


		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {

				amplitude = 1;
				frequency = 1;
				float noiseHeight = 0;

				for (int i = 0; i < settings.octaves; i++) {
					float sampleX = (x-halfWidth + octaveOffsets[i].x) / settings.scale * frequency;
					float sampleY = (y-halfHeight + octaveOffsets[i].y) / settings.scale * frequency;

					float perlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;
					noiseHeight += perlinValue * amplitude;

					amplitude *= settings.persistance;
					frequency *= settings.lacunarity;
				}

				if (noiseHeight > maxLocalNoiseHeight) {
					maxLocalNoiseHeight = noiseHeight;
				} 
				if (noiseHeight < minLocalNoiseHeight) {
					minLocalNoiseHeight = noiseHeight;
				}
				noiseMap [x, y] = noiseHeight;

				if (settings.normalizeMode == NormalizeMode.Global) {
					float normalizedHeight = (noiseMap [x, y] + 1) / (maxPossibleHeight / 0.9f);
					noiseMap [x, y] = Mathf.Clamp (normalizedHeight, 0, int.MaxValue);
				}
			}
		}

		// If the normalize is local, the program will generate the noise map based on the information of the individual planes, rather than accounting for the all planes that have been generated
		if (settings.normalizeMode == NormalizeMode.Local) {
			for (int y = 0; y < mapHeight; y++) {
				for (int x = 0; x < mapWidth; x++) {
					noiseMap [x, y] = Mathf.InverseLerp (minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap [x, y]);
				}
			}
	}

		return noiseMap;
	}

}

// creating a class to store all the information for each noise values for easier access and trading of information
[System.Serializable]
public class NoiseSettings {
	public Noise.NormalizeMode normalizeMode;

	public float scale = 50;

	public int octaves = 6;
	[Range(0,1)]
	public float persistance =.6f;
	public float lacunarity = 2;

	public int seed;
	public Vector2 offset;

	public void ValidateValues() {
		scale = Mathf.Max(scale, 0.01f);
		octaves = Mathf.Max(octaves, 1);
		lacunarity = Mathf.Max(lacunarity, 1);
		persistance = Mathf.Clamp01(persistance);
	}
}