using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore
{
    public class BoardBlock : MonoBehaviour
    {
        #region Fields
        
        // Thông tin của block
        public int RowID { get; private set; }
        public int ColumnID { get; private set; }
        public bool IsFilled { get; private set; }
        
        private Image _blockImage;
        private Image _thisImage;
        
        private readonly Color _highlightColor = new Color(1f, 1f, 1f, .5f);
        private readonly Color _transparentColor = new Color(1f, 1f, 1f, 0f);
        
        #endregion

        #region Control functions

        // Khởi tạo thông tin block
        public void InitializeBlock(int rowID, int columnID)
        {
            RowID = rowID;
            ColumnID = columnID;
            IsFilled = false;
            _thisImage = transform.GetComponent<Image>();
            _blockImage = transform.GetChild(0).GetComponent<Image>();
            
            StopHighlighting();
        }

        // Hiển thị ảnh khi block có thể fill
        public void SetHighlightImage()
        {
            _blockImage.gameObject.SetActive(true);
            _blockImage.color = _highlightColor;
        }

        // Dừng hiển thị ảnh khi block không fill được
        public void StopHighlighting()
        {
            _blockImage.color = _transparentColor;
            _blockImage.gameObject.SetActive(false);
        }

        // Set fill cho block
        public void SetFilledBlock()
        {
            _blockImage.gameObject.SetActive(true);
            _blockImage.color = Color.white;
            IsFilled = true;
        }

        // Clear block có hiệu ứng.
        public void ClearBlockWithEffect()
        {
            _thisImage.color = _transparentColor;
		
            _blockImage.transform.DOScale(Vector3.zero, .35f).OnComplete(() => 
            {
                _blockImage.transform.localScale = Vector3.one;
            });

            _thisImage.DOFade(1, .35f).SetDelay(.3f);
            _blockImage.DOFade(0, .3f);

            IsFilled = false;

            _blockImage.gameObject.SetActive(false);
        }

        // Clear block
        public void ClearBlock()
        {
            IsFilled = false;

            _blockImage.gameObject.SetActive(false);
        }

        #endregion
    }
}