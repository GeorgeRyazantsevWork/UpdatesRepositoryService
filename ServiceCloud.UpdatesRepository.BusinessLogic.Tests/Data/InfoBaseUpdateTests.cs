using NUnit.Framework.Legacy;
using ServiceCloud.UpdatesRepository.BusinessLogic.Data;
using ServiceCloud.UpdatesRepository.BusinessLogic.Data.Tests;

namespace ServiceCloud.UpdatesRepository.BusinessLogic.Tests.Data
{
    [TestFixture]
    public class InfoBaseUpdateTests
    {
        private InfoBaseUpdate CreateTestObject()
        {
            return new InfoBaseUpdate()
            {
                DestinationConfig = InfoBaseUpdateTestContext.DestinationConfig,
                SourseConfigs = InfoBaseUpdateTestContext.SourseConfigs,
                IsTransitive = false
            };
        }

        [Test]
        public void InfoBaseUpdate_Equals_SameObject_Test()
        {
            var target = CreateTestObject();
            var operand = target;

            var answer = target.Equals(operand);

            Assert.That(answer, Is.True);
        }

        [Test]
        public void InfoBaseUpdate_Equals_EquivalentOperand_Test()
        {
            var target = CreateTestObject();
            var operand = CreateTestObject();

            var answer = target.Equals(operand);

            Assert.That(answer, Is.True);
        }

        [Test]
        [TestCaseSource(typeof(InfoBaseUpdateTestContext), nameof(InfoBaseUpdateTestContext.InequivalentOperand))]
        public void InfoBaseUpdate_Equals_InequivalentOperand_Test(InfoBaseUpdate operand)
        {
            var target = CreateTestObject();

            var answer = target.Equals(operand);

            Assert.That(answer, Is.False);
        }

        [Test]
        public void InfoBaseUpdate_Equals_NullOperand_Test()
        {
            var target = CreateTestObject();

            var answer = target.Equals(null);

            Assert.That(answer, Is.False);
        }

        [Test]
        public void InfoBaseUpdate_EqualityOperator_SameObjects_Test()
        {
            var target = CreateTestObject();
            var operand = target;

            var answer = (target == operand);

            Assert.That(answer, Is.True);
        }

        [Test]
        public void InfoBaseUpdate_EqualityOperator_EquivalentOperands_Test()
        {
            var target = CreateTestObject();
            var operand = CreateTestObject();

            var answer = (target == operand);

            Assert.That(answer, Is.True);
        }

        [Test]
        [TestCaseSource(typeof(InfoBaseUpdateTestContext), nameof(InfoBaseUpdateTestContext.InequivalentOperand))]
        public void InfoBaseUpdate_EqualityOperator_InequivalentOperands_Test(InfoBaseUpdate operand)
        {
            var target = CreateTestObject();

            var answer = (target == operand);

            Assert.That(answer, Is.False);
        }

        [Test]
        public void InfoBaseUpdate_EqualityOperator_NullLeftOperand_Test()
        {
            var target = CreateTestObject();

            var answer = (null == target);

            Assert.That(answer, Is.False);
        }

        [Test]
        public void InfoBaseUpdate_EqualityOperator_NullRightOperand_Test()
        {
            var target = CreateTestObject();

            var answer = (target == null);

            Assert.That(answer, Is.False);
        }

        [Test]
        public void InfoBaseUpdate_InequalityOperator_SameObjects_Test()
        {
            var target = CreateTestObject();
            var operand = target;

            var answer = (target != operand);

            Assert.That(answer, Is.False);
        }

        [Test]
        public void InfoBaseUpdate_InequalityOperator_EquivalentOperands_Test()
        {
            var target = CreateTestObject();
            var operand = CreateTestObject();

            var answer = (target != operand);

            Assert.That(answer, Is.False);
        }

        [Test]
        [TestCaseSource(typeof(InfoBaseUpdateTestContext), nameof(InfoBaseUpdateTestContext.InequivalentOperand))]
        public void InfoBaseUpdate_InequalityOperator_InequivalentOperands_Test(InfoBaseUpdate operand)
        {
            var target = CreateTestObject();

            var answer = (target != operand);

            Assert.That(answer, Is.True);
        }

        [Test]
        public void InfoBaseUpdate_InequalityOperator_NullLeftOperand_Test()
        {
            var target = CreateTestObject();

            var answer = (null != target);

            Assert.That(answer, Is.True);
        }

        [Test]
        public void InfoBaseUpdate_InequalityOperator_NullRightOperand_Test()
        {
            var target = CreateTestObject();

            var answer = (target != null);

            Assert.That(answer, Is.True);
        }

        [Test]
        public void InfoBaseUpdate_DestinationConfig_DefaultValue_Test()
        {
            var target = new InfoBaseUpdate();

            Assert.IsNotNull(target.DestinationConfig);
        }

        [Test]
        public void InfoBaseUpdate_DestinationConfig_SetNullValue_Test()
        {
            var target = CreateTestObject();

            Assert.Throws<ArgumentNullException>(
                () => target.DestinationConfig = null!
            );
        }

        [Test]
        public void InfoBaseUpdate_DestinationConfig_SetValue_Test()
        {
            var target = CreateTestObject();
            var expected = new InfobaseConfig();
            target.DestinationConfig = expected;

            Assert.AreEqual(expected, target.DestinationConfig);
        }

        [Test]
        public void InfoBaseUpdate_SourseConfigs_DefaultValue_Test()
        {
            var target = new InfoBaseUpdate();

            Assert.IsNotNull(target.SourseConfigs);
            CollectionAssert.IsEmpty(target.SourseConfigs);
        }

        [Test]
        public void InfoBaseUpdate_SourseConfigs_NullValue_Test()
        {
            var target = CreateTestObject();

            Assert.Throws<ArgumentNullException>(
                () => target.SourseConfigs = null!
            );
        }

        [Test]
        public void InfoBaseUpdate_SourseConfigs_SetValue_Test()
        {
            var target = CreateTestObject();

            var expected = new InfobaseConfig[0];
            target.SourseConfigs = expected;
            Assert.AreEqual(expected, target.SourseConfigs);
        }

        [Test]
        public void InfoBaseUpdate_Transitive_DefaultValue_Test()
        {
            var target = CreateTestObject();

            Assert.That(target.IsTransitive, Is.False);
        }

        [Test]
        public void InfoBaseUpdate_Transitive_SetValue_Test()
        {
            var target = CreateTestObject();
            var expected = true;

            target.IsTransitive = expected;

            Assert.That(target.IsTransitive, Is.EqualTo(expected));
        }

        [Test]
        public void InfoBaseUpdate_Id_DefaultValue_Test()
        {
            var target = CreateTestObject();

            Assert.That(target.Id, Is.Null);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("$?!")]
        public void InfoBaseUpdate_Id_SetValue_Test(string? expected)
        {
            var target = CreateTestObject();

            target.Id = expected;

            Assert.That(target.Id, Is.EqualTo(expected));
        }
    }
}

