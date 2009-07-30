using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace uNhAddIns.WPF.Collections
{
    public class EditableCollection<T> : ObservableCollection<T>, IEditableCollection<T>
    {
        private IList<T> _bakupList;
        private bool _isInEditMode;

        #region IEditableObject Members

        public void BeginEdit()
        {
            _bakupList = new List<T>();
            Items.ForEach(_bakupList.Add);
            _isInEditMode = true;
        }

        public void EndEdit()
        {
            if (!_isInEditMode)
                throw new InvalidOperationException("EndEdit without a BeginEdit"); 

            _isInEditMode = false;
            _bakupList = null;
        }

        public void CancelEdit()
        {
            if(!_isInEditMode)
                throw new InvalidOperationException("CancelEdit without a BeginEdit"); 
            _isInEditMode = false;
            ClearItems();
            _bakupList.ForEach(Add);
        }

        #endregion
    }
}