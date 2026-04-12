using UnityEngine;

public class SpriteOutline2D : MonoBehaviour
{
    public Color OutlineColor = Color.yellow;
    public float OutlineThickness = 0.05f;

    [SerializeField] SpriteRenderer _mainSR;
    [SerializeField] SpriteRenderer _outlineSR;
    [SerializeField] GameObject _outlineGO;


    public void Init()
    {
        if (_mainSR == null) return;

        // Create child for outline if it doesn't exist
        if (_outlineGO == null)
        {
            GameObject _outlineGO = new("Outline");
            _outlineGO.transform.SetParent(transform, false);
            _outlineSR = _outlineGO.AddComponent<SpriteRenderer>();
            _outlineSR.sortingLayerID = _mainSR.sortingLayerID;
            _outlineSR.sortingOrder = _mainSR.sortingOrder - 1; // behind main sprite
        }

        _outlineSR.sprite = _mainSR.sprite;
        _outlineSR.color = OutlineColor;
        _outlineSR.transform.localScale = Vector3.one * (1f + OutlineThickness);

        _outlineGO.SetActive(false);
    }

    /*
    void Update()
    {
        if (_mainSR.sprite != _outlineSR.sprite)
            _outlineSR.sprite = _mainSR.sprite;
    }
    */

    public void Higlight(bool highlight)
    {
        //Debug.Log("Attempting highlight.");
        if (_outlineGO != null)
        {
            //Debug.Log("Highlighted???" + highlight);
            _outlineGO.SetActive(highlight);
        }
    }
}
