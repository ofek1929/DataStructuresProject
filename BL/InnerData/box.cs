using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.InnerData
{
    public class box : IComparable<box>
    {
   
        public DataX X { get; set; }

        public DataY Y { get; set; }

       

        public box(DataX x,DataY y)
        {
            X = x;
            Y= y;
            
        }

        public string ToStringSell(int amount) => $"{X.ToString()},{Y.ToStringAfterSell(amount)}";

        public override string ToString()
        {
            return $"{X.ToString()},{Y.ToString()}";
        }

        public int CompareTo(box other)
        {
            if (this.X.CompareTo(other.X) == 0 && this.Y.CompareTo(other.Y) == 0) return 0;

            else if (this.X.CompareTo(other.X) > 0 || this.X.CompareTo(other.X) < 0) return 1;

            else if (this.Y.CompareTo(other.Y) > 0 || this.Y.CompareTo(other.Y) < 0) return -1;

            return -1;
        }
    }
}
