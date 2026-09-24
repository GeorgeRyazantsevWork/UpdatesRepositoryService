using ServiceCloud.UpdatesRepository.BusinessLogic.Data;

namespace ServiceCloud.UpdatesRepository.BusinessLogic.Tests.Data
{
    [TestFixture]
    public class InfobaseConfigTests
    {
        private InfobaseConfig CreateTarget()
        {
            return InfobaseConfigTestContext.TestConfig;
        }

        [Test]
        public void InfobaseConfig_Equals_SameObjects_Test()
        {
            var target = CreateTarget();
            var operand = target;

            var answer = target.Equals(operand);

            Assert.That(answer, Is.True);
        }

        [Test, TestCaseSource(typeof(InfobaseConfigTestContext), nameof(InfobaseConfigTestContext.InequivalentOperands))]
        public void InfobaseConfig_Equals_InequivalentOperand_Test(InfobaseConfig operand)
        {
            var target = CreateTarget();

            var answer = target.Equals(operand);

            Assert.That(answer, Is.False);
        }

        [Test]
        public void InfobaseConfig_Equals_EquivalentOperand_Test()
        {
            var target = CreateTarget();
            var operand = CreateTarget();

            var answer = target.Equals(operand);

            Assert.That(answer, Is.True);
        }

        [Test]
        public void InfobaseConfig_Equals_NullArgument_Test()
        {
            var target = CreateTarget();

            var answer = target.Equals(null);

            Assert.That(answer, Is.False);
        }

        [Test]
        public void InfobaseConfig_EqualityOperator_NullLeftOperand_Test()
        {
            var target = CreateTarget();

            var answer = (null == target);

            Assert.That(answer, Is.False);
        }

        [Test]
        public void InfobaseConfig_EqualityOperator_NullRightOperand_Test()
        {
            var target = CreateTarget();

            var answer = (target == null);

            Assert.That(answer, Is.False);
        }

        [Test]
        public void InfobaseConfig_EqualityOperator_SameObjects_Test()
        {
            var left = CreateTarget();
            var right = left;

            Assert.IsTrue(left == right);
        }

        [Test]
        public void InfobaseConfig_EqualityOperator_EquivalentOperands_Test()
        {
            var left = CreateTarget();
            var right = CreateTarget();

            Assert.IsTrue(left == right);
        }

        [Test, TestCaseSource(typeof(InfobaseConfigTestContext), nameof(InfobaseConfigTestContext.InequivalentOperands))]
        public void InfobaseConfig_EqualityOperator_InequivalentOperands_Test(InfobaseConfig right)
        {
            var left = CreateTarget();

            var answer = (left == right);

            Assert.That(answer, Is.False);
        }

        [Test]
        public void InfobaseConfig_InequalityOperator_NullLeftOperand_Test()
        {
            var target = CreateTarget();

            var answer = (null != target);

            Assert.IsNotNull(answer);
        }

        [Test]
        public void InfobaseConfig_InequalityOperator_NullRightOperand_Test()
        {
            var target = CreateTarget();

            var answer = (target != null);

            Assert.That(answer, Is.True);
        }

        [Test]
        public void InfobaseConfig_InequalityOperator_SameObjects_Test()
        {
            var left = CreateTarget();
            var right = left;

            Assert.IsFalse(left != right);
        }

        [Test]
        public void InfobaseConfig_InequalityOperator_EquivalentOperands_Test()
        {
            var left = CreateTarget();
            var right = CreateTarget();

            Assert.IsFalse(left != right);
        }

        [Test, TestCaseSource(typeof(InfobaseConfigTestContext), nameof(InfobaseConfigTestContext.InequivalentOperands))]
        public void InfobaseConfig_InequalityOperator_InequivalentOperands_Test(InfobaseConfig right)
        {
            var left = CreateTarget();

            var answer = (left != right);

            Assert.That(answer, Is.True);
        }

        [Test]
        public void InfobaseConfig_Publisher_DefaultValue_Test()
        {
            var target = new InfobaseConfig();

            Assert.IsNull(target.Publisher);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("publisher")]
        public void InfobaseConfig_Publisher_SetValue_Test(string? value)
        {
            var target = CreateTarget();

            target.Publisher = value;

            Assert.AreEqual(value, target.Publisher);
        }

        [Test]
        public void InfobaseConfig_Name_DefaultValue_Test()
        {
            var target = new InfobaseConfig();

            Assert.IsNull(target.Name);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("name")]
        public void InfobaseConfig_Name_SetValue_Test(string? value)
        {
            var target = CreateTarget();

            target.Name = value;

            Assert.AreEqual(value, target.Name);
        }

        [Test]
        public void InfobaseConfig_Version_DefaultValue_Test()
        {
            var target = new InfobaseConfig();

            Assert.IsNull(target.VersionString);
        }

        [Test]
        public void InfobaseConfig_Version_SetNullValue_Test()
        {
            var target = CreateTarget();

            Assert.DoesNotThrow(() => target.VersionString = null);
        }

        [Test]
        public void InfobaseConfig_Version_SetValue_Test()
        {
            var target = CreateTarget();
            var expected = "1.2.3";

            target.VersionString = expected;

            Assert.AreEqual(expected, target.VersionString);
        }

        [Test]
        public void InfobaseConfig_Edition_DefaultValue_Test()
        {
            var target = CreateTarget();

            Assert.That(target.Edition, Is.EqualTo(InfoBaseConfigEdition.Unknown));
        }

        [Test]
        public void InfobaseConfig_Edition_SetValue_Test()
        {
            var target = CreateTarget();
            var expected = InfoBaseConfigEdition.CORP;

            target.Edition = expected;

            Assert.That(target.Edition, Is.EqualTo(expected));
        }

        [TestCaseSource(typeof(InfobaseConfigTestContext), nameof(InfobaseConfigTestContext.CompareCases))]
        public void InfobaseConfig_CompareTo_Test(InfobaseConfig target, InfobaseConfig config, int expected)
        {
            var actual = target.CompareTo(config);

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}