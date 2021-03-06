﻿using System;
using System.Reflection;
using FluentAssertions.Common;
using FluentAssertions.Types;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    
    public class PropertyInfoAssertionSpecs
    {
        #region BeVirtual

        [Fact]
        public void When_asserting_that_a_property_is_virtual_and_it_is_then_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if WINRT || WINDOWS_PHONE_APP
            PropertyInfo propertyInfo = typeof(ClassWithAllPropertiesVirtual).GetRuntimeProperty("PublicVirtualProperty");
#else
            PropertyInfo propertyInfo = typeof(ClassWithAllPropertiesVirtual).GetProperty("PublicVirtualProperty");
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                propertyInfo.Should().BeVirtual();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [Fact]
        public void When_asserting_that_a_property_is_virtual_and_it_is_not_then_it_fails_with_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if WINRT || WINDOWS_PHONE_APP
            PropertyInfo propertyInfo = typeof(ClassWithNonVirtualPublicProperties).GetRuntimeProperty("PublicNonVirtualProperty");
#else
            PropertyInfo propertyInfo = typeof(ClassWithNonVirtualPublicProperties).GetProperty("PublicNonVirtualProperty");
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                propertyInfo.Should().BeVirtual("we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>()
               .WithMessage(
                   "Expected property String FluentAssertions.Specs.ClassWithNonVirtualPublicProperties.PublicNonVirtualProperty" +
                       " to be virtual because we want to test the error message," +
                       " but it is not.");
        }

        #endregion

        #region BeDecortatedWithOfT

        [Fact]
        public void When_asserting_a_property_is_decorated_with_attribute_and_it_is_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if WINRT || WINDOWS_PHONE_APP
            PropertyInfo propertyInfo = typeof(ClassWithAllPropertiesDecoratedWithDummyAttribute).GetRuntimeProperty("PublicProperty");
#else
            PropertyInfo propertyInfo = typeof(ClassWithAllPropertiesDecoratedWithDummyAttribute).GetProperty("PublicProperty");
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                propertyInfo.Should().BeDecoratedWith<DummyPropertyAttribute>();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }        
        
        [Fact]
        public void When_a_property_is_decorated_with_an_attribute_it_allow_chaining_assertions()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if WINRT || WINDOWS_PHONE_APP
            PropertyInfo propertyInfo = typeof(ClassWithAllPropertiesDecoratedWithDummyAttribute).GetRuntimeProperty("PublicProperty");
#else
            PropertyInfo propertyInfo = typeof(ClassWithAllPropertiesDecoratedWithDummyAttribute).GetProperty("PublicProperty");
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                propertyInfo.Should().BeDecoratedWith<DummyPropertyAttribute>().Which.Value.Should().Be("OtherValue");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage("Expected*OtherValue*Value*");
        }
        
        [Fact]
        public void When_a_property_is_decorated_with_an_attribute_and_multiple_attributes_match_continuation_using_the_matched_value_fail()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if WINRT || WINDOWS_PHONE_APP
            PropertyInfo propertyInfo = typeof(ClassWithAllPropertiesDecoratedWithDummyAttribute).GetRuntimeProperty("PublicPropertyWithSameAttributeTwice");
#else
            PropertyInfo propertyInfo = typeof(ClassWithAllPropertiesDecoratedWithDummyAttribute).GetProperty("PublicPropertyWithSameAttributeTwice");
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                propertyInfo.Should().BeDecoratedWith<DummyPropertyAttribute>().Which.Value.Should().Be("OtherValue");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>();
        }

        [Fact]
        public void When_asserting_a_property_is_decorated_with_attribute_and_it_is_not_it_throw_with_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if WINRT || WINDOWS_PHONE_APP
            PropertyInfo propertyInfo = typeof(ClassWithPropertiesThatAreNotDecoratedWithDummyAttribute).GetRuntimeProperty("PublicProperty");
#else
            PropertyInfo propertyInfo =
                typeof(ClassWithPropertiesThatAreNotDecoratedWithDummyAttribute).GetProperty("PublicProperty");
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                propertyInfo.Should().BeDecoratedWith<DummyPropertyAttribute>("because we want to test the error message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>()
               .WithMessage("Expected property String " +
                   "FluentAssertions.Specs.ClassWithPropertiesThatAreNotDecoratedWithDummyAttribute.PublicProperty to be decorated with " +
                   "FluentAssertions.Specs.DummyPropertyAttribute because we want to test the error message, but that attribute was not found.");
        }

        [Fact]
        public void When_asserting_a_property_is_decorated_with_an_attribute_matching_a_predeicate_but_it_is_not_it_throw_with_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if WINRT || WINDOWS_PHONE_APP
            PropertyInfo propertyInfo = typeof(ClassWithPropertiesThatAreNotDecoratedWithDummyAttribute).GetRuntimeProperty("PublicProperty");
#else
            PropertyInfo propertyInfo =
                typeof(ClassWithPropertiesThatAreNotDecoratedWithDummyAttribute).GetProperty("PublicProperty");
