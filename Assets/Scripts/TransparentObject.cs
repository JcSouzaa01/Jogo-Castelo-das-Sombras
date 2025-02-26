using UnityEngine;
using System.Collections;

public class TransparentObject : MonoBehaviour
{

    [Range(0,1)]
    [SerializeField] private float _transparencyValue = 0.7f;
    [SerializeField] private float _transparencyFadeTime = .4f;

    private SpriteRenderer _spriteRender;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _spriteRender = GetComponent<SpriteRenderer>();
    }

   private void OnTriggerEnter2D(Collider2D collision) 
   {
        if(collision.gameObject.GetComponent<PlayerController>())
        {
            StartCoroutine(FadeTree(_spriteRender, _transparencyFadeTime, _spriteRender.color.a, _transparencyValue));
        }
    }

    private void OnTriggerExit2D(Collider2D collision) 
    {
        if(collision.gameObject.GetComponent<PlayerController>())
        {
            StartCoroutine(FadeTree(_spriteRender, _transparencyFadeTime, _spriteRender.color.a, 1f));
        }
    }    

  
   private IEnumerator FadeTree(SpriteRenderer _spriteTransparecy, float _fadeTime, float _startValue, float _targetTransparency)
   {
        float _timeElapsed = 0;
        while(_timeElapsed < _fadeTime)
        {
            _timeElapsed += Time.deltaTime;
            float _newAlpha = Mathf.Lerp(_startValue, _targetTransparency, _timeElapsed / _fadeTime);
            _spriteTransparecy.color = new Color(_spriteTransparecy.color.r, _spriteTransparecy.color.g, _spriteTransparecy.color.b, _newAlpha);
            yield return null;
        }

        
   }
}
