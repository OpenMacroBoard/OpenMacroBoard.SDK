using FluentAssertions;
using OpenMacroBoard.SDK;
using System;
using System.Collections.Generic;
using Xunit;

namespace OpenMacroBoard.Tests
{
    public class KeyBitmapTests
    {
        [Fact]
        public void NegativeOrZeroWidthCausesConstructorToThrow()
        {
            Action act_zero = () => _ = KeyBitmap.Create.FromBgr24Array(0, 1, null);
            Action act_negative = () => _ = KeyBitmap.Create.FromBgr24Array(-1, 1, null);

            act_zero.Should()
                .Throw<ArgumentOutOfRangeException>()
                .And.ParamName.Should().Be("width");

            act_negative.Should()
                .Throw<ArgumentOutOfRangeException>()
                .And.ParamName.Should().Be("width");
        }

        [Fact]
        public void NegativeOrZeroHeightCausesConstructorToThrow()
        {
            Action act_zero = () => _ = KeyBitmap.Create.FromBgr24Array(1, 0, null);
            Action act_negative = () => _ = KeyBitmap.Create.FromBgr24Array(1, -1, null);

            act_zero.Should()
                .Throw<ArgumentOutOfRangeException>()
                .And.ParamName.Should().Be("height");

            act_negative.Should()
                .Throw<ArgumentOutOfRangeException>()
                .And.ParamName.Should().Be("height");
        }

        [Fact]
        public void ImageDataArrayLengthMismatchThrowsException()
        {
            Action act_correctParams = () => _ = KeyBitmap.Create.FromBgr24Array(2, 2, new byte[12]);
            Action act_incorrectParams = () => _ = KeyBitmap.Create.FromBgr24Array(3, 3, new byte[5]);

            act_correctParams.Should().NotThrow();
            act_incorrectParams.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void EqualsReturnsFalseIfWidthIsDifferent()
        {
            var key1 = KeyBitmap.Create.Empty(4, 4);
            var key2 = KeyBitmap.Create.Empty(3, 4);
            var key3 = KeyBitmap.Create.Empty(4, 4);

            key1.Should().NotBe(key2);
            key1.Should().NotBeSameAs(key2);

            key1.Should().Be(key3);
            key1.Should().NotBeSameAs(key3);

            KeyBitmap.Equals(key1, key3).Should().BeTrue();
        }

        [Fact]
        public void EqualsReturnsFalseIfHeightIsDifferent()
        {
            var key1 = KeyBitmap.Create.Empty(4, 4);
            var key2 = KeyBitmap.Create.Empty(4, 3);
            var key3 = KeyBitmap.Create.Empty(4, 4);

            key1.Should().NotBe(key2);
            key1.Should().NotBeSameAs(key2);

            key1.Should().Be(key3);
            key1.Should().NotBeSameAs(key3);

            KeyBitmap.Equals(key1, key3).Should().BeTrue();
        }

        [Fact]
        public void EqualsReturnsFalseIfOnlyOneElementIsNull()
        {
            var key = KeyBitmap.Create.Empty(4, 4);

            KeyBitmap.Equals(null, null).Should().BeTrue();
            KeyBitmap.Equals(null, key).Should().BeFalse();
            KeyBitmap.Equals(key, null).Should().BeFalse();
        }


        [Fact]
        public void EqualsReturnsFalseIfDataDoesNotMatch()
        {
            var key1 = KeyBitmap.Create.Empty(1, 1);
            var key2 = KeyBitmap.Create.FromBgr24Array(1, 1, new byte[3]);

            key1.Should().NotBe(key2);
            key2.Should().NotBe(key1);
        }

        [Fact]
        public void KeyBitmapsWithDifferentBgrValuesAreNotEqual()
        {
            var key1 = KeyBitmap.Create.FromBgr24Array(1, 1, new byte[3]);

            var key2Data = new byte[3] { 1, 2, 3 };
            var key2 = KeyBitmap.Create.FromBgr24Array(1, 1, key2Data);

            key1.Should().NotBe(key2);
            key2.Should().NotBe(key1);
        }

        [Fact(DisplayName = "All equality methods behave the same way.")]
        public void AllEqualityMethodsBehaveTheSameWay()
        {
            var key2Data = new byte[3] { 1, 2, 3 };

            var key1 = KeyBitmap.Create.FromBgr24Array(1, 1, new byte[3]);
            var key2 = KeyBitmap.Create.FromBgr24Array(1, 1, key2Data);
            var key3 = KeyBitmap.Create.FromBgr24Array(1, 1, new byte[3]);

            var equalityMethods = new List<Func<KeyBitmap, KeyBitmap, bool>>()
            {
                KeyBitmap.Equals,
                (a,b) => a == b,
                #pragma warning disable S1940 // Boolean checks should not be inverted
                (a,b) => !(a != b),
                #pragma warning restore S1940
                (a,b) => a.Equals(b),
            };

            foreach (var eq in equalityMethods)
            {
                eq(key1, key3).Should().BeTrue();
                eq(key3, key1).Should().BeTrue();
                eq(key1, key2).Should().BeFalse();
                eq(key2, key1).Should().BeFalse();
            }
        }

        [Fact]
        public void HashCodesDonNotMatchEasilyForDifferentObjects()
        {
            var key2Data = new byte[3] { 1, 2, 3 };

            var key1 = KeyBitmap.Create.FromBgr24Array(1, 1, new byte[3]);
            var key2 = KeyBitmap.Create.FromBgr24Array(1, 1, key2Data);
            var key3 = KeyBitmap.Create.Empty(1, 1);
            var key4 = KeyBitmap.Create.FromBgr24Array(100, 100, new byte[100 * 100 * 3]);

            var hash1 = key1.GetHashCode();
            var hash2 = key2.GetHashCode();
            var hash3 = key3.GetHashCode();
            var hash4 = key4.GetHashCode();

            hash1.Should().NotBe(hash2);
            hash1.Should().NotBe(hash3);
            hash1.Should().NotBe(hash4);
        }
    }
}


