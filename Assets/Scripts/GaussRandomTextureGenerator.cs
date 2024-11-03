
using UnityEngine;

public class GaussRandomTextureGenerator
{
    static float GenerateGaussianRandomNumber()
    {
        // Using Box-Muller transform to generate Gaussian random numbers
        float u1 = Random.value; // Uniform(0,1) random number
        float u2 = Random.value; // Uniform(0,1) random number

        float z0 = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Cos(2.0f * Mathf.PI * u2);
        return z0;
    }
    public static Texture2D GenerateGaussianTexture(int N)
    {
        Texture2D gauss_random_texture = new Texture2D(N, N, TextureFormat.RGBAFloat, false);

        Color[] pixels = new Color[N * N];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = new Color(
                GenerateGaussianRandomNumber(),
                GenerateGaussianRandomNumber(),
                GenerateGaussianRandomNumber(),
                GenerateGaussianRandomNumber()
            );


        }
        gauss_random_texture.SetPixels(pixels);        

        
        gauss_random_texture.Apply();

        return gauss_random_texture;
    }
        
}
