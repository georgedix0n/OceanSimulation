using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FourierAmplitudesGenerator
{
    static ComputeShader fourierAmplitudesShader;

    static RenderTexture tilde_h0k;

    static RenderTexture tilde_h0minusk;

    static RenderTexture fourier_amplitudes;

    static RenderTexture Displacement;

    static RenderTexture Jacobian;

    public struct FourierRenderTextures
    {
        public RenderTexture amplitudeTexture;
        public RenderTexture jacobiTexture;
    }

    


    /// CHANGE IN VAR NAMES TO MORE DESCRIPTIVE
    public static FourierRenderTextures  DispatchFourierAmplitudesShader(int size, int L, float A, Vector2 windDirection, float windspeed, Texture2D gauss_random_texture)
    {
        fourierAmplitudesShader = Resources.Load<ComputeShader>("Shaders/FourierAmplitudesShader");


        tilde_h0k = new RenderTexture(size, size, 0, RenderTextureFormat.ARGBFloat);
        tilde_h0k.wrapMode = TextureWrapMode.Clamp; 
        tilde_h0k.filterMode = FilterMode.Point; 
        tilde_h0k.enableRandomWrite = true;
        tilde_h0k.Create();

        tilde_h0minusk = new RenderTexture(size, size, 0, RenderTextureFormat.ARGBFloat);
        tilde_h0minusk.wrapMode = TextureWrapMode.Clamp; 
        tilde_h0minusk.filterMode = FilterMode.Point; 
        tilde_h0minusk.enableRandomWrite = true;
        tilde_h0minusk.Create();


        //REMOVE
        fourier_amplitudes = new RenderTexture(size, size, 0, RenderTextureFormat.ARGBFloat);
        fourier_amplitudes.wrapMode = TextureWrapMode.Clamp; 
        fourier_amplitudes.filterMode = FilterMode.Point; 
        fourier_amplitudes.enableRandomWrite = true;
        fourier_amplitudes.Create();

        Displacement = new RenderTexture(size, size, 0, RenderTextureFormat.ARGBFloat);
        Displacement.wrapMode = TextureWrapMode.Clamp; 
        Displacement.filterMode = FilterMode.Point; 
        Displacement.enableRandomWrite = true;
        Displacement.Create();

        Jacobian = new RenderTexture(size, size, 0, RenderTextureFormat.RFloat);
        Jacobian.wrapMode = TextureWrapMode.Clamp; 
        Jacobian.filterMode = FilterMode.Point; 
        Jacobian.enableRandomWrite = true;
        Jacobian.Create();



        

        fourierAmplitudesShader.SetTexture(0, "tilde_h0k", tilde_h0k);
        fourierAmplitudesShader.SetTexture(0, "tilde_h0minusk", tilde_h0minusk);
        fourierAmplitudesShader.SetTexture(0, "fourier_amplitudes", fourier_amplitudes);
        fourierAmplitudesShader.SetTexture(0, "Displacement", Displacement);
        fourierAmplitudesShader.SetTexture(0, "Jacobian", Jacobian);

        fourierAmplitudesShader.SetTexture(0, "gauss_random_texture", gauss_random_texture);

        

        fourierAmplitudesShader.SetInt("N", size);
        fourierAmplitudesShader.SetInt("L", L);
        fourierAmplitudesShader.SetFloat("A", A);
        
        fourierAmplitudesShader.SetFloat("time", Time.time);
        fourierAmplitudesShader.SetVector("windDirection", windDirection);
        fourierAmplitudesShader.SetFloat("windspeed", windspeed);


        int threadGroupsX = Mathf.CeilToInt(size / 16.0f);///change divisor back to 16
        int threadGroupsY = Mathf.CeilToInt(size / 16.0f);///change divisor back to 16
        fourierAmplitudesShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);

        return new FourierRenderTextures
        {
            amplitudeTexture = Displacement,
            jacobiTexture = Jacobian
        };
    }

}