#endif
            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                propertyInfo.Should().BeDecoratedWith<DummyPropertyAttribute>(d => d.Value == "NotARealValue", "because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>()
                .WithMessage(
                    "Expected property String FluentAssertions.Specs.ClassWithPropertiesThatAreNotDecoratedWithDummyAttribute.PublicProperty to be decorated with " +
                        "FluentAssertions.Specs.DummyPropertyAttribute because we want to test the error message," +
                        " but that attribute was not found.");
        }

        [Fact]
        public void When_asserting_a_property_is_decorated_with_attribute_matching_a_predicate_and_it_is_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if WINRT || WINDOWS_PHONE_APP
            PropertyInfo propertyInfo = typeof(ClassWithAllPropertiesDecoratedWithDummyAttribute).GetRuntimeProperty("PublicProperty");
#else
            PropertyInfo propertyInfo =
                typeof(ClassWithAllPropertiesDecoratedWithDummyAttribute).GetProperty("PublicProperty");
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                propertyInfo.Should().BeDecoratedWith<DummyPropertyAttribute>(d => d.Value == "Value");


            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        #endregion

        #region BeWritable

        [Fact]
        public void When_asserting_a_readonly_property_is_writable_it_fails_with_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if WINRT || WINDOWS_PHONE_APP
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("ReadOnlyProperty");
#else
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetProperty("ReadOnlyProperty");
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action = () => propertyInfo.Should().BeWritable("we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action
                .ShouldThrow<XunitException>()
                .WithMessage("Expected property ReadOnlyProperty to have a setter because we want to test the error message.");
        }
        
        [Fact]
        public void When_asserting_a_readwrite_property_is_writable_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if WINRT || WINDOWS_PHONE_APP
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("ReadWriteProperty");
#else
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetProperty("ReadWriteProperty");
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action = () => propertyInfo.Should().BeWritable("that's required");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [Fact]
        public void When_asserting_a_writeonly_property_is_writable_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if WINRT || WINDOWS_PHONE_APP
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("WriteOnlyProperty");
#else
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetProperty("WriteOnlyProperty");
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action = () => propertyInfo.Should().BeWritable("that's required");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        #endregion

        #region BeReadable

        [Fact]
        public void When_asserting_a_readonly_property_is_readable_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if WINRT || WINDOWS_PHONE_APP
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("ReadOnlyProperty");
#else
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetProperty("ReadOnlyProperty");
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action = () => propertyInfo.Should().BeReadable("that's required");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [Fact]
        public void When_asserting_a_readwrite_property_is_readable_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if WINRT || WINDOWS_PHONE_APP
            PropertyInfo propertyInfo = typeof(ClassWithReadOnlyProperties).GetRuntimeProperty("ReadWriteProperty");
#else
            PropertyInfo propertyInfo = typeof(ClassWithReadOnlyProperties).GetProperty("ReadWriteProperty");
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action = () => propertyInfo.Should().BeReadable("that's required");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [Fact]
        public void When_asserting_a_writeonly_property_is_readable_it_fails_with_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if WINRT || WINDOWS_PHONE_APP
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("WriteOnlyProperty");
#else
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetProperty("WriteOnlyProperty");
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action = () => propertyInfo.Should().BeReadable("we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action
                .ShouldThrow<XunitException>()
                .WithMessage("Expected property WriteOnlyProperty to have a getter because we want to test the error message, but it does not.");
        }

        #endregion

        #region NotBeWritable

        [Fact]
        public void When_asserting_a_readonly_property_is_not_writable_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if WINRT || WINDOWS_PHONE_APP
            PropertyInfo propertyInfo = typeof(ClassWithReadOnlyProperties).GetRuntimeProperty("ReadOnlyProperty");
#else
            PropertyInfo propertyInfo = typeof(ClassWithReadOnlyProperties).GetProperty("ReadOnlyProperty");
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action = () => propertyInfo.Should().NotBeWritable("that's required");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [Fact]
        public void When_asserting_a_readwrite_property_is_not_writable_it_fails_with_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if WINRT || WINDOWS_PHONE_APP
            PropertyInfo propertyInfo = typeof(ClassWithReadOnlyProperties).GetRuntimeProperty("ReadWriteProperty");
#else
            PropertyInfo propertyInfo = typeof(ClassWithReadOnlyProperties).GetProperty("ReadWriteProperty");
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action = () => propertyInfo.Should().NotBeWritable("we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action
                .ShouldThrow<XunitException>()
                .WithMessage("Expected property ReadWriteProperty not to have a setter because we want to test the error message.");
        }

        [Fact]
        public void When_asserting_a_writeonly_property_is_not_writeable_it_fails_with_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if WINRT || WINDOWS_PHONE_APP
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("WriteOnlyProperty");
#else
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetProperty("WriteOnlyProperty");
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action = () => propertyInfo.Should().NotBeWritable("we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action
                .ShouldThrow<XunitException>()
                .WithMessage("Expected property WriteOnlyProperty not to have a setter because we want to test the error message.");
        }

        #endregion

        #region NotBeReadable

        [Fact]
        public void When_asserting_a_readonly_property_is_not_readable_it_fails_with_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if WINRT || WINDOWS_PHONE_APP
            PropertyInfo propertyInfo = typeof(ClassWithReadOnlyProperties).GetRuntimeProperty("ReadOnlyProperty");
