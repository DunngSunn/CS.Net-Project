using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore
{
    public class BoardBlock : MonoBehaviour
    {
        #region Fields
        
        // Thông tin của block
        public int BlockID { get; private set; }
        public int RowID { get; private set; }
        public int ColumnID { get; private set; }
        public bool IsFilled { get; private set; }
        
        private Image blockImage;

        private Color highlightColor = new Color(1f, 1f, 1f, .5f);
        private Color transparentColor = new Color(1f, 1f, 1f, 0f);
        
        #endregion

        #region Control functions

        // Khởi tạo thông tin block
        public void InitializeBlock(int rowID, int columnID)
        {
            BlockID = -1;
            RowID = rowID;
            ColumnID = columnID;
            IsFilled = false;
            blockImage = transform.GetChild(0).GetComponent<Image>();
        }

        // Hiển thị ảnh khi block có thể fill
        public void SetHighlightImage(Sprite sprite)
        {
            blockImage.sprite = sprite;
            blockImage.color = highlightColor;
        }

        // Dừng hiển thị ảnh khi block không fill được
        public void StopHighlighting()
        {
            blockImage.sprite = null;
            blockImage.color = transparentColor;
        }

        // Set fill cho block
        public void SetFilledBlock(Sprite sprite, int blockID)
        {
            blockImage.sprite = sprite;
            blockImage.color = Color.white;
            BlockID = blockID;
            IsFilled = true;
        }

        // Clear block có hiệu ứng.
        public void ClearBlock()
        {
            transform.GetComponent<Image>().color = transparentColor;
		
            blockImage.transform.DOScale(Vector3.zero, .35f).OnComplete(() => 
            {
                blockImage.transform.localScale = Vector3.one;
                blockImage.sprite = null;
            });

            transform.GetComponent<Image>().DOFade(1, .35f).SetDelay(.3f);
            blockImage.DOFade(0, .3f);

            BlockID = -1;
            IsFilled = false;
        }

        #endregion
    }
}