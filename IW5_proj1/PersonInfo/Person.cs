using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

using System.Xml;
using System.Xml.Serialization;

namespace IW5_proj1
{
    [Serializable]

    public class CustomersCollection: ObservableCollection<Person>,INotifyPropertyChanged
    {
        public CustomersCollection() : base() { }

        public Person Person
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
    }

    public class Person
    {
        public enum T_Sex
        {
            T_MALE,
            T_FEMALE
        }

        #region Class Variables
        [XmlElement("Name")]
        private string m_name;
        
        [XmlElement("Surname")]
        private string m_surname;
        
        [XmlElement("Sex")]
        public string m_sex { get; set; }

        [XmlElement("Age")]
        public uint m_age { get; set; }

        [XmlElement("Address")]
        public Address m_address { get; set; }

        [XmlElement("CustomerID")]
        public int m_id { get; set; }
        #endregion


        #region Constructors
        public Person()
        {
            m_address = new Address();
        }

        //creates Person object
        public Person(  string name, string surname,
                        T_Sex s, uint age,
                        string street, string city, string zipcode, string country,
                        int id = 0)
        {
            this.m_name = name.Substring(0, 1).ToUpper() + name.Substring(1);
            this.m_surname = surname.Substring(0, 1).ToUpper() + surname.Substring(1); ;
            this.m_address = new Address(street,city,zipcode,country);
            this.m_age = age;

            if(s == T_Sex.T_FEMALE)
                this.m_sex = "Female";
            else
                this.m_sex = "Male";

            this.m_id = id;
        }
        #endregion

        #region Public Properties
        public string Name
        {
            get { return m_name; }
            set { m_name = value.Substring(0, 1).ToUpper() + value.Substring(1); }
        }

        public string Surname
        {
            get { return m_surname; }
            set { m_surname = value.Substring(0, 1).ToUpper() + value.Substring(1); }
        }
        #endregion


    }
}
