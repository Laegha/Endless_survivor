using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeUpdateInstance
{
    public List<Tile> tilesToUpdate;
    public Biome biome;
    public BiomeUpdateInstance(List<Tile> tilesToUpdate, Biome biome)
    {
        this.tilesToUpdate = tilesToUpdate;
        this.biome = biome;
    }

}
