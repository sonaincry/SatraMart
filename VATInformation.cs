using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatraMart
{
    public class VATInformation
    {
        public string COMBINATION { get; set; }
        public string CUSTREQUEST { get; set; }
        public string FORMFORMAT { get; set; }
        public string FORMNUM { get; set; }
        public string INVOICEDATE { get; set; }
        public string INVOICENUM { get; set; }
        public string PURCHASERNAME { get; set; }
        public string RETAILTRANSACTIONTABLE { get; set; }
        public string RETAILTRANSRECIDGROUP { get; set; }
        public string SERIALNUM { get; set; }
        public string TAXCOMPANYADDRESS { get; set; }
        public string TAXCOMPANYNAME { get; set; }
        public string TAXREGNUM { get; set; }
        public string TAXTRANSTXT { get; set; }
        public string TRANSTIME { get; set; }
        public string DATAAREAID { get; set; }
        public int? RECVERSION { get; set; }
        public long? PARTITION { get; set; }
        public long RECID { get; set; }
        public string EMAIL { get; set; }
        public string PHONE { get; set; }
        public string CUSTACCOUNT { get; set; }
        public bool? CANCEL { get; set; }
    }
}
