using UnityEngine;
using UnityEngine.UI;

public class PartyStatusUI : MonoBehaviour
{
    private Image[] partyStatusIcons;

    // Start is called before the first frame update
    private void Start()
    {
        this.partyStatusIcons = this.GetComponentsInChildren<Image>();
    }

    public void PartyPokemonFaintedAt(int index)
    {
        if (ValidateIndex(index))
        {
            partyStatusIcons[index].color = ColorPalette.AddShade(Color.white, 4);
        }
        else
        {
            Debug.LogError($"index: {index} not valid for party size");
        }
    }

    public void PopulatePartyStatusIcons(int partySize)
    {
        int size = partyStatusIcons.Length;
        if (partySize <= size)
        {
            for (int i = partySize; i < size; i++)
            {
                partyStatusIcons[i].color = new Color(0, 0, 0, 0);
            }
        }
        else
        {
            Debug.LogError($"party size: {partySize} is too large (>6)");
        }
    }

    private bool ValidateIndex(int index)
    {
        return index >= 0 && index < partyStatusIcons.Length;
    }
}