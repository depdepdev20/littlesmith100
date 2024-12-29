using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceDisplayCode : MonoBehaviour
{
    public TMP_Text woodCountText;
    public TMP_Text ironCountText;
    public TMP_Text silverCountText;
    public TMP_Text copperCountText;
    public TMP_Text emeraldCountText;
    public TMP_Text diamondCountText;
    public TMP_Text platinumCountText;
    public TMP_Text orichalcumCountText;
    public TMP_Text amethystCountText;
    public TMP_Text obsidianCountText;
 

    void Update()
    {
        int wood = ResourceManagerCode.instance.GetResourceValue("wood");
        int iron = ResourceManagerCode.instance.GetResourceValue("iron");
        int silver = ResourceManagerCode.instance.GetResourceValue("silver");
        int copper = ResourceManagerCode.instance.GetResourceValue("copper");
        int emerald = ResourceManagerCode.instance.GetResourceValue("emerald");
        int diamond = ResourceManagerCode.instance.GetResourceValue("diamond");
        int platinum = ResourceManagerCode.instance.GetResourceValue("platinum");
        int orichalcum = ResourceManagerCode.instance.GetResourceValue("orichalcum");
        int amethyst = ResourceManagerCode.instance.GetResourceValue("amethyst");
        int obsidian = ResourceManagerCode.instance.GetResourceValue("obsidian");

        UpdateResourceCount(wood, iron, silver, copper, emerald, diamond, platinum, orichalcum, amethyst, obsidian);

    }

    public void UpdateResourceCount(int wood, int iron, int silver, int copper, int emerald, int diamond, int platinum, int orichalcum, int amethyst, int obsidian)
    {
        woodCountText.text = "Wood: " + wood.ToString();
        ironCountText.text = "Iron Ore: " + iron.ToString();
        silverCountText.text = "Silver Ore: " + silver.ToString();
        copperCountText.text = "Copper Ore: " + copper.ToString();
        emeraldCountText.text = "Emerald Ore: " + emerald.ToString();
        diamondCountText.text = "Diamond Ore: " + diamond.ToString();
        platinumCountText.text = "Platinum Ore: " + platinum.ToString();
        orichalcumCountText.text = "Orichalcum Ore: " + orichalcum.ToString();
        amethystCountText.text = "Amethyst Ore: " + amethyst.ToString();
        obsidianCountText.text = "Obsidian Ore: " + obsidian.ToString();

    }
}
