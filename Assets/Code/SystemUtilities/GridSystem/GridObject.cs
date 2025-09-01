using System;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private readonly Vector3 NO_PIXEL_EULER_ANGLES = new Vector3(90f, 0f, 90f);
    private readonly Vector3 PIXEL_EULER_ANGLES = new Vector3(90f, 0f, 0f);
    
    private readonly Vector3 PIXEL_SCALE = new Vector3(1.13f, 1f, 1f);
    private readonly Vector3 BOMB_SCALE = new Vector3(.8f, .8f, .8f);

    private GameManager _gameManager;
    private Transform _visualTransform;

    private GameObject _state;
    private GameObject _hover;
   
    private bool _isSelected;

    private List<GridObject> _neighbors = new List<GridObject>();

    private CellType _cellType = CellType.NoBombNear;

    public Action<GridObject> NotifySelected;

    public Transform VisualTransform => _visualTransform;

    public bool IsSelected
    {
        get => _isSelected;

        set
        {
            _isSelected = value;

            if (_isSelected)
                Selected();
        }
    }

    public List<GridObject> Neighbors { get => _neighbors; set => _neighbors = value; }
    public CellType CellType { get => _cellType; set => _cellType = value; }

    public void SetVisualTransform(Transform visualTransform, GameManager gameManager)
    {
        _visualTransform = visualTransform;
        _gameManager = gameManager;
        _state = _visualTransform.Find("State")?.gameObject;
        _hover = _visualTransform.Find("Hover")?.gameObject;
    }

    private void SetState(bool isActive, int index)
    {
        if (_state is null) return;
        
        switch (index)
        {
            case 0:
                _state.GetComponent<MeshRenderer>().sharedMaterial = _gameManager.GameData.EmptySelectedMaterial;
                _state.transform.eulerAngles = NO_PIXEL_EULER_ANGLES;
                _state.transform.localScale = Vector3.one;
                break;

            case 1:
                _state.GetComponent<MeshRenderer>().sharedMaterial = _gameManager.GameData.BombSelectedMaterial;
                _state.transform.eulerAngles = PIXEL_EULER_ANGLES;
                _state.transform.localScale = BOMB_SCALE;
                break;

            case 2:
                _state.GetComponent<MeshRenderer>().sharedMaterial = _gameManager.GameData.CautionSelectedMaterial;
                _state.transform.eulerAngles = PIXEL_EULER_ANGLES;
                _state.transform.localScale = PIXEL_SCALE;
                break;

            default:
                _state.GetComponent<MeshRenderer>().sharedMaterial = _gameManager.GameData.EmptySelectedMaterial;
                _state.transform.eulerAngles = NO_PIXEL_EULER_ANGLES;
                _state.transform.localScale = Vector3.one;
                break;
        }

        _state.SetActive(isActive);
    }

    private void SetHover(bool isActive, bool isCautionHover)
    {
        if (_hover is null) return;

        _hover.GetComponent<MeshRenderer>().sharedMaterial = isCautionHover ?
            _gameManager.GameData.CautionHoverMaterial:
            _gameManager.GameData.HoverMaterial;

        _hover.transform.eulerAngles = isCautionHover ?
            PIXEL_EULER_ANGLES :
            NO_PIXEL_EULER_ANGLES;

        _hover.transform.localScale = isCautionHover ?
            PIXEL_SCALE :
            Vector3.one;

        _hover.SetActive(isActive);
    }

    public void Default()
    {
        if(!_isSelected)
            SetState(false, 0);
        
        SetHover(false, false);
    }

    public void Hover()
    {
       SetHover(true, false);
    }

    public void HoverBomb()
    {
        SetHover(true, true);
    }

    public void Selected()
    {
        SetState(true, 0);
        NotifySelected.Invoke(this);
    }

    public void Clear() 
    {
        _isSelected = true;
        SetState(true, 0);
    }

    public void MarkAsBomb()
    {
        _isSelected = true;
        SetState(true, 2);
    }

    public void Explode() 
    {
        _isSelected = true;
        SetState(true, 1);
    }
}
