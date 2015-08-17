using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    public static class AssertException
    {
        /// <summary>
        /// Executes the input delegate and checks it throws a exception of type TException
        /// </summary>
        /// <typeparam name="TException">The expected type of exception</typeparam>
        /// <param name="action">The action to execute that should generate the exception</param>
        public static void Throws<TException>(Action action) where TException : Exception
        {
            try
            {
                action();
            }
            catch(Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(TException), "Expected exception of type " + typeof(TException) + " but type of " + ex.GetType() + " was thrown instead.");
                return;
            }
            Assert.Fail("Expected exception of type " + typeof(TException) + " but no exception was thrown.");
        }

        /// <summary>
        /// Executes the input delegate and checks it throws a exception of type TException with
        /// a message equal to expectedMessage
        /// </summary>
        /// <typeparam name="TException">The expected type of exception</typeparam>
        /// <param name="action">The action to execute that should generate the exception</param>
        /// <param name="expectedMessage">The expected exception message</param>
        public static void Throws<TException>(Action action, string expectedMessage) where TException : Exception
        {
            try
            {
                action();
            }
            catch(Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(TException), "Expected exception of type " + typeof(TException) + " but type of " + ex.GetType() + " was thrown instead.");
                Assert.AreEqual(expectedMessage, ex.Message, "Expected exception with a message of '" + expectedMessage + "' but exception with message of '" + ex.Message + "' was thrown instead.");
                return;
            }
            Assert.Fail("Expected exception of type " + typeof(TException) + " but no exception was thrown.");
        }
    }
}
