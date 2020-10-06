/*The character copier is a simple class that reads characters from a source and copies them
to a destination one character at a time.
When the method Copy is called on the copier then it should read characters from the source
and copy them to the destination until the source returns a newline (\n).
The exercise is to implement the character copier using Test Doubles for the source and the
destination (try using Spies – manually written Mocks – and Mocks written with a mocking
framework).*/
/* Stubs used mainly for queries to respond to calls with pre-programmed happy noises
 * Mocks used to respond to calls to confirm commands have been triggered correctly under SUT*/

using System;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace TestDoubles
{
    public class CopierTests
    {
        private Mock<IDestination> destination;
        private SourceStub source;
        private Copier copier;
        public void SetUp(string str)
        {
            this.source = new SourceStub(str);
            this.destination = new Mock<IDestination>();
            this.copier= new Copier(this.source, this.destination.Object);
        }

        [TestCase("\n")]
        public void GivenNewLine__WhenCopierIsCalled__ThenDoNotCopy(string str)
        {
            this.SetUp(str);
            this.copier.Copy();
            this.destination.Verify(s =>s.SetChar(It.IsAny<char>()),Times.Never);
        }

        [TestCase("A\n", 1)]
        [TestCase("AB\n", 2)]
        [TestCase("A B\n", 3)]
        public void GivenAStringWith1CharacterAndNewline__WhenCopierIsCalled__ThenCopy(string str, int times)
        {
            this.SetUp(str);
            this.copier.Copy();
            this.destination.Verify(s => s.SetChar(It.IsAny<char>()), Times.Exactly(times));
        }

        public class SourceStub : ISource
        {
            private string str;
            private int ctr =0;

            public SourceStub(string inputStr)
            {
                str = inputStr;
            }

            public char GetChar()
            {
                return str[ctr++];
            }
        }
    }
    /*implement this using Test Doubles for source and destination*/
    public class Copier
    {
        private IDestination desti;
        private ISource source;

        public Copier(ISource source, IDestination destination)
        { 
            this.source = source;
            this.desti = destination;
        }

        public void Copy()
        {
            char character = '\0';

            while ( character != '\n')
            {
                character = source.GetChar();
                if (character != '\n')
                {
                    desti.SetChar(character);
                }
            }
        }
        }
    }

    public interface ISource
    {
        char GetChar();
    }

    public interface IDestination
    {
        void SetChar(char character);
    }
