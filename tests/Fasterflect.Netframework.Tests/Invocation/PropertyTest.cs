#region License
// Copyright 2010 Buu Nguyen, Morten Mertner
// 
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
// http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.
// 
// The latest version of this file can be found at http://fasterflect.codeplex.com/
#endregion

using System;
using System.Reflection;
using Fasterflect;
using FasterflectTest.SampleModel.People;
using NUnit.Framework;

namespace FasterflectTest.Invocation
{
	[TestFixture]
	public class PropertyTest : BaseInvocationTest
	{
		[Test]
		public void TestAccessStaticProperties()
		{
			RunWith( ( Type type ) =>
			         {
			         	int totalPeopleCreated = (int) type.GetPropertyValue( "TotalPeopleCreated" ) + 1;
			         	type.SetPropertyValue( "TotalPeopleCreated", totalPeopleCreated );
			         	VerifyProperties( type, new { totalPeopleCreated } );
			         } );
		}

		[Test]
		[ExpectedException( typeof(MissingMemberException) )]
		public void TestGetPublicStaticPropertyUnderNonPublicBindingFlags()
		{
			RunWith( ( Type type ) => type.GetPropertyValue( "TotalPeopleCreated", Flags.NonPublic | Flags.Instance ) );
		}

		[Test]
		[ExpectedException( typeof(MissingMemberException) )]
		public void TestSetPublicStaticPropertyUnderNonPublicBindingFlags()
		{
			RunWith( ( Type type ) => type.SetPropertyValue( "TotalPeopleCreated", 2, Flags.NonPublic | Flags.Instance ) );
		}

		[Test]
		public void TestAccessStaticPropertyViaPropertyInfo()
		{
			RunWith( ( Type type ) =>
			         {
			         	PropertyInfo propInfo = type.Property( "TotalPeopleCreated", Flags.StaticAnyVisibility );
			         	int totalPeopleCreated = (int) propInfo.Get() + 1;
			         	propInfo.Set( totalPeopleCreated );
			         	VerifyProperties( type, new { totalPeopleCreated } );
			         } );
		}

		[Test]
		public void TestAccessInstanceProperties()
		{
			RunWith( ( object person ) =>
			         {
			         	string name = (string) person.GetPropertyValue( "Name" ) + " updated";
			         	person.SetPropertyValue( "Name", name );
			         	VerifyProperties( person, new { name } );
			         } );
		}

		[Test]
		[ExpectedException( typeof(MissingMemberException) )]
		public void TestGetPublicPropertyUnderNonPublicBindingFlags()
		{
			RunWith( ( object person ) => person.GetPropertyValue( "Name", Flags.NonPublic | Flags.Instance ) );
		}

		[Test]
		[ExpectedException( typeof(MissingMemberException) )]
		public void TestSetPublicPropertyUnderNonPublicBindingFlags()
		{
			RunWith( ( object person ) => person.SetPropertyValue( "Name", "John", Flags.NonPublic | Flags.Instance ) );
		}

		[Test]
		public void TestAccessInstancePropertyViaPropertyInfo()
		{
			RunWith( ( object person ) =>
			         {
			         	PropertyInfo propInfo = person.UnwrapIfWrapped().GetType().Property( "Name" );
			         	string name = (string) propInfo.Get( person ) + " updated";
			         	propInfo.Set( person, name );
			         	VerifyProperties( person, new { name } );
			         } );
		}

		[Test]
		public void TestChainInstancePropertySetters()
		{
			RunWith( ( object person ) =>
			         {
			         	person.SetPropertyValue( "Name", "John" )
			         		.SetPropertyValue( "Age", 20 )
			         		.SetPropertyValue( "MetersTravelled", 120d );
			         	VerifyProperties( person, new { Name = "John", Age = 20, MetersTravelled = 120d } );
			         } );
		}

		[Test]
		public void TestAccessStaticPropertiesViaSubclassType()
		{
			int totalPeopleCreated = (int) EmployeeType.GetPropertyValue( "TotalPeopleCreated" ) + 1;
			EmployeeType.SetPropertyValue( "TotalPeopleCreated", totalPeopleCreated );
			VerifyProperties( EmployeeType, new { totalPeopleCreated } );
		}

		[Test]
		[ExpectedException( typeof(MissingMemberException) )]
		public void TestSetNotExistentProperty()
		{
			RunWith( ( object person ) => person.SetPropertyValue( "not_exist", null ) );
		}

		[Test]
		[ExpectedException( typeof(MissingMemberException) )]
		public void TestSetNotExistentStaticProperty()
		{
			RunWith( ( Type type ) => type.SetPropertyValue( "not_exist", null ) );
		}

		[Test]
		[ExpectedException( typeof(MissingMemberException) )]
		public void TestGetNotExistentProperty()
		{
			RunWith( ( object person ) => person.GetPropertyValue( "not_exist" ) );
		}

		[Test]
		[ExpectedException( typeof(MissingMemberException) )]
		public void TestGetNotExistentStaticProperty()
		{
			RunWith( ( Type type ) => type.GetPropertyValue( "not_exist" ) );
		}

		[Test]
		[ExpectedException( typeof(InvalidCastException) )]
		public void TestSetInvalidValueType()
		{
			RunWith( ( object person ) => person.SetPropertyValue( "MetersTravelled", 1 ) );
		}

		[Test]
		[ExpectedException( typeof(NullReferenceException) )]
		public void TestSetNullToValueType()
		{
			RunWith( ( object person ) => person.SetPropertyValue( "Age", null ) );
		}

		[Test]
		public void TestGetPropertyUsingMemberExpression()
		{
			var person = new Person( "Arthur", 32 );
			Assert.AreEqual( "Arthur", person.GetPropertyValue( () => person.Name ) );
			person.SetPropertyValue( () => person.Name, "Ford" );
			Assert.AreEqual( "Ford", person.Name );
		}
	}
}