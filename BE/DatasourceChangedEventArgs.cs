using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class DatasourceChangedEventArgs : EventArgs
    {
        public DatasourceChangedEventArgs(IKey itemChanged, Changing change)
        {
            ChangedItem = itemChanged;
            Change = change;
        }

        public Changing Change { get; protected set; }

        public IKey ChangedItem { get; protected set; }

        private string _typeChange;
        public string TypeChange
        {
            get => _typeChange; protected set
            {
                value = value?.ToLower();
                if (value != "tester" && value != "trainee" && value != "test")
                    throw new ArgumentException("Not supported type");
                _typeChange = value;
            }
        }
    }

    public enum Changing : byte
    {
        Add, Update, Remove
    }
}
