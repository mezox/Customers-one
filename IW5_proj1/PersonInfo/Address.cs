using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;
using System.Xml.Serialization;

namespace IW5_proj1
{
    public class Address
    {
        #region Constructors
        public Address() { }

        public Address(string street, string city, string zipcode, string country)
        {
            this.m_street = street;
            this.m_city = city;
            this.m_zipcode = zipcode;
            this.m_country = country;
        }
        #endregion

        #region Properties
        [XmlElement("Street")]
        public string m_street { get; set; }

        [XmlElement("City")]
        public string m_city { get; set; }

        [XmlElement("Zipcode")]
        public string m_zipcode { get; set; }

        [XmlElement("Country")]
        public string m_country { get; set; }
        #endregion

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
}
