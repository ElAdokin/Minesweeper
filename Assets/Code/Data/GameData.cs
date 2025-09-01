using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Scriptable Objects/GameData")]
public class GameData : ScriptableObject
{
    [SerializeField] private GridData _gridData;
    [SerializeField] private int _bombs;
    [SerializeField] private Font _font;
    [SerializeField] private Material _emptySelectedMaterial;
    [SerializeField] private Material _bombSelectedMaterial;
    [SerializeField] private Material _cautionSelectedMaterial;
    [SerializeField] private Material _hoverMaterial;
    [SerializeField] private Material _cautionHoverMaterial;
    [SerializeField] private int _sceneIndex;
    [SerializeField] private float _checkBombFrequency;

    public GridData GridData => _gridData;
    public int Bombs { get => _bombs; set => _bombs = value; }
    public Font Font => _font;
    public Material EmptySelectedMaterial => _emptySelectedMaterial; 
    public Material BombSelectedMaterial => _bombSelectedMaterial; 
    public Material CautionSelectedMaterial => _cautionSelectedMaterial;
    public Material HoverMaterial => _hoverMaterial;
    public Material CautionHoverMaterial => _cautionHoverMaterial;
    public int SceneIndex { get => _sceneIndex; set => _sceneIndex = value; }
    public float CheckBombFrequency => _checkBombFrequency;
}
