using UnityEngine;
using UnityEngine.UI;

public class Ammo : MonoBehaviour
{
    public Text totalBullet;  // Hiển thị đạn dự trữ
    public Text currentBullet; // Hiển thị đạn hiện tại
    [SerializeField] private AmmoSlot[] ammoSlots;

    [System.Serializable]
    private class AmmoSlot
    {
        public AmmoType type;
        public int ammoAmount;      // Đạn trong băng hiện tại
        public int maxAmmo = 30;    // Số đạn tối đa trong một băng đạn
        public int reserveAmmo = 90; // Đạn dự trữ
    }

    private void UpdateUI(AmmoSlot slot)
    {
        if (slot != null && currentBullet != null && totalBullet != null)
        {
            currentBullet.text = slot.ammoAmount.ToString();
            totalBullet.text = slot.reserveAmmo.ToString();
        }
    }

    public void UpdateAmmoUI(AmmoType ammoType)
    {
        AmmoSlot slot = GetAmmoSlot(ammoType);
        UpdateUI(slot);
    }

    public int GetCurrentAmmo(AmmoType ammoType)
    {
        AmmoSlot slot = GetAmmoSlot(ammoType);
        return slot != null ? slot.ammoAmount : 0;
    }

    public int GetMaxAmmo(AmmoType ammoType)
    {
        AmmoSlot slot = GetAmmoSlot(ammoType);
        return slot != null ? slot.maxAmmo : 0;
    }

    public int GetReserveAmmo(AmmoType ammoType)
    {
        AmmoSlot slot = GetAmmoSlot(ammoType);
        return slot != null ? slot.reserveAmmo : 0;
    }

    public void ReduceCurrentAmmo(AmmoType ammoType)
    {
        AmmoSlot slot = GetAmmoSlot(ammoType);
        if (slot != null)
        {
            slot.ammoAmount--;
            UpdateUI(slot);
        }
    }

    public void ReloadAmmo(AmmoType ammoType)
    {
        AmmoSlot slot = GetAmmoSlot(ammoType);
        if (slot != null && slot.reserveAmmo > 0)
        {
            int ammoNeeded = slot.maxAmmo - slot.ammoAmount;
            int ammoToAdd = Mathf.Min(ammoNeeded, slot.reserveAmmo);
            slot.ammoAmount += ammoToAdd;
            slot.reserveAmmo -= ammoToAdd;
            UpdateUI(slot);
        }
    }

    public bool NeedsReload(AmmoType ammoType)
    {
        AmmoSlot slot = GetAmmoSlot(ammoType);
        if (slot == null) return false;
        return slot.ammoAmount < slot.maxAmmo && slot.reserveAmmo > 0;
    }

    public bool HasAmmoToReload(AmmoType ammoType)
    {
        AmmoSlot slot = GetAmmoSlot(ammoType);
        return slot != null && slot.reserveAmmo > 0;
    }

    private AmmoSlot GetAmmoSlot(AmmoType ammoType)
    {
        foreach (AmmoSlot slot in ammoSlots)
        {
            if (slot.type == ammoType) return slot;
        }
        return null;
    }
}