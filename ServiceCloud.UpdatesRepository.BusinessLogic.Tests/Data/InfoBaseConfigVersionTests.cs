using NUnit.Framework.Legacy;
using ServiceCloud.UpdatesRepository.BusinessLogic.Data;

namespace ServiceCloud.UpdatesRepository.BusinessLogic.Tests.Data
{
    [TestFixture]
    public class InfoBaseConfigVersionTests
    {
        [Test]
        public void InfoBaseConfigVersion_Constructor_NullInitValue_Test()
        {
            Assert.That(() => new InfoBaseConfigVersion(null),
                        Throws.Nothing);
        }

        [Test]
        public void InfoBaseConfigVersion_Constructor_EmptyInitValue_Test()
        {
            Assert.That(() => new InfoBaseConfigVersion(string.Empty),
                        Throws.Nothing);
        }

        [Test]
        public void InfoBaseConfigVersion_Equals_SameObjects_Test()
        {
            var target = new InfoBaseConfigVersion("1.0");
            var operand = target;

            var answer = target.Equals(operand);

            Assert.That(answer, Is.True);
        }

        [Test]
        public void InfoBaseConfigVersion_Equals_InequivalentOperand_Test()
        {
            var target = new InfoBaseConfigVersion("1.0");
            var operand = new InfoBaseConfigVersion("2.0");

            var answer = target.Equals(operand);

            Assert.That(answer, Is.False);
        }

        [Test]
        public void InfoBaseConfigVersion_Equals_EquivalentOperand_Test()
        {
            var target = new InfoBaseConfigVersion("1.0");
            var operand = new InfoBaseConfigVersion("1.0");

            var answer = target.Equals(operand);

            Assert.That(answer, Is.True);
        }

        [Test]
        public void InfoBaseConfigVersion_Equals_NullArgument_Test()
        {
            var target = new InfoBaseConfigVersion("1.0");

            var answer = target.Equals(null);

            Assert.That(answer, Is.False);
        }

        [Test]
        public void InfoBaseConfigVersion_EqualityOperator_NullLeftOperand_Test()
        {
            var target = new InfoBaseConfigVersion("1.0");

            var answer = (null == target);

            Assert.That(answer, Is.False);
        }

        [Test]
        public void InfoBaseConfigVersion_EqualityOperator_NullRightOperand_Test()
        {
            var target = new InfoBaseConfigVersion("1.0");

            var answer = (target == null);

            Assert.That(answer, Is.False);
        }

        [Test]
        public void InfoBaseConfigVersion_EqualityOperator_SameObjects_Test()
        {
            var left = new InfoBaseConfigVersion("1.0");
            var right = left;

            Assert.IsTrue(left == right);
        }

        [Test]
        public void InfoBaseConfigVersion_EqualityOperator_EquivalentOperands_Test()
        {
            var left = new InfoBaseConfigVersion("1.0");
            var right = new InfoBaseConfigVersion("1.0");

            Assert.IsTrue(left == right);
        }

        [Test]
        public void InfoBaseConfigVersion_EqualityOperator_InequivalentOperands_Test()
        {
            var left = new InfoBaseConfigVersion("1.0");
            var right = new InfoBaseConfigVersion("2.0");

            var answer = (left == right);

            Assert.That(answer, Is.False);
        }

        [Test]
        public void InfoBaseConfigVersion_InequalityOperator_NullLeftOperand_Test()
        {
            var target = new InfoBaseConfigVersion("1.0");

            var answer = (null != target);

            Assert.IsNotNull(answer);
        }

        [Test]
        public void InfoBaseConfigVersion_InequalityOperator_NullRightOperand_Test()
        {
            var target = new InfoBaseConfigVersion("1.0");

            var answer = (target != null);

            Assert.That(answer, Is.True);
        }

        [Test]
        public void InfoBaseConfigVersion_InequalityOperator_SameObjects_Test()
        {
            var left = new InfoBaseConfigVersion("1.0");
            var right = left;

            Assert.IsFalse(left != right);
        }

