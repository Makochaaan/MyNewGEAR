using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class PartsSlot
{
    public bool isOccupied;
    public bool isActive;
    public Transform slotTransform;
    public int slotLevel;
    public PlayerPartsFoundation parts;
}
public class PlayerPartsManager : MonoBehaviour
{
    private Debug_Player inputActions;
    private Transform selectionTemp;
    private bool selecting;
    [SerializeField] private PartsSlot[] partsSlots;
    // Start is called before the first frame update
    void Start()
    {
        InitializeInputAction();
        InitializePartsSlots();
    }

    private void InitializeInputAction()
    {
        inputActions = new Debug_Player();
        inputActions.Enable();
        inputActions.Game.ItemSlot1.performed += context => EquipIfPressed(1);
        inputActions.Game.ItemSlot2.performed += context => EquipIfPressed(2);
        inputActions.Game.ItemSlot3.performed += context => EquipIfPressed(3);
        inputActions.Game.ItemSlot4.performed += context => EquipIfPressed(4);
    }
    private void InitializePartsSlots()
    {
        for (int i = 0; i < partsSlots.Length; i++)
        {
            partsSlots[i].isActive = false;
            partsSlots[i].slotLevel = 0;
            partsSlots[i].parts = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        PartsSelectRay();
        if(inputActions.Game.Fire.ReadValue<float>() == 1)
        {
            UseActiveWeapon();
        }
    }
    private void PartsSelectRay()
    {
        //ray���A�C�e���ɓ������Ă���Ƃ��A�C�e���̗֊s��`�悷��B�����ƃV���v���ɂȂ�Ȃ����̂�
        Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit,100))
        {
            //�I�𒆃I�u�W�F�N�g�����݂�ray�Ɠ����Ȃ�return�A�قȂ�Ȃ�֊s�\����؂�ւ���
            if (selectionTemp == hit.transform)
            {
                return;
            }
            else
            {
                if (selectionTemp != null && selectionTemp.TryGetComponent(out Outline outlineOld))
                {
                    outlineOld.OutlineWidth = 0;
                }
                if (hit.transform != null && hit.transform.TryGetComponent(out Outline outlineNew))
                {
                    selecting = true;
                    outlineNew.OutlineWidth = 10;
                }
                else
                {
                    selecting = false;
                }
                selectionTemp = hit.transform;
            }
        }
        
        
    }
    private void EquipIfPressed(int slotNumberInArray)
    {
        if (!selecting)
        {
            if (partsSlots[slotNumberInArray].isOccupied)
            {
                if (partsSlots[slotNumberInArray].isActive)
                {
                    partsSlots[slotNumberInArray].isActive = false;
                    partsSlots[slotNumberInArray].parts.OnDeactivated();
                }
                else
                {
                    partsSlots[slotNumberInArray].isActive = true;
                    partsSlots[slotNumberInArray].parts.OnActivated();
                }
            }
            return;
        }
        else
        {
            if(selectionTemp.TryGetComponent(out PlayerPartsFoundation parts))
            {
                //���A���A�r�A�w����p�p�[�c�͂��̘g�ցA���̑��͎w�肵���g��
                switch (parts.type)
                {
                    case PlayerPartsFoundation.PartsType.Head:
                        EquipSwitchFunction(parts, 0, false);
                        break;
                    case PlayerPartsFoundation.PartsType.Body:
                        EquipSwitchFunction(parts, 2, false);
                        break;
                    case PlayerPartsFoundation.PartsType.Arms:
                        EquipSwitchFunction(parts, 1, false);
                        break;
                    case PlayerPartsFoundation.PartsType.Legs:
                        EquipSwitchFunction(parts, 5, false);
                        break;
                    case PlayerPartsFoundation.PartsType.Back:
                        EquipSwitchFunction(parts, 4, false);
                        break;
                    case PlayerPartsFoundation.PartsType.Booster:
                        EquipSwitchFunction(parts, slotNumberInArray, true);
                        break;
                    case PlayerPartsFoundation.PartsType.Gun:
                        EquipSwitchFunction(parts, slotNumberInArray, true);
                        break;
                    case PlayerPartsFoundation.PartsType.Missile:
                        EquipSwitchFunction(parts, slotNumberInArray, true);
                        break;
                    default:
                        break;
                }
            }
        }
    }
    //switch�ŋ��ʂ��镔���A�ړ��A��]�����킹�A�g�p�A�g���x��UP�A������
    private void EquipSwitchFunction(PlayerPartsFoundation parts, int slotNumber,bool activate)
    {
        parts.transform.position = partsSlots[slotNumber].slotTransform.position;
        parts.transform.rotation = partsSlots[slotNumber].slotTransform.rotation;
        parts.transform.parent = partsSlots[slotNumber].slotTransform;
        partsSlots[slotNumber].parts = parts;
        partsSlots[slotNumber].isOccupied = true;
        partsSlots[slotNumber].isActive = activate;
        partsSlots[slotNumber].slotLevel++;
        parts.OnEquipped(partsSlots[slotNumber].slotLevel);
    }
    private void UseActiveWeapon()
    {
        for (int i = 0; i < partsSlots.Length; i++)
        {
            if (partsSlots[i].isActive)
            {
                partsSlots[i].parts.UseWeapon();
            }
        }
    }
}
