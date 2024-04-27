using UnityEngine;
using TMPro;

public class DynamicTMP : MonoBehaviour
{
    public TMP_Text tmp;

    [Header("ǿ������")]
    public float sinWaveMul ; // sin��ǿ�� 
    public float SportMul ; // �˶�ǿ�� 

    void Update()
    {
        tmp.ForceMeshUpdate();

        var textInfo = tmp.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
            for (int j = 0; j < 4; j++)
            {
                var orig = verts[charInfo.vertexIndex + j];
                //����
                verts[charInfo.vertexIndex + j] = orig + new Vector3(0, Mathf.Sin(Time.time * 2f + orig.x * 0.45f) * 0.3f * sinWaveMul) * SportMul;

            }
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            tmp.UpdateGeometry(meshInfo.mesh, i);
        }
    }
}