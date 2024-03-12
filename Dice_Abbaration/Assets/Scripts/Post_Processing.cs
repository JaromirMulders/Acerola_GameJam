using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Post_Processing : MonoBehaviour
{
    public Material[] materials;
    public RenderTexture[] renderTextures;


    private void Awake()
    {
        renderTextures = new RenderTexture[2];
        renderTextures[0] = new RenderTexture(Screen.width, Screen.height, 0);
        renderTextures[1] = new RenderTexture(Screen.width, Screen.height, 0);
        renderTextures[0].filterMode = FilterMode.Point;
        renderTextures[1].filterMode = FilterMode.Point;
        renderTextures[0].name = gameObject.name + "_0";
        renderTextures[1].name = gameObject.name + "_1";

    }

    private void OnDestroy()
    {
        // Release the render textures when the script is destroyed or no longer needed
        renderTextures[0].Release();
        renderTextures[1].Release();
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, renderTextures[0]);

        for (int i = 0; i < materials.Length; i++)
        {
            Graphics.Blit(renderTextures[i % 2], renderTextures[(i + 1) % 2], materials[i]);
        }

        Graphics.Blit(renderTextures[materials.Length % 2], dest);
    }
}
