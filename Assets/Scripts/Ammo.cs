using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField] private AmmoSlot[] ammoSlots;

    [System.Serializable]
    private class AmmoSlot
    {
        public AmmoType type;
        public int ammoAmount;      // Đạn trong băng hiện tại
        public int maxAmmo = 30;    // Số đạn tối đa trong một băng đạn
        public int reserveAmmo = 90; // Đạn dự trữ
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
        }
    }

    public bool NeedsReload(AmmoType ammoType)
    {
        AmmoSlot slot = GetAmmoSlot(ammoType);
        if (slot == null) return false;

        // Cần nạp đạn khi: đạn trong băng chưa đầy VÀ còn đạn dự trữ
        return slot.ammoAmount < slot.maxAmmo && slot.reserveAmmo > 0;
    }

    public bool HasAmmoToReload(AmmoType ammoType)
    {
        AmmoSlot slot = GetAmmoSlot(ammoType);
        return slot != null && slot.reserveAmmo > 0;
    }

    public void ReloadAmmo(AmmoType ammoType)
    {
        AmmoSlot slot = GetAmmoSlot(ammoType);
        if (slot != null && slot.reserveAmmo > 0)
        {
            // Tính số đạn cần để nạp đầy băng
            int ammoNeeded = slot.maxAmmo - slot.ammoAmount;

            // Kiểm tra xem có đủ đạn dự trữ không
            int ammoToAdd = Mathf.Min(ammoNeeded, slot.reserveAmmo);

            // Nạp đạn và trừ đạn dự trữ
            slot.ammoAmount += ammoToAdd;
            slot.reserveAmmo -= ammoToAdd;
        }
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