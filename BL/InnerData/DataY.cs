using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.InnerData
{
    public class DataY :IComparable<DataY>
    {
        public double YSize { get; set; }

        public int Quantity { get; set; } // public int Count { get; set; } 
       

        public DateTime lastBuyDate { get; set; } // maybe to move this to the doubleLinkeList 

        public DataY(double size)
        {
            YSize = size;
            lastBuyDate = DateTime.Now;
        }

        public string ToStringAfterSell (int hasBeenSale)
        {
            return $"the y size is: { YSize} , you bought {hasBeenSale} boxes ";
        }
        public int CompareTo(DataY other)
        {
            return this.YSize.CompareTo(other.YSize);
        }

        public override string ToString()
        {
            return $"the y size is: { YSize} , there is {Quantity} in stock and last buy date of this item is {lastBuyDate}";
        }
    }
}
