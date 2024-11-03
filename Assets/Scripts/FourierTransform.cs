using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FourierTransform
{
    static ComputeShader FFTShader;
    static ComputeShader permuteShader;

    private static RenderTexture DispatchFFTShader(int size, RenderTexture input_texture, RenderTexture butterfly)
    {
        FFTShader = Resources.Load<ComputeShader>("Shaders/fft");
        FFTShader.SetInt("N", size);
        
        FFTShader.SetInt("direction", 0);
        FFTShader.SetInt("flip", 0);
       

        RenderTexture output_texture = new RenderTexture(size, size, 0, RenderTextureFormat.ARGBFloat);
        output_texture.wrapMode = TextureWrapMode.Clamp;
        output_texture.filterMode = FilterMode.Bilinear;
        output_texture.enableRandomWrite = true;
        output_texture.Create();



        FFTShader.SetTexture(0, "inputTexture", input_texture);
        FFTShader.SetTexture(0, "precomputedButterflyTexture", butterfly);
        FFTShader.SetTexture(0, "outputTexture", output_texture);
        

        int threadGroupsX = Mathf.CeilToInt(size / 16.0f);
        int threadGroupsY = Mathf.CeilToInt(size / 16.0f);

        int flip = 0;

        for (int stage = 0; stage < butterfly.width; stage++)
        {
            // Set pingpong value and index stage.
            FFTShader.SetInt("stage", stage);
            FFTShader.SetInt("flip", flip);
       
            FFTShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);

            // Switch flag on pingpong variable.
            flip = 1 - flip;
        }

        FFTShader.SetInt("direction", 1);

        for (int stage = 0; stage < butterfly.width; stage++)
        {
            // Set pingpong value and index stage.
            FFTShader.SetInt("stage", stage);
            FFTShader.SetInt("flip", flip);
       
            FFTShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);

            // Switch flag on pingpong variable.
            flip = 1 - flip;
        }
        if (flip == 0)
        {
            return output_texture; //THIS IS UNNECESSARY
        }
        return input_texture;
    }

    private static RenderTexture DispatchPermuteShader(int size, RenderTexture permute_texture)
    {
        permuteShader = Resources.Load<ComputeShader>("Shaders/permute");
        permuteShader.SetInt("N", size); //change var names to match
        permuteShader.SetTexture(0, "permute_texture", permute_texture);

        int threadGroupsX = Mathf.CeilToInt(size / 16.0f);
        int threadGroupsY = Mathf.CeilToInt(size / 16.0f);

        permuteShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);

        return permute_texture;
    }

    public static RenderTexture ComputeAndPermuteFFT(int size, RenderTexture input_texture, RenderTexture butterfly)
    {
        RenderTexture heightMap = DispatchFFTShader(size, input_texture, butterfly);
        RenderTexture permutedHeightMap = DispatchPermuteShader(size, heightMap);
        return permutedHeightMap;
    }

}