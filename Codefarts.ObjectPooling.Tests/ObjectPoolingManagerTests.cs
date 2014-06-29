// <copyright>
//   Copyright (c) 2012 Codefarts
//   All rights reserved.
//   contact@codefarts.com
//   http://www.codefarts.com
// </copyright>

namespace Codefarts.Tests.ObjectPooling
{
    using System;

    using Codefarts.ObjectPooling;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ObjectPoolingManagerTests
    {
        ObjectPoolManager<TestObject> manager;

        [TestInitialize]
        public void Setup()
        {
            this.manager = new ObjectPoolManager<TestObject>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.manager = null;
        }

        public class TestObject
        {
            public string stringValue;
            public int intValue;
        }

        [TestMethod]
        public void Pop_With_Empty_Pool_NoCallback()
        {
            try
            {
                var item = this.manager.Pop();
                Assert.IsNotNull(item);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is NullReferenceException);
            }
        }

        [TestMethod]
        public void Pop_With_Empty_Pool_WithCallback()
        {
            try
            {
                this.manager.CreationCallback = this.Callback;
                var item = this.manager.Pop();
                Assert.IsNotNull(item);
                Assert.AreEqual(0, item.intValue);
                Assert.AreEqual(null, item.stringValue);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is NullReferenceException);
            }
        }

        private TestObject Callback()
        {
            return new TestObject();
        }

        [TestMethod]
        public void Push_Object()
        {
            try
            {
                Assert.AreEqual(0, this.manager.Count);
                this.manager.Push(new TestObject());
                Assert.AreEqual(1, this.manager.Count);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }

        [TestMethod]
        public void Push_Pop_Objects()
        {
            try
            {
                Assert.AreEqual(0, this.manager.Count);
                for (var i = 0; i < 3; i++)
                {
                    this.manager.Push(new TestObject() { stringValue = "Item" + i, intValue = i });
                }

                Assert.AreEqual(3, this.manager.Count);

                for (var i = 3 - 1; i >= 0; i--)
                {
                    var item = this.manager.Pop();
                    Assert.AreEqual(i, item.intValue);
                    Assert.AreEqual("Item" + i, item.stringValue);
                }

                Assert.AreEqual(0, this.manager.Count);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }
    }
}
