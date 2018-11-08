using FluentAssertions;
using OpenMacroBoard.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OpenMacroBoard.Tests
{
    public class KeyEventArgsTests
    {
        [Theory(DisplayName = "KeyEventArgs .ctor sets the properties")]
        [InlineData(1, false)]
        [InlineData(1, true)]
        [InlineData(34, false)]
        [InlineData(22, true)]
        [InlineData(-26, true)]
        [InlineData(-42, false)]
        public void KeyEventArgConstructorSetsProperties(int keyId, bool isDown)
        {
            var args = new KeyEventArgs(keyId, isDown);
            args.Key.Should().Be(keyId);
            args.IsDown.Should().Be(isDown);
        }
    }
}
