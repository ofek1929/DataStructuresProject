using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class BST<T> where T : IComparable<T>
    {
        Node root;

        public bool CheckIfRootIsNull()
        {
            if (root == null) return true;
            else return false;
        }
        public bool Search(T data, out T result)
        {
            result = (T)default;
            if (root == null) // Case of an Empty tree
            {
                //result = null;
                return false;
            }

            Node temp = root;
            int comparisonResult;
            while (true)
            {
                comparisonResult = data.CompareTo(temp.data);
                if (comparisonResult < 0) // if it's smaller check the left
                {
                    if (temp.left == null)
                    {
                        //result = null;
                        return false;
                    }
                    else
                        temp = temp.left;
                }
                else if (comparisonResult > 0) // if its bigger check the right
                {
                    if (temp.right == null)
                    {
                        //result = null;
                        return false;
                    }
                    else temp = temp.right;
                }
                else // found the tresure
                {
                    result = temp.data;
                    return true;
                }
            }
        }

        public bool SerchTheClosest(T data, out T result)//in order to find the best mech - the data or one biger
        {
            Node temp = root;
            Node res = default; // null
            while (temp != null) //will be null when it is in the end of the tree 
            {
                if (data.CompareTo(temp.data) > 0)//data>temp
                {
                    temp = temp.right;
                }
                else if (data.CompareTo(temp.data) < 0)//data<temp date--->item to serch
                {
                    if (res == null || res.data.CompareTo(temp.data) > 0) res = temp;//go in for forst time or if res>temp
                    temp = temp.left;
                }
                else//data=temp
                {
                    result = temp.data;
                    return true;
                }
            }
            if (res == null) //case there is no biger size 
            {
                result = default(T);
                return false;
            }
            else // returns the closest and biger size 
            {
                result = res.data;
                return true;

            }

        }
        
        /// <summary>
        /// serch the same closet
        /// </summary>
        /// <param name="data"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool SerchTheClosestIfExist(T data, out T result)
        {
            Node res = default;
            if (Search(data, out _)) // check if data exist
            {
                Node temp = root;
                Node prev;
                while (true)//serch the data in bst
                {
                    if (data.CompareTo(temp.data) > 0)//data>temp
                    {
                        //ראס
                        temp = temp.right;
                    }
                    else if (data.CompareTo(temp.data) < 0)//data<temp date--->item to serch
                    {
                        //ראס
                        temp = temp.left;
                    }
                    else//data=temp
                    {
                        prev = res;
                        res = temp;
                        break;
                    }
                }
                while (res.left != null)
                {
                    prev = res;
                    res = res.left;
                }
                if (res.data.CompareTo(data) == 0)
                {
                    if (res.right == null) res = prev;
                    else res = res.right;                    
                }                
                result = res.data;
                return true;
            }
            else return SerchTheClosest(data, out  result); // if not exsist serch in SerchTheClosest func           
        }


        public void Add(T val)
        {
            if (root == null)
            {
                root = new Node(val);
                return;
            }

            Node tmp = root;

            while (tmp != null)
            {
                if (val.CompareTo(tmp.data) < 0) // val < tmp.data
                {
                    if (tmp.left == null)
                    {
                        tmp.left = new Node(val);
                        break;
                    }
                    tmp = tmp.left;
                }
                else
                {
                    if (tmp.right == null)
                    {
                        tmp.right = new Node(val);
                        break;
                    }
                    tmp = tmp.right;
                }
            }
        }


        public void PrintInOrderTree()
        {
            if (root == null) return;
            PrintInOrderTree(root);
        }
        private void PrintInOrderTree(Node val)
        {
            if (val.left != null) PrintInOrderTree(val.left);
            Console.WriteLine(val.data);
            if (val.right != null) PrintInOrderTree(val.right);
        }


        public int GetDepth()
        {
            return GetDepth(root);
        }
        private int GetDepth(Node val)
        {
            return (val == null) ? 0 : Math.Max(GetDepth(val.right), GetDepth(val.left)) + 1;
        }


        public bool Remove(T value)
        {
            if (root == null) return false;
            #region Check When ToRemove Is First
            if (root.data.CompareTo(value) == 0)
            {
                while (true)
                {
                    if (root.left == null && root.right == null) root = null;
                    else if (root.left != null && root.right == null) root = root.left;
                    else if (root.left == null && root.right != null) root = root.right;
                    else break;
                    return true;
                }
            }
            #endregion

            Node tmp = root;
            Node tmpParent = null;
            bool isLeft = true;
            while (true)
            {
                if (value.CompareTo(tmp.data) == 0) // check if the the removal value same to tmp - in order to remove this time
                {
                    if (tmp.right != null && tmp.left != null)// first case - it is not a leafe - tree on both sides
                    {
                        Node toBeRemoved = tmp.right;
                        Node toBeRemovedParent = tmp;
                        isLeft = false;
                        while (true)
                        {
                            if (toBeRemoved.left == null) break;
                            isLeft = true;
                            toBeRemovedParent = toBeRemoved;
                            toBeRemoved = toBeRemoved.left;
                        }

                        tmp.data = toBeRemoved.data;
                        if (toBeRemoved.right == null)
                        {
                            if (isLeft) toBeRemovedParent.left = null;
                            else toBeRemovedParent.right = null;
                        }
                        else
                        {
                            if (isLeft) toBeRemovedParent.left = toBeRemoved.right;
                            else toBeRemovedParent.right = toBeRemoved.right;
                        }
                    }
                    else if (tmp.right == null && tmp.left == null)// second case - it is a leafe
                        if (isLeft) tmpParent.left = null;
                        else tmpParent.right = null;
                    else if (tmp.right == null && tmp.left != null) // third case - there is tree on one side 
                        if (isLeft) tmpParent.left = tmp.left;
                        else tmpParent.right = tmp.left;
                    else if (tmp.right != null && tmp.left == null) // forth case - there is tree on the other side 
                        if (isLeft) tmpParent.left = tmp.right;
                        else tmpParent.right = tmp.right;

                    return true; // its retrns true because the removal have been ocerd 
                }
                else if (value.CompareTo(tmp.data) < 0) // in order to become onto the removal value
                {
                    if (tmp.left == null) return false;
                    tmpParent = tmp;
                    tmp = tmp.left;
                    isLeft = true;
                }
                else// in order to become onto the removal value
                {
                    if (tmp.right == null) return false;
                    tmpParent = tmp;
                    tmp = tmp.right;
                    isLeft = false;
                }
            }
        }

        class Node
        {
            public T data;
            public Node left;
            public Node right;

            public Node(T data)
            {
                this.data = data;
                left = right = null;
            }
        }
    }
}
