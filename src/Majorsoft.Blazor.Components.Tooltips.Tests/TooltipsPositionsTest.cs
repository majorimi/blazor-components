using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.Tooltips.Tests
{
    [TestClass]
    public class TooltipsPositionsTest
    {
        [TestMethod]
        public void TooltipsPositions_enum_should_have_all_expected_values()
        {
            // Arrange & Act
            var values = System.Enum.GetValues(typeof(TooltipsPositons));

            // Assert
            Assert.IsNotNull(values);
            Assert.AreEqual(4, values.Length);
            Assert.AreEqual(TooltipsPositons.Top, values.GetValue(0));
            Assert.AreEqual(TooltipsPositons.Right, values.GetValue(1));
            Assert.AreEqual(TooltipsPositons.Bottom, values.GetValue(2));
            Assert.AreEqual(TooltipsPositons.Left, values.GetValue(3));
        }

        [TestMethod]
        public void TooltipsPositions_Top_should_have_correct_value()
        {
            // Arrange & Act
            var position = TooltipsPositons.Top;

            // Assert
            Assert.AreEqual(TooltipsPositons.Top, position);
            Assert.AreNotEqual(TooltipsPositons.Bottom, position);
            Assert.AreNotEqual(TooltipsPositons.Left, position);
            Assert.AreNotEqual(TooltipsPositons.Right, position);
        }

        [TestMethod]
        public void TooltipsPositions_Right_should_have_correct_value()
        {
            // Arrange & Act
            var position = TooltipsPositons.Right;

            // Assert
            Assert.AreEqual(TooltipsPositons.Right, position);
            Assert.AreNotEqual(TooltipsPositons.Top, position);
            Assert.AreNotEqual(TooltipsPositons.Bottom, position);
            Assert.AreNotEqual(TooltipsPositons.Left, position);
        }

        [TestMethod]
        public void TooltipsPositions_Bottom_should_have_correct_value()
        {
            // Arrange & Act
            var position = TooltipsPositons.Bottom;

            // Assert
            Assert.AreEqual(TooltipsPositons.Bottom, position);
            Assert.AreNotEqual(TooltipsPositons.Top, position);
            Assert.AreNotEqual(TooltipsPositons.Left, position);
            Assert.AreNotEqual(TooltipsPositons.Right, position);
        }

        [TestMethod]
        public void TooltipsPositions_Left_should_have_correct_value()
        {
            // Arrange & Act
            var position = TooltipsPositons.Left;

            // Assert
            Assert.AreEqual(TooltipsPositons.Left, position);
            Assert.AreNotEqual(TooltipsPositons.Top, position);
            Assert.AreNotEqual(TooltipsPositons.Right, position);
            Assert.AreNotEqual(TooltipsPositons.Bottom, position);
        }

        [TestMethod]
        public void TooltipsPositions_enum_should_be_comparable()
        {
            // Arrange
            var positions = new[] { TooltipsPositons.Top, TooltipsPositons.Right, TooltipsPositons.Bottom, TooltipsPositons.Left };

            // Act & Assert
            Assert.IsTrue(positions[0] < positions[1]);
            Assert.IsTrue(positions[1] < positions[2]);
            Assert.IsTrue(positions[2] < positions[3]);
        }

        [TestMethod]
        public void TooltipsPositions_ToString_should_return_position_name()
        {
            // Arrange
            var topPosition = TooltipsPositons.Top;
            var rightPosition = TooltipsPositons.Right;
            var bottomPosition = TooltipsPositons.Bottom;
            var leftPosition = TooltipsPositons.Left;

            // Act & Assert
            Assert.AreEqual("Top", topPosition.ToString());
            Assert.AreEqual("Right", rightPosition.ToString());
            Assert.AreEqual("Bottom", bottomPosition.ToString());
            Assert.AreEqual("Left", leftPosition.ToString());
        }
    }
}
