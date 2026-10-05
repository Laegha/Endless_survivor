using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ModInfo
{
    public byte[] modIconData;
    public string modTitle;
    public string modDirectoryName;
    public string modDescription;
    public string modCreatorName;
    public string downloadUrl;

    [JsonIgnore]
    public static string modsDirectoryPath = Path.Combine(Application.persistentDataPath, "Mods");
    [JsonIgnore]
    public string DirectoryPath => Path.Combine(modsDirectoryPath, modDirectoryName);
    [JsonIgnore]
    public Sprite modIcon { get 
        {
            Sprite iconSprite = default;
            if (modIconData != null && modIconData.Length > 0)
            {
                var tex = new Texture2D(2, 2);
                if (tex.LoadImage(modIconData)) // resizes the texture to the image
                {
                    iconSprite = Sprite.Create(
                        tex,
                        new Rect(0, 0, tex.width, tex.height),
                        new Vector2(0.5f, 0.5f));
                }
            }
            return iconSprite;
        }
        set
        {
            modIconData = value.texture.EncodeToPNG();
        }
    }
}
