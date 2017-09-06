using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZDataStructure
{
    public class ZBinaryTreeNode<T> : IComparable where T : IComparable
    {
        public ZBinaryTreeNode(T data) : this(data, null, null) { }

        public ZBinaryTreeNode(T data, ZBinaryTreeNode<T> left, ZBinaryTreeNode<T> right)
        {
            this.Value = data;
            this.LeftNode = left;
            this.RightNode = right;
        }

        public T Value { get; set; }
        public ZBinaryTreeNode<T> LeftNode { get; set; }
        public ZBinaryTreeNode<T> RightNode { get; set; }

        public bool HasChildren
        {
            get
            {
                if (this.LeftNode == null && this.RightNode == null)
                    return false;
                else
                    return true;
            }
        }

        public int CompareTo(object data)
        {
            return 0;
        }
    }
    public class ZBinaryTree<T> where T : IComparable
    {
        
        public ZBinaryTree()
        {
            Root = null;
        }

        public void Clear()
        {
            Root = null;
        }

        public ZBinaryTreeNode<T> Root { get; set; }

        public static ZBinaryTree<T> BuildTree(List<T> preorderList, List<T> inorderList)
        {
            ZBinaryTree<T> tree = new ZBinaryTree<T>();
            int preIndex = 0;
            tree.Root = BuildTree(preorderList, inorderList, 0, inorderList.Count - 1, preIndex);
            return tree;
        }

        private static ZBinaryTreeNode<T> BuildTree(List<T> preorderList, List<T> inorderList, int start, int end, int preIndex)
        {
            if (start > end)
                return null;

            //Pick current node from preorder traversal using preIndex and increment preIndex
            ZBinaryTreeNode<T> node = new ZBinaryTreeNode<T>(preorderList[preIndex++]);

            //If this node has no children then return
            if (start == end)
                return node;

            //Else find the index of this node in inorder traversal
            int inIndex = Search(inorderList, start, end, node.Value);

            //Using index in inorder traversal, construct left and right subtrees
            node.LeftNode = BuildTree(inorderList, preorderList, start, inIndex - 1, preIndex);
            node.RightNode = BuildTree(inorderList, preorderList, inIndex + 1, end, preIndex);

            return node;
        }

        private static int Search(List<T> list, int start, int end, T data)
        {
            int i;
            for (i = start; i <= end; i++)
            {
                if (list[i].Equals(data))
                    return i;
            }
            return i;
        }

        public int Count()
        {
            return LevelOrderList().Count;
        }

        public int MaxHeight()
        {
            return CountChildren(Root);
        }

        public int MaxWidth()
        {
            return 0;
        }

        private int CountChildren(ZBinaryTreeNode<T> node)
        {
            if (node == null)
                return 0;
            int left = CountChildren(node.LeftNode);
            int right = CountChildren(node.RightNode);

            if (left >= right)
                return left + 1;
            else
                return right + 1;
        }

        public ZBinaryTreeNode<T> Search(T data)
        {
            ZBinaryTreeNode<T> match = null;
            ZQueue<ZBinaryTreeNode<T>> que = new ZQueue<ZBinaryTreeNode<T>>();

            if (Root != null)
            {
                if (data.Equals(Root.Value))
                    return Root;

                if (Root.LeftNode != null)
                    que.Enqueue(Root.LeftNode);
                if (Root.RightNode != null)
                    que.Enqueue(Root.RightNode);
            }

            while (que.Count > 0)
            {
                ZBinaryTreeNode<T> node = que.Dequeue();
                if (data.Equals(node.Value))
                    return node;

                if (Root.LeftNode != null)
                    que.Enqueue(Root.LeftNode);
                if (Root.RightNode != null)
                    que.Enqueue(Root.RightNode);
            }

            return match;
        }

        public int CountLeafNodes()
        {
            int total = 0;
            ZQueue<ZBinaryTreeNode<T>> que = new ZQueue<ZBinaryTreeNode<T>>();

            if (Root != null)
            {
                if (Root.LeftNode == null && Root.RightNode == null)
                    total++;

                if (Root.LeftNode != null)
                    que.Enqueue(Root.LeftNode);
                if (Root.RightNode != null)
                    que.Enqueue(Root.RightNode);
            }

            while (que.Count > 0)
            {
                ZBinaryTreeNode<T> node = que.Dequeue();
                if (Root.LeftNode == null && Root.RightNode == null)
                    total++;

                if (Root.LeftNode != null)
                    que.Enqueue(Root.LeftNode);
                if (Root.RightNode != null)
                    que.Enqueue(Root.RightNode);
            }
            return total;
        }

        public int CountFullNodes()
        {
            int total = 0;
            ZQueue<ZBinaryTreeNode<T>> que = new ZQueue<ZBinaryTreeNode<T>>();

            if (Root != null)
            {
                if (Root.LeftNode != null && Root.RightNode != null)
                    total++;

                if (Root.LeftNode != null)
                    que.Enqueue(Root.LeftNode);
                if (Root.RightNode != null)
                    que.Enqueue(Root.RightNode);
            }

            while (que.Count > 0)
            {
                ZBinaryTreeNode<T> node = que.Dequeue();
                if (Root.LeftNode != null && Root.RightNode != null)
                    total++;

                if (Root.LeftNode != null)
                    que.Enqueue(Root.LeftNode);
                if (Root.RightNode != null)
                    que.Enqueue(Root.RightNode);
            }
            return total;
        }

        public List<ZBinaryTreeNode<T>> GetAncesters(T data)
        {
            List<ZBinaryTreeNode<T>> items = new List<ZBinaryTreeNode<T>>();

            if (Root != null)
            {
                if (!Root.Value.Equals(data))
                {
                    if (GetAncesters(Root.LeftNode, data, items))
                        items.Add(Root.LeftNode);
                    if (GetAncesters(Root.LeftNode, data, items) || GetAncesters(Root.RightNode, data, items))
                    {
                        items.Add(Root);
                    }
                }
            }
            return items;
        }

        private bool GetAncesters(ZBinaryTreeNode<T> node, T data, List<ZBinaryTreeNode<T>> items)
        {
            if (node != null)
            {
                if (!node.Value.Equals(data))
                {
                    if (GetAncesters(node.LeftNode, data, items) || GetAncesters(node.RightNode, data, items))
                    {
                        items.Add(node);
                        return true;
                    }

                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        public List<T> LevelOrderList()
        {
            List<T> items = new List<T>();
            ZQueue<ZBinaryTreeNode<T>> que = new ZQueue<ZBinaryTreeNode<T>>();
            
            if (Root != null)
            {
                items.Add(Root.Value);
                if (Root.LeftNode != null)
                    que.Enqueue(Root.LeftNode);
                if (Root.RightNode != null)
                    que.Enqueue(Root.RightNode);
            }

            while(que.Count > 0)
            {
                ZBinaryTreeNode<T> node = que.Dequeue();
                items.Add(node.Value);
                if (Root.LeftNode != null)
                    que.Enqueue(Root.LeftNode);
                if (Root.RightNode != null)
                    que.Enqueue(Root.RightNode);
            }

            return items;
        }

        public List<T> PreOrderList()
        {
            List<T> items = new List<T>();
            PreOrder(Root, items);
            return items;
        }

        public List<T> InOrderList()
        {
            List<T> items = new List<T>();
            InOrder(Root, items);
            return items;
        }

        public List<T> PostOrderList()
        {
            List<T> items = new List<T>();
            PostOrder(Root, items);
            return items;
        }

        private void PreOrder(ZBinaryTreeNode<T> node, List<T> items)
        {
            if (node != null)
            {
                PreOrder(node.LeftNode, items);
                items.Add(node.Value);
                PreOrder(node.RightNode, items);
            }
        }

        private void InOrder(ZBinaryTreeNode<T> node, List<T> items)
        {
            if (node != null)
            {
                InOrder(node.LeftNode, items);
                items.Add(node.Value);
                InOrder(node.RightNode, items);
            }
        }

        private void PostOrder(ZBinaryTreeNode<T> node, List<T> items)
        {
            if (node != null)
            {
                PostOrder(node.LeftNode, items);
                PostOrder(node.RightNode, items);
                items.Add(node.Value);
            }
        }
    }
}
