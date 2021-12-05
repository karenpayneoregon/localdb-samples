using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LocalDbLibrary.Models
{
    public class Customer : INotifyPropertyChanged
    {

        private int identifier;
        private string companyName;
        private string contactName;

        public int Identifier
        {
            get => identifier; set { identifier = value; OnPropertyChanged(); }
        }

        public string CompanyName
        {
            get => companyName; set { companyName = value; OnPropertyChanged(); }
        }

        public string ContactName
        {
            get => contactName; set { contactName = value; OnPropertyChanged(); }
        }

        public override string ToString() => CompanyName;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
