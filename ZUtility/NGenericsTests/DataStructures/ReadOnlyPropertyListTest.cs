using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.ComponentModel;
using NGenerics.DataStructures;
using System.Collections;
using NGenerics.Visitors;

namespace NGenericsTests.DataStructures
{
    [TestFixture]
    public class ReadOnlyPropertyListTest
    {
        private PropertyDescriptor property = TypeDescriptor.GetProperties(typeof(SimpleClass)).Find("TestProperty", false);

        [Test]
        public void TestSuccesfulInit()
        {
            List<SimpleClass> simpleList = GetTestSimpleClassList();
            ReadOnlyPropertyList<SimpleClass, string> list = new ReadOnlyPropertyList<SimpleClass, string>(simpleList, property);

            Assert.AreEqual(list.Count, 20);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestInitNullList()
        {            
            ReadOnlyPropertyList<SimpleClass, string> list = new ReadOnlyPropertyList<SimpleClass, string>(null, property);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestInitNullProperty()
        {
            List<SimpleClass> simpleList = GetTestSimpleClassList();
            ReadOnlyPropertyList<SimpleClass, string> list = new ReadOnlyPropertyList<SimpleClass, string>(simpleList, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInitInvalidPropertyType()
        {
            List<SimpleClass> simpleList = GetTestSimpleClassList();
            ReadOnlyPropertyList<SimpleClass, string> list = new ReadOnlyPropertyList<SimpleClass, string>(simpleList, TypeDescriptor.GetProperties(typeof(SimpleClass)).Find("InvalidProperty", false));
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestInterfaceRemove()
        {
            List<SimpleClass> simpleList = GetTestSimpleClassList();
            ReadOnlyPropertyList<SimpleClass, string> list = new ReadOnlyPropertyList<SimpleClass, string>(simpleList, property);

            ((IList<string>)list).Remove("5");
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestInterfaceAdd()
        {
            List<SimpleClass> simpleList = GetTestSimpleClassList();
            ReadOnlyPropertyList<SimpleClass, string> list = new ReadOnlyPropertyList<SimpleClass, string>(simpleList, property);

            ((IList<string>)list).Add("5");
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestInterfaceInsert()
        {
            List<SimpleClass> simpleList = GetTestSimpleClassList();
            ReadOnlyPropertyList<SimpleClass, string> list = new ReadOnlyPropertyList<SimpleClass, string>(simpleList, property);

            ((IList<string>)list).Insert(2, "5");
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestInterfaceRemoveAt()
        {
            List<SimpleClass> simpleList = GetTestSimpleClassList();
            ReadOnlyPropertyList<SimpleClass, string> list = new ReadOnlyPropertyList<SimpleClass, string>(simpleList, property);

            ((IList<string>)list).RemoveAt(2);
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestInterfaceClear()
        {
            List<SimpleClass> simpleList = GetTestSimpleClassList();
            ReadOnlyPropertyList<SimpleClass, string> list = new ReadOnlyPropertyList<SimpleClass, string>(simpleList, property);

            ((IList<string>)list).Clear();
        }
        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestInterfaceSetter()
        {
            List<SimpleClass> simpleList = GetTestSimpleClassList();
            ReadOnlyPropertyList<SimpleClass, string> list = new ReadOnlyPropertyList<SimpleClass, string>(simpleList, property);

            ((IList<string>)list)[0] = "3";
        }

        [Test]
        public void TestCompareTo()
        {
            List<SimpleClass> simpleList1 = GetTestSimpleClassList();
            ReadOnlyPropertyList<SimpleClass, string> list1 = new ReadOnlyPropertyList<SimpleClass, string>(simpleList1, property);

            List<SimpleClass> simpleList2 = new List<SimpleClass>();
            ReadOnlyPropertyList<SimpleClass, string> list2 = new ReadOnlyPropertyList<SimpleClass, string>(simpleList2, property);

            Assert.AreEqual(list1.CompareTo(list2), 1);

            Assert.AreEqual(list2.CompareTo(list1), -1);

            Assert.AreEqual(list2.CompareTo(list2), 0);
            Assert.AreEqual(list1.CompareTo(list1), 0);

            Assert.AreEqual(list2.CompareTo(new object()), -1);
        }

        [Test]        
        public void TestInterfaceGetter()
        {
            List<SimpleClass> simpleList = GetTestSimpleClassList();
            ReadOnlyPropertyList<SimpleClass, string> list = new ReadOnlyPropertyList<SimpleClass, string>(simpleList, property);

            Assert.AreEqual(((IList<string>)list)[0], "0");
        }

        [Test]
        public void TestIsEmpty()
        {
            List<SimpleClass> simpleList = GetTestSimpleClassList();
            ReadOnlyPropertyList<SimpleClass, string> list = new ReadOnlyPropertyList<SimpleClass, string>(simpleList, property);

            Assert.AreEqual(list.IsEmpty, false);

            simpleList = new List<SimpleClass>();
            list = new ReadOnlyPropertyList<SimpleClass, string>(simpleList, property);

            Assert.AreEqual(list.IsEmpty, true);
        }

        [Test]
        public void TestIsFixedSize()
        {
            List<SimpleClass> simpleList = GetTestSimpleClassList();
            ReadOnlyPropertyList<SimpleClass, string> list = new ReadOnlyPropertyList<SimpleClass, string>(simpleList, property);

            Assert.AreEqual(list.IsFixedSize, true);

            simpleList = new List<SimpleClass>();
            list = new ReadOnlyPropertyList<SimpleClass, string>(simpleList, property);

            Assert.AreEqual(list.IsFixedSize, true);
        }

        [Test]
        public void TestIsFull()
        {
            List<SimpleClass> simpleList = GetTestSimpleClassList();
            ReadOnlyPropertyList<SimpleClass, string> list = new ReadOnlyPropertyList<SimpleClass, string>(simpleList, property);

            Assert.AreEqual(list.IsFull, true);

            simpleList = new List<SimpleClass>();
            list = new ReadOnlyPropertyList<SimpleClass, string>(simpleList, property);

            Assert.AreEqual(list.IsFull, true);
        }

        [Test]
        public void TestIsReadOnly()
        {
            List<SimpleClass> simpleList = GetTestSimpleClassList();
            ReadOnlyPropertyList<SimpleClass, string> list = new ReadOnlyPropertyList<SimpleClass, string>(simpleList, property);

            Assert.AreEqual(list.IsReadOnly, true);

            simpleList = new List<SimpleClass>();
            list = new ReadOnlyPropertyList<SimpleClass, string>(simpleList, property);

            Assert.AreEqual(list.IsReadOnly, true);
        }

        [Test]
        public void TestAccept()
        {
            List<SimpleClass> simpleList = GetTestSimpleClassList();
            ReadOnlyPropertyList<SimpleClass, string> list = new ReadOnlyPropertyList<SimpleClass, string>(simpleList, property);

            TrackingVisitor<string> v = new TrackingVisitor<string>();

            list.Accept(v);

            Assert.AreEqual(v.TrackingList.Count, 20);

            for (int i = 0; i < 20; i++)
            {
                Assert.AreEqual(v.TrackingList.Contains(i.ToString()), true);
            }
        }

        [Test]
        public void TestCopyTo()
        {
            List<SimpleClass> simpleList = GetTestSimpleClassList();
            ReadOnlyPropertyList<SimpleClass, string> list = new ReadOnlyPropertyList<SimpleClass, string>(simpleList, property);

            string[] array = new string[20];
            list.CopyTo(array, 0);

            List<string> l = new List<string>(array);

            for (int i = 0; i < list.Count; i++)
            {
                Assert.AreEqual(l[i], list[i]);
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidCopyTo1()
        {
            List<SimpleClass> simpleList = GetTestSimpleClassList();
            ReadOnlyPropertyList<SimpleClass, string> list = new ReadOnlyPropertyList<SimpleClass, string>(simpleList, property);

            string[] array = new string[19];
            list.CopyTo(array, 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidCopyTo2()
        {
            List<SimpleClass> simpleList = GetTestSimpleClassList();
            ReadOnlyPropertyList<SimpleClass, string> list = new ReadOnlyPropertyList<SimpleClass, string>(simpleList, property);

            string[] array = new string[20];
            list.CopyTo(array, 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullCopyTo()
        {
            List<SimpleClass> simpleList = GetTestSimpleClassList();
            ReadOnlyPropertyList<SimpleClass, string> list = new ReadOnlyPropertyList<SimpleClass, string>(simpleList, property);
            list.CopyTo(null, 0);
        }   

        [Test]
        public void TestIndexer()
        {
            List<SimpleClass> simpleList = GetTestSimpleClassList();
            ReadOnlyPropertyList<SimpleClass, string> list = new ReadOnlyPropertyList<SimpleClass, string>(simpleList, property);

            Assert.AreEqual(list.Count, 20);
                        
            for (int i = 0; i < simpleList.Count; i++)
            {
                Assert.AreEqual(list[i], (string) property.GetValue(simpleList[i]));
                
            }
        }

        [Test]
        public void TestContains()
        {
            List<SimpleClass> simpleList = GetTestSimpleClassList();
            ReadOnlyPropertyList<SimpleClass, string> list = new ReadOnlyPropertyList<SimpleClass, string>(simpleList, property);

            Assert.AreEqual(list.Count, 20);

            for (int i = 0; i < simpleList.Count; i++)
            {
                Assert.AreEqual(list[i].Contains((string)property.GetValue(simpleList[i])), true);
            }

            Assert.AreEqual(list.Contains("50"), false);
        }

        [Test]
        public void TestInterfaceEnumerator()
        {
            List<SimpleClass> simpleList = GetTestSimpleClassList();
            ReadOnlyPropertyList<SimpleClass, string> list = new ReadOnlyPropertyList<SimpleClass, string>(simpleList, property);

            List<string> strings = new List<string>();

            IEnumerator enumerator = ((IEnumerable)list).GetEnumerator();
            
            while (enumerator.MoveNext())
            {
                strings.Add((string) enumerator.Current);
            }

            Assert.AreEqual(strings.Count, 20);


            for (int i = 0; i < 20; i++)
            {
                Assert.AreEqual(strings.Contains(i.ToString()), true);
            }
        }

        [Test]
        public void TestEnumerator()
        {
            List<SimpleClass> simpleList = GetTestSimpleClassList();
            ReadOnlyPropertyList<SimpleClass, string> list = new ReadOnlyPropertyList<SimpleClass, string>(simpleList, property);

            List<string> strings = new List<string>();

            using (IEnumerator<string> enumerator = list.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    strings.Add(enumerator.Current);
                }
            }

            Assert.AreEqual(strings.Count, 20);
           

            for (int i = 0; i < 20; i++)
            {
                Assert.AreEqual(strings.Contains(i.ToString()), true);
            }
        }
                
        private List<SimpleClass> GetTestSimpleClassList()
        {
            List<SimpleClass> list = new List<SimpleClass>();

            for (int i = 0; i < 20; i++)
            {
                list.Add(new SimpleClass(i.ToString()));
            }

            return list;
        }
    }

    internal class SimpleClass
    {
        #region Globals

        private string testProperty;        

        #endregion

        #region Construction

        public SimpleClass(string value)
        {
            testProperty = value;
        }

        #endregion

        #region Public Members

        public string TestProperty
        {
            get
            {
                return testProperty;
            }
            set
            {
                testProperty = value;
            }
        }

        public int InvalidProperty
        {
            get
            {
                return 5;
            }
        }

        #endregion
    }
}