        [Test]
        public void InfoBaseConfigVersion_InequalityOperator_EquivalentOperands_Test()
        {
            var left = new InfoBaseConfigVersion("1.0");
            var right = new InfoBaseConfigVersion("1.0");

            Assert.IsFalse(left != right);
        }

        [Test]
        public void InfoBaseConfigVersion_InequalityOperator_InequivalentOperands_Test()
        {
            var left = new InfoBaseConfigVersion("1.0");
            var right = new InfoBaseConfigVersion("2.0");

            var answer = (left != right);

            Assert.That(answer, Is.True);
        }

        [TestCase("1.0", "2.0", -1)]
        [TestCase("1.0", "1.0", 0)]
        [TestCase("2.0", "1.0", 1)]
        [TestCase("1.0/1.0", "1.0/2.0", -1)]
        [TestCase("1.0/1.1", "1.0/1.1", 0)]
        [TestCase("2.0/1.0", "1.0/2.0", 1)]
        public void InfoBaseConfigVersion_CompareTo_Test(string targetValue, string otherValue, int expectedResult)
        {
            var target = new InfoBaseConfigVersion(targetValue);
            var other = new InfoBaseConfigVersion(otherValue);

            var actual = target.CompareTo(other);

            Assert.That(actual, Is.EqualTo(expectedResult));
        }

        InfoBaseConfigVersion[] unsortedCollection = new InfoBaseConfigVersion[]
        {
                new InfoBaseConfigVersion("1"),
                new InfoBaseConfigVersion("2"),
                new InfoBaseConfigVersion("3"),
                new InfoBaseConfigVersion("1a"),
                new InfoBaseConfigVersion("1b"),
                new InfoBaseConfigVersion("1c"),
                new InfoBaseConfigVersion("1.0"),
                new InfoBaseConfigVersion("2.0"),
                new InfoBaseConfigVersion("3.0"),
                new InfoBaseConfigVersion("1.13"),
                new InfoBaseConfigVersion("1.1"),
                new InfoBaseConfigVersion("2.13"),
                new InfoBaseConfigVersion("2.1"),
                new InfoBaseConfigVersion("3.13"),
                new InfoBaseConfigVersion("3.1"),
                new InfoBaseConfigVersion("2.2"),
                new InfoBaseConfigVersion("3.3"),
                new InfoBaseConfigVersion("1.0.1"),
                new InfoBaseConfigVersion("1.1.1"),
                new InfoBaseConfigVersion("2.0.0"),
                new InfoBaseConfigVersion("2.1.1"),
                new InfoBaseConfigVersion("3.1.1"),
                new InfoBaseConfigVersion("2.1.2"),
                new InfoBaseConfigVersion("3.0.0"),
                new InfoBaseConfigVersion("3.1.3"),
                new InfoBaseConfigVersion("1.0.13"),
                new InfoBaseConfigVersion("2.1.13"),
                new InfoBaseConfigVersion("3.1.13"),
                new InfoBaseConfigVersion("1.0.0.0"),
                new InfoBaseConfigVersion("1.0.0.1"),
                new InfoBaseConfigVersion("1.0.1.1"),
                new InfoBaseConfigVersion("1.1.1.1"),
                new InfoBaseConfigVersion("2.0.0.0"),
                new InfoBaseConfigVersion("2.1.1.1"),
                new InfoBaseConfigVersion("3.0.0.0"),
                new InfoBaseConfigVersion("3.1.1.1"),
                new InfoBaseConfigVersion("2.1.1.2"),
                new InfoBaseConfigVersion("3.1.1.3"),
                new InfoBaseConfigVersion("1.1.1.13"),
                new InfoBaseConfigVersion("2.1.1.13"),
                new InfoBaseConfigVersion("3.1.1.13"),
                new InfoBaseConfigVersion("1.0.0.0a"),
                new InfoBaseConfigVersion("1.0.0.0b"),
                new InfoBaseConfigVersion("1.0.0.0c"),
                new InfoBaseConfigVersion("1.0.2/2.0.0.1"),
                new InfoBaseConfigVersion("1.0.1.1/2.0.1"),
                new InfoBaseConfigVersion("1.0.1.1/2.0.0.1"),
                new InfoBaseConfigVersion("1a.0.0.0/a1.0.0.0"),
                new InfoBaseConfigVersion("1a.0.0.0/1a.0.0.0"),
                new InfoBaseConfigVersion("a"),
                new InfoBaseConfigVersion("aa"),
                new InfoBaseConfigVersion("aa11"),
                new InfoBaseConfigVersion("aa2"),
                new InfoBaseConfigVersion("aaa"),
        };

