using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace SettingsManager.ViewModels
{
    public class KeyValueViewModel : SelectableViewModel, IEditableObject, IDataErrorInfo
    {
        private string _key;
        public string Key
        {
            get { return _key; }
            set
            {
                if (_key == value) return;

                _key = value;

                OnPropertyChanged("Key");
            }
        }

        private string _value;
        public string Value
        {
            get { return _value; }
            set
            {
                if (_value == value) return;

                _value = value;

                OnPropertyChanged("Value");
            }
        }

        private KeyValueViewModel _backupData;
        private bool _editing;

        public void BeginEdit()
        {
            if (_editing) return;

            _editing = true;
            _backupData = MemberwiseClone() as KeyValueViewModel;
        }

        public void CancelEdit()
        {
            if (!_editing) return;

            Key = _backupData.Key;
            Value = _backupData.Value;
        }

        public void EndEdit()
        {
            if (!_editing) return;

            _editing = false;
            _backupData = null;
        }

        public string Error
        {
            get
            {
                var sb = new StringBuilder();

                sb.AppendLine(this["Key"]);
                sb.AppendLine(this["Value"]);

                return sb.ToString().Trim();
            }
        }

        public string this[string columnName]
        {
            get 
            {
                var error = string.Empty;

                switch (columnName.ToLowerInvariant())
                {
                    case "key":
                        if (string.IsNullOrWhiteSpace(Key)) error = "Key must be specified";
                        break;
                }

                return error;
            }
        }
    }
}
