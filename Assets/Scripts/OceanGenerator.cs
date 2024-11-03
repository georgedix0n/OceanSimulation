using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanGenerator : MonoBehaviour
{//CHANGE TO EITHER CAMELCASE OR UNDERSCORE CONSISTENT!
    int size = 256;
    int L = 1000;
    float A = 1.0f;
    Vector2 windDirection = new Vector2(1.0f,1.0f);
    float windspeed = 10.0f;

    Material planeMaterial;
    RenderTexture butterfly;

    Texture2D gauss;


    //MAKE ABOVE EDITABLE IN INSPECTOR

    void Start()
    {
        planeMaterial = Resources.Load<Material>("Materials/PlaneMaterial");
        MeshGenerator.CreatePlaneMesh(size, planeMaterial);
        butterfly = ButterflyTextureGenerator.DispatchButterflyShader(size);
        gauss = GaussRandomTextureGenerator.GenerateGaussianTexture(size);
        FourierAmplitudesGenerator.FourierRenderTextures textures = FourierAmplitudesGenerator.DispatchFourierAmplitudesShader(size, L, A, windDirection, windspeed, gauss);
        RenderTexture fourier_amplitudes = textures.amplitudeTexture;
        RenderTexture Jacobian = textures.jacobiTexture;
        RenderTexture height_map = FourierTransform.ComputeAndPermuteFFT(size, fourier_amplitudes, butterfly);
        RenderTexture jacobian_texture = FourierTransform.ComputeAndPermuteFFT(size, Jacobian, butterfly);
        TextureUtility.SaveRenderTextureToPNG(jacobian_texture, "jacobian_texture");
        TextureUtility.SaveRenderTextureToPNG(fourier_amplitudes, "fourier_amplitudes");
        
        
        planeMaterial.SetTexture("_HeightMap", height_map);
        planeMaterial.SetTexture("_Jacobian", jacobian_texture);
    }

    void Update()
    {
        FourierAmplitudesGenerator.FourierRenderTextures textures = FourierAmplitudesGenerator.DispatchFourierAmplitudesShader(size, L, A, windDirection, windspeed, gauss);
        RenderTexture fourier_amplitudes = textures.amplitudeTexture;
        RenderTexture Jacobian = textures.jacobiTexture;
        RenderTexture height_map = FourierTransform.ComputeAndPermuteFFT(size, fourier_amplitudes, butterfly);
        RenderTexture jacobian_texture = FourierTransform.ComputeAndPermuteFFT(size, Jacobian, butterfly);
        planeMaterial.SetTexture("_HeightMap", height_map);
        planeMaterial.SetTexture("_Jacobian", jacobian_texture);
    }



    

}
