using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    private Vector2 _position;

    public void SetParams(Quaternion rotation, Vector2 position, float magnitude)
    {
        _position = position;

        transform.position = new Vector3(_position.x, _position.y, 0);
        transform.rotation = rotation;
        Vector2 spriteSize = _spriteRenderer.size;
        spriteSize.x = magnitude;
        _spriteRenderer.size = spriteSize;
    }
}