#else
            PropertyInfo propertyInfo = typeof(ClassWithReadOnlyProperties).GetProperty("ReadOnlyProperty");
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action = () => propertyInfo.Should().NotBeReadable("we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action
                .ShouldThrow<XunitException>()
                .WithMessage("Expected property ReadOnlyProperty not to have a getter because we want to test the error message.");
        }

        [Fact]
        public void When_asserting_a_readwrite_property_is_not_readable_it_fails_with_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if WINRT || WINDOWS_PHONE_APP
            PropertyInfo propertyInfo = typeof(ClassWithReadOnlyProperties).GetRuntimeProperty("ReadWriteProperty");
#else
            PropertyInfo propertyInfo = typeof(ClassWithReadOnlyProperties).GetProperty("ReadWriteProperty");
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action = () => propertyInfo.Should().NotBeReadable("we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action
                .ShouldThrow<XunitException>()
                .WithMessage("Expected property ReadWriteProperty not to have a getter because we want to test the error message.");
        }

        [Fact]
        public void When_asserting_a_writeonly_property_is_not_readable_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if WINRT || WINDOWS_PHONE_APP
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("WriteOnlyProperty");
#else
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetProperty("WriteOnlyProperty");
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action = () => propertyInfo.Should().NotBeReadable("that's required");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        #endregion

        #region BeReadableAccessModifier

        [Fact]
        public void When_asserting_a_public_read_private_write_property_is_public_readable_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if WINRT || WINDOWS_PHONE_APP
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("ReadPrivateWriteProperty");
#else
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetProperty("ReadPrivateWriteProperty");
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action = () => propertyInfo.Should().BeReadable(CSharpAccessModifier.Public, "that's required");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [Fact]
        public void When_asserting_a_private_read_public_write_property_is_public_readable_it_fails_with_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if WINRT || WINDOWS_PHONE_APP
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("WritePrivateReadProperty");
#else
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetProperty("WritePrivateReadProperty");
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action = () => propertyInfo.Should().BeReadable(CSharpAccessModifier.Public, "we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action.ShouldThrow<XunitException>()
                .WithMessage("Expected method get_WritePrivateReadProperty to be Public because we want to test the error message, but it is Private.");
        }

        #endregion

        #region BeWritableAccessModifier

        [Fact]
        public void When_asserting_a_public_write_private_read_property_is_public_writeable_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if WINRT || WINDOWS_PHONE_APP
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("WritePrivateReadProperty");
#else
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetProperty("WritePrivateReadProperty");
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action = () => propertyInfo.Should().BeWritable(CSharpAccessModifier.Public, "that's required");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [Fact]
        public void When_asserting_a_private_write_public_read_property_is_public_writeable_it_fails_with_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if WINRT || WINDOWS_PHONE_APP
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("ReadPrivateWriteProperty");
#else
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetProperty("ReadPrivateWriteProperty");
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action = () => propertyInfo.Should().BeWritable(CSharpAccessModifier.Public, "we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action.ShouldThrow<XunitException>()
                .WithMessage("Expected method set_ReadPrivateWriteProperty to be Public because we want to test the error message, but it is Private.");
        }

        #endregion

        #region Return

        [Fact]
        public void When_asserting_a_String_property_returns_a_String_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if WINRT || WINDOWS_PHONE_APP
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("StringProperty");
#else
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetProperty("StringProperty");
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action = () => propertyInfo.Should().Return(typeof (String));

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [Fact]
        public void When_asserting_a_String_property_returns_an_Int32_it_throw_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if WINRT || WINDOWS_PHONE_APP
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("StringProperty");
#else
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetProperty("StringProperty");
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action = () => propertyInfo.Should().Return(typeof(Int32), "we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action.ShouldThrow<XunitException>()
                .WithMessage("Expected Type of property StringProperty to be System.Int32 because we want to test the error " +
                             "message, but it is System.String.");
        }

        #endregion

        #region Return

        [Fact]
        public void When_asserting_a_String_property_returnsOfT_a_String_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if WINRT || WINDOWS_PHONE_APP
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("StringProperty");
#else
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetProperty("StringProperty");
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action = () => propertyInfo.Should().Return<string>();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        #endregion

        #region Internal classes used in unit tests

        private class ClassWithProperties
        {
            public string ReadOnlyProperty { get { return ""; } }
            public string ReadPrivateWriteProperty { get; private set; }
            public string ReadWriteProperty { get; set; }
            public string WritePrivateReadProperty { private get; set; }
            public string WriteOnlyProperty { set{} }
            public string StringProperty { get; set; }
        }

        #endregion
    }
}