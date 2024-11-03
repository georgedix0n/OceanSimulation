using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ButterflyTextureGenerator
{
    static ComputeShader butterflyShader;
    static RenderTexture butterfly;

    public static RenderTexture DispatchButterflyShader(int size)
    {
        butterflyShader = Resources.Load<ComputeShader>("Shaders/ButterflyShader");
        
        butterfly = new RenderTexture((int)(Mathf.Log(size) / Mathf.Log(2)), size, 0, RenderTextureFormat.ARGBFloat);
        butterfly.wrapMode = TextureWrapMode.Clamp; 
        butterfly.filterMode = FilterMode.Point; 
        butterfly.enableRandomWrite = true;
        butterfly.Create();

        ComputeBuffer bitReversedIndiciesBuffer = new ComputeBuffer(size, sizeof(uint));
        uint[] bitReversedIndiciesArray = new uint[size];

        butterflyShader.SetInt("N", size);

        butterflyShader.SetTexture(0, "Result", butterfly);
        butterflyShader.SetBuffer(0, "bitReversedIndicies", bitReversedIndiciesBuffer);


        butterflyShader.Dispatch(0, (int)(Mathf.Log(size) / Mathf.Log(2)), size / 16, 1);///change divisor back to 16

        bitReversedIndiciesBuffer.GetData(bitReversedIndiciesArray);
        //Debug.Log("test: " + string.Join(", ", bitReversedIndiciesArray));

        bitReversedIndiciesBuffer.Release();

        return butterfly;
    }

}