using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;


namespace BL.InnerData
{
    public class DataX : IComparable<DataX>
    {
        public double XSize { get; set; }
        public BST<DataY> BstY { get; set; }

        public DataX(double size)
        {
            XSize = size;
            BstY = new BST<DataY>();
        }


        public int CompareTo(DataX other)
        {
            return this.XSize.CompareTo(other.XSize);
        }

        public override string ToString()
        {
            return $"x size is {XSize}";
        }
    }
}
