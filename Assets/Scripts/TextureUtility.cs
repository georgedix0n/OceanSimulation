using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
/// REQUIRES TEXTURES FOLDER, CHANGE!
public static class TextureUtility
{
    public static void SaveTextureAsPNG(Texture2D texture, string filePath)
    {
        byte[] pngData = texture.EncodeToPNG();
        if (pngData != null)
        {
            string path = "/Textures/" + filePath;
            File.WriteAllBytes(path, pngData);
            Debug.Log("HeightMap saved as PNG at: " + path);
        }
        else
        {
            Debug.LogError("Failed to save height map as PNG.");
        }
    }

    public static void SaveRenderTextureToPNG(RenderTexture selectedTex, string name)
    {
        if (name == "butterfly" || name == "test")
        {
            RenderTexture stretchedTex = new RenderTexture(256, 256, 0); //CHANGE BACK TO 256,256,0
            stretchedTex.wrapMode = TextureWrapMode.Clamp;
            stretchedTex.filterMode = FilterMode.Bilinear;
            stretchedTex.enableRandomWrite = true;
            stretchedTex.Create();

            // Stretch the input RenderTexture to 256x256 using Graphics.Blit
            Graphics.Blit(selectedTex, stretchedTex);

            // Read the stretched RenderTexture and save as PNG
            Texture2D tex2D = new Texture2D(stretchedTex.width, stretchedTex.height, TextureFormat.RGBA32, false);
            RenderTexture.active = stretchedTex;
            tex2D.ReadPixels(new Rect(0, 0, stretchedTex.width, stretchedTex.height), 0, 0);
            tex2D.Apply();
            RenderTexture.active = null;

            // Encode to PNG
            byte[] bytes = tex2D.EncodeToPNG();
            string filePath = Application.dataPath + "/Textures/" + name + ".png";
            File.WriteAllBytes(filePath, bytes);

            Debug.Log("Saved stretched texture to: " + filePath);

        }
        else{
            Texture2D tex2D = new Texture2D(selectedTex.width, selectedTex.height, TextureFormat.RGBA32, false);
            RenderTexture.active = selectedTex;
            tex2D.ReadPixels(new Rect(0, 0, selectedTex.width, selectedTex.height), 0, 0);
            tex2D.Apply();
            RenderTexture.active = null;

            byte[] bytes = tex2D.EncodeToPNG();
            string filePath = Application.dataPath + "/Textures/" + name + ".png";
            File.WriteAllBytes(filePath, bytes);

            Debug.Log("Saved texture to: " + filePath);
        }
    }


}