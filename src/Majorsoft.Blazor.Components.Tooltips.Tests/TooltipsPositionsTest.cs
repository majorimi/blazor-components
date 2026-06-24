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
            var values = System.Enum.GetValues(typeof(TooltipPositions));

            // Assert
            Assert.IsNotNull(values);
            Assert.AreEqual(4, values.Length);
            Assert.AreEqual(TooltipPositions.Top, values.GetValue(0));
            Assert.AreEqual(TooltipPositions.Right, values.GetValue(1));
            Assert.AreEqual(TooltipPositions.Bottom, values.GetValue(2));
            Assert.AreEqual(TooltipPositions.Left, values.GetValue(3));
        }

        [TestMethod]
        public void TooltipsPositions_Top_should_have_correct_value()
        {
            // Arrange & Act
            var position = TooltipPositions.Top;

            // Assert
            Assert.AreEqual(TooltipPositions.Top, position);
            Assert.AreNotEqual(TooltipPositions.Bottom, position);
            Assert.AreNotEqual(TooltipPositions.Left, position);
            Assert.AreNotEqual(TooltipPositions.Right, position);
        }

        [TestMethod]
        public void TooltipsPositions_Right_should_have_correct_value()
        {
            // Arrange & Act
            var position = TooltipPositions.Right;

            // Assert
            Assert.AreEqual(TooltipPositions.Right, position);
            Assert.AreNotEqual(TooltipPositions.Top, position);
            Assert.AreNotEqual(TooltipPositions.Bottom, position);
            Assert.AreNotEqual(TooltipPositions.Left, position);
        }

        [TestMethod]
        public void TooltipsPositions_Bottom_should_have_correct_value()
        {
            // Arrange & Act
            var position = TooltipPositions.Bottom;

            // Assert
            Assert.AreEqual(TooltipPositions.Bottom, position);
            Assert.AreNotEqual(TooltipPositions.Top, position);
            Assert.AreNotEqual(TooltipPositions.Left, position);
            Assert.AreNotEqual(TooltipPositions.Right, position);
        }

        [TestMethod]
        public void TooltipsPositions_Left_should_have_correct_value()
        {
            // Arrange & Act
            var position = TooltipPositions.Left;

            // Assert
            Assert.AreEqual(TooltipPositions.Left, position);
            Assert.AreNotEqual(TooltipPositions.Top, position);
            Assert.AreNotEqual(TooltipPositions.Right, position);
            Assert.AreNotEqual(TooltipPositions.Bottom, position);
        }

        [TestMethod]
        public void TooltipsPositions_enum_should_be_comparable()
        {
            // Arrange
            var positions = new[] { TooltipPositions.Top, TooltipPositions.Right, TooltipPositions.Bottom, TooltipPositions.Left };

            // Act & Assert
            Assert.IsTrue(positions[0] < positions[1]);
            Assert.IsTrue(positions[1] < positions[2]);
            Assert.IsTrue(positions[2] < positions[3]);
        }

        [TestMethod]
        public void TooltipsPositions_ToString_should_return_position_name()
        {
            // Arrange
            var topPosition = TooltipPositions.Top;
            var rightPosition = TooltipPositions.Right;
            var bottomPosition = TooltipPositions.Bottom;
            var leftPosition = TooltipPositions.Left;

            // Act & Assert
            Assert.AreEqual("Top", topPosition.ToString());
            Assert.AreEqual("Right", rightPosition.ToString());
            Assert.AreEqual("Bottom", bottomPosition.ToString());
            Assert.AreEqual("Left", leftPosition.ToString());
        }
    }
}
