using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZDataStructure
{
    public class ZBinarySearchTree<T> : ZBinaryTree<T> where T : IComparable
    {
        public ZBinarySearchTree() : base () { }

        
        public void Add(T data)
        {
            ZBinaryTreeNode<T> current = Root;

            if (current == null)
                Root = new ZBinaryTreeNode<T>(data);
            else
            {
                while (true)
                {
                    int compareResult = current.Value.CompareTo(data);

                    if (compareResult < 0)
                    {
                        if (current.RightNode == null)
                        {
                            current.RightNode = new ZBinaryTreeNode<T>(data);
                            return;
                        }
                        else
                            current = current.RightNode;
                        
                    }
                    if (compareResult > 0)
                    {
                        if (current.LeftNode == null)
                        {
                            current.LeftNode = new ZBinaryTreeNode<T>(data);
                            return;
                        }
                        else
                            current = current.LeftNode;
                    }
                    else
                        return;
                }
            }
        }

        public void Delete(T data)
        {
            ZBinaryTreeNode<T> node = null;
            ZBinaryTreeNode<T> parent = FindParent(data);
            if (parent != null)
            {
                if (parent.LeftNode != null && parent.LeftNode.Value.Equals(data))
                {
                    node = parent.LeftNode;
                    //case1: has no children.
                    if (node.LeftNode == null && node.RightNode == null)
                        parent.LeftNode = null;
                    //case2: has left child but no right child.
                    else if (node.RightNode == null)
                        parent.LeftNode = node.LeftNode;
                    //case3: has right child and right child has no left child of it's own.
                    else if (node.RightNode.LeftNode == null)
                        parent.LeftNode = node.RightNode;
                    //case4: Finally, if the deleted node's right child does have a left child, then the deleted node needs to be replaced by the deleted node's right child's left-most descendant. That is, we replace the deleted node with the deleted node's right subtree's smallest value.
                    else
                    {
                        ZBinaryTreeNode<T> minNode = FindMin(node.RightNode);
                        parent.LeftNode = minNode;
                    }
                }
                else if (parent.RightNode != null && parent.RightNode.Value.Equals(data))
                {
                    node = parent.RightNode;
                    //case1: has no children.
                    if (node.LeftNode == null && node.RightNode == null)
                        parent.RightNode = null;
                    //case2: has left child but no right child.
                    else if (node.RightNode == null)
                        parent.RightNode = node.LeftNode;
                    //case3: has right child and right child has no left child of it's own.
                    else if (node.RightNode.LeftNode == null)
                        parent.RightNode = node.RightNode;
                    //case4: Finally, if the deleted node's right child does have a left child, then the deleted node needs to be replaced by the deleted node's right child's left-most descendant. That is, we replace the deleted node with the deleted node's right subtree's smallest value.
                    else
                    {
                        ZBinaryTreeNode<T> minNode = FindMin(node.RightNode);
                        parent.RightNode = minNode;
                    }
                }
            }
        }

        public ZBinaryTreeNode<T> FindParent(T data)
        {
            ZBinaryTreeNode<T> current = Root;
            ZBinaryTreeNode<T> parent = null;

            if (Root.Value.Equals(data))
                return parent;

            while (current != null)
            {
                int compareResult = current.Value.CompareTo(data);

                if (compareResult == 0)
                    return parent;
                else if (compareResult < 0)
                {
                    parent = current;
                    current = current.LeftNode;
                }
                else
                {
                    parent = current;
                    current = current.RightNode;
                }
            }

            return parent;
        }

        public ZBinaryTreeNode<T> Find(T data)
        {
            ZBinaryTreeNode<T> node = null;
            ZBinaryTreeNode<T> current = Root;

            while (current != null)
            {
                int compareResult = current.Value.CompareTo(data);

                if (compareResult == 0)
                    return current;
                else if (compareResult < 0)
                    current = current.LeftNode;
                else
                    current = current.RightNode;
            }

            return node;
        }

        public ZBinaryTreeNode<T> FindMax(ZBinaryTreeNode<T> node)
        {
            if (node.RightNode == null)
                return node;
            else
                return (FindMax(node.RightNode));
        }

        public ZBinaryTreeNode<T> FindMin(ZBinaryTreeNode<T> node)
        {
            if (node.LeftNode == null)
                return node;
            else
                return (FindMax(node.LeftNode));
        }
    }
}