        InfoBaseConfigVersion[] sortedCollection = new InfoBaseConfigVersion[]
        {
                new InfoBaseConfigVersion("1"),
                new InfoBaseConfigVersion("1.0"),
                new InfoBaseConfigVersion("1.0.0.0"),
                new InfoBaseConfigVersion("1.0.0.0a"),
                new InfoBaseConfigVersion("1.0.0.0b"),
                new InfoBaseConfigVersion("1.0.0.0c"),
                new InfoBaseConfigVersion("1.0.0.1"),
                new InfoBaseConfigVersion("1.0.1"),
                new InfoBaseConfigVersion("1.0.1.1"),
                new InfoBaseConfigVersion("1.0.1.1/2.0.0.1"),
                new InfoBaseConfigVersion("1.0.1.1/2.0.1"),
                new InfoBaseConfigVersion("1.0.2/2.0.0.1"),
                new InfoBaseConfigVersion("1.0.13"),
                new InfoBaseConfigVersion("1.1"),
                new InfoBaseConfigVersion("1.1.1"),
                new InfoBaseConfigVersion("1.1.1.1"),
                new InfoBaseConfigVersion("1.1.1.13"),
                new InfoBaseConfigVersion("1.13"),
                new InfoBaseConfigVersion("1a"),
                new InfoBaseConfigVersion("1a.0.0.0/1a.0.0.0"),
                new InfoBaseConfigVersion("1a.0.0.0/a1.0.0.0"),
                new InfoBaseConfigVersion("1b"),
                new InfoBaseConfigVersion("1c"),
                new InfoBaseConfigVersion("2"),
                new InfoBaseConfigVersion("2.0"),
                new InfoBaseConfigVersion("2.0.0"),
                new InfoBaseConfigVersion("2.0.0.0"),
                new InfoBaseConfigVersion("2.1"),
                new InfoBaseConfigVersion("2.1.1"),
                new InfoBaseConfigVersion("2.1.1.1"),
                new InfoBaseConfigVersion("2.1.1.2"),
                new InfoBaseConfigVersion("2.1.1.13"),
                new InfoBaseConfigVersion("2.1.2"),
                new InfoBaseConfigVersion("2.1.13"),
                new InfoBaseConfigVersion("2.2"),
                new InfoBaseConfigVersion("2.13"),
                new InfoBaseConfigVersion("3"),
                new InfoBaseConfigVersion("3.0"),
                new InfoBaseConfigVersion("3.0.0"),
                new InfoBaseConfigVersion("3.0.0.0"),
                new InfoBaseConfigVersion("3.1"),
                new InfoBaseConfigVersion("3.1.1"),
                new InfoBaseConfigVersion("3.1.1.1"),
                new InfoBaseConfigVersion("3.1.1.3"),
                new InfoBaseConfigVersion("3.1.1.13"),
                new InfoBaseConfigVersion("3.1.3"),
                new InfoBaseConfigVersion("3.1.13"),
                new InfoBaseConfigVersion("3.3"),
                new InfoBaseConfigVersion("3.13"),
                new InfoBaseConfigVersion("a"),
                new InfoBaseConfigVersion("aa"),
                new InfoBaseConfigVersion("aa2"),
                new InfoBaseConfigVersion("aa11"),
                new InfoBaseConfigVersion("aaa"),
        };

        [Test]
        public void InfoBaseConfigVersion_Sorting_Test()
        {
            var collection = unsortedCollection.OrderBy(x => x).ToArray();
            CollectionAssert.AreEqual(sortedCollection, collection);
        }
    }
}
