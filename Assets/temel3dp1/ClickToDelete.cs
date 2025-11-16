using UnityEngine;

public class ClickToDelete : MonoBehaviour
{
    void Update()
    {
        // Sol fare tuşuna basıldığında
        if (Input.GetMouseButtonDown(0))
        {
            // Ekrandan bir ray gönder
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Eğer bir objeye çarparsa
            if (Physics.Raycast(ray, out hit))
            {
                // Objeyi sil
                Destroy(hit.collider.gameObject);
            }
        }
    }
}
