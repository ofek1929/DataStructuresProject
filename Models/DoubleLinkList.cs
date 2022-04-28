using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace Models
{
    public class DoubleLinkList<T> :IEnumerable<T> where T : IComparable<T> 
    {
       public Node start = null;
        public Node end = null;        
        int count;

        public int Count { get { return count; } }

        public void AddLast (T val)
        {
            if (start == null)
            {
                AddFirst(val);
                return;
            }
            Node newNode = new Node(val);
            end.next = newNode;
            newNode.prev = end;
            end = newNode;
            count++;
        }

        public void AddFirst (T val)
        {
            Node newNode = new Node(val);
            newNode.next = start;
            start = newNode;
            
            if (start.next == null)
                end = newNode;
            else start.next.prev = start;
            count++;
        }

        public bool RemoveFirst(out T removedValue)
        {
            removedValue = default(T);
            if (start != null)
            {
                removedValue = start.data;
                start = start.next;                
                if (start == null) end = null;
                else start.prev  =null;
                count--;
                return true;
            }

            return false;
        }

        public bool RemoveLast(out T removedValue)
        {
            removedValue = default(T);
            if (start != null)
            {
                removedValue = end.data;
                end = end.prev;
                end.next = null;
                if (end == null) start = null;
                count--;
                return true;
                
            }
            return false;

        }


        public bool AddAt(T value, int position = 0) //position -zero based position
        {
            if (position < 0) throw new ArgumentException("position must be positive or zero");

            if (position == 0)
            {
                AddFirst(value);
                return true;
            }
            if (start == null || position > count) return false;

            Node tmp = start;
            for (int i = 1; tmp != null && i < position; i++) tmp = tmp.next;

            if (tmp == null) return false;
            Node newNode = new Node(value);
            newNode.next = tmp.next;
            newNode.prev = tmp;
            tmp.next = newNode;
            count++;
            return true;
        }

        public bool GetAt(out T value, int position = 0)//position -zero based position
        {
            value = default(T);

            if (position >= count) return false;

            Node tmp = start;
            for (int i = 0; i < position; i++) tmp = tmp.next;
            value = tmp.data;
            return true;
        }

        public bool MoveToEnd (T value)
        {
            Node temp = Serch(value);
            if (temp == null) return false;
            end.next = temp;
            temp.prev = end;
            end = temp;
            return true;


        }
        public Node Serch(T value)
        {
            Node temp = start;
            if (start == null) return null;

            for (int i = 0; i < count; i++)
            {
                if (temp.data.CompareTo(value) == 0) return temp;
                else if (temp == null) return null;
                else temp = temp.next;
            }
            return null;
        }

        public bool Delete(T value)
        {
            Node temp = Serch(value);
            if (this.Removel(temp)) return true;
            else return false;
        }

        private bool Removel(Node removedValue)
        {
            if (removedValue == null) return false;
            else if (removedValue == start)
            {
                RemoveFirst(out _);
                return true;
            }
            else if (removedValue == end)
            {
                RemoveLast(out _);
                return true;
            }
            removedValue.prev.next = removedValue.next;
            removedValue.next.prev = removedValue.prev;
            return true;
        }



        public override string ToString()
        {
            Node tmp = start;
            StringBuilder sb = new StringBuilder();

            while (tmp != null)
            {
                sb.Append($"{tmp.data} ");
                tmp = tmp.next;
            }

            return sb.ToString();
        }

        public IEnumerator<T> GetEnumerator()
        {
            Node current = start;
            while (current != null)
            {
                yield return current.data;
                current = current.next;
            }
            
        }

        IEnumerator IEnumerable.GetEnumerator()//private 
        {
            return this.GetEnumerator() ;
        }

        public class Node
        {
            public T data;
            public Node next;
            public Node prev;
            public Node(T val)
            {
                data = val;
                next = null;
                prev = null;
            }

        }
    }
}
