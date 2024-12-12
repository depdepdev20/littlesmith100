using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceDisplayCode : MonoBehaviour
{
    public TMP_Text ironCountText;
    public TMP_Text woodCountText;
    public TMP_Text diamondCountText;
    public TMP_Text emeraldCountText;
    public TMP_Text platinumCountText;
    public TMP_Text orichalcumCountText;
    public TMP_Text amethystCountText;
    public TMP_Text obsidianCountText;
 

    void Update()
    {
        int iron = ResourceManagerCode.instance.GetResourceValue("iron");
        int wood = ResourceManagerCode.instance.GetResourceValue("wood");
        int diamond = ResourceManagerCode.instance.GetResourceValue("diamond");
        int emerald = ResourceManagerCode.instance.GetResourceValue("emerald");
        int platinum = ResourceManagerCode.instance.GetResourceValue("platinum");
        int orichalcum = ResourceManagerCode.instance.GetResourceValue("orichalcum");
        int amethyst = ResourceManagerCode.instance.GetResourceValue("amethyst");
        int obsidian = ResourceManagerCode.instance.GetResourceValue("obsidian");

        UpdateResourceCount(iron, wood, diamond, emerald, platinum, orichalcum, amethyst, obsidian);

    }

    public void UpdateResourceCount(int iron, int wood, int diamond, int emerald, int platinum, int orichalcum, int amethyst, int obsidian)
    {

        ironCountText.text = "Iron Ore: " + iron.ToString();
        woodCountText.text = "Wood: " + wood.ToString();
        diamondCountText.text = "Diamond Ore: " + diamond.ToString();
        emeraldCountText.text = "Iron Ore: " + emerald.ToString();
        platinumCountText.text = "Platinum Ore: " + platinum.ToString();
        orichalcumCountText.text = "Orichalcum Ore: " + orichalcum.ToString();
        amethystCountText.text = "Amethyst Ore: " + amethyst.ToString();
        obsidianCountText.text = "Obsidian Ore: " + obsidian.ToString();

    }
}
