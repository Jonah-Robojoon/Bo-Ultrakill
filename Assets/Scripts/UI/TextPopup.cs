using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextPopup : MonoBehaviour
{
    [SerializeField] private TextMeshPro template;
    [SerializeField] private float lineSpacing = 0.25f;

    [SerializeField] private float lifetime = 1f;
    [SerializeField] private int maxEntries = 6;

    private readonly List<TextMeshPro> active = new();

    private void Start()
    {
        template.gameObject.SetActive(false);
    }

    public void AddEntry(string text)
    {
        
        TextMeshPro tmp = Instantiate(template, template.transform.parent, false);
        tmp.gameObject.SetActive(true);
        tmp.text = text;

       
        active.Insert(0, tmp);

        Reposition();

        if (active.Count > maxEntries)
        {
            Destroy(active[^1].gameObject);
            active.RemoveAt(active.Count - 1);
        }

        StartCoroutine(RemoveAfter(tmp, lifetime));
    }

    private void Reposition()
    {
        Vector3 baseLocalPos = template.transform.localPosition;

        for (int i = 0; i < active.Count; i++)
        {
            active[i].transform.localPosition = baseLocalPos - Vector3.forward * (i * lineSpacing);
        }
    }

    private IEnumerator RemoveAfter(TextMeshPro tmp, float time)
    {
        yield return new WaitForSeconds(time);

        if (tmp != null)
        {
            active.Remove(tmp);
            Destroy(tmp.gameObject);
            Reposition();
        }
    }
}
