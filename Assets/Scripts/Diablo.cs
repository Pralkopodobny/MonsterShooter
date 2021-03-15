using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Diablo : MonoBehaviour
{
    [SerializeField] private Sprite dead_diablo;
    [SerializeField] private float deathDelay;
    private IListener manager;
    private bool dead = false;
    private Image image;
    
    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public void Die()
    {
        if (!dead)
        {
            dead = true;
            StartCoroutine(DieAfterDelay());
        }
    }

    private IEnumerator DieAfterDelay()
    {
        image.sprite = dead_diablo;
        yield return new WaitForSeconds(deathDelay);
        manager?.Notify(this);
        GameObject.Destroy(gameObject);
    }

    public void SetListener(IListener listener)
    {
        manager = listener;
    }

}
