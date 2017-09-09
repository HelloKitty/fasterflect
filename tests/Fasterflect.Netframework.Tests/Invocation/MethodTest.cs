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
using System.Linq;
using Fasterflect;
using NUnit.Framework;

namespace FasterflectTest.Invocation
{
	[TestFixture]
	public class MethodTest : BaseInvocationTest
	{
		public MethodTest()
			: base()
		{

		}

		[Test]
		public void TestInvokeInstanceMethod()
		{
			RunWith( ( object person ) =>
			   {
				   var elements = new[] { 1d, 2d, 3d, 4d, 5d };
				   elements.ForEach( element => person.CallMethod( "Walk", element ) );
				   Assert.AreEqual( elements.Sum(), person.GetFieldValue( "metersTravelled" ) );
			   } );
		}

		[Test]
		public void TestInvokePrivateInstanceMethodUnderNonPublicBindingFlags()
		{
			RunWith((object person) => person.CallMethod("Walk", Flags.NonPublic | Flags.Instance, 10d));
		}

		[Test]
		[ExpectedException(typeof(MissingMethodException))]
		public void TestInvokePublicStaticMethodUnderStaticBindingFlags()
		{
			RunWith((object person) => person.CallMethod("Walk", Flags.StaticAnyVisibility, 10d));
		}

		[Test]
		[ExpectedException(typeof(MissingMethodException))]
		public void TestInvokePrivateInstanceMethodUnderPublicBindingFlags()
		{
			RunWith((object person) => person.CallMethod("Walk", Flags.Public | Flags.Instance, 10d));
		}

		[Test]
		public void TestInvokeInstanceMethodViaMethodInfo()
		{
			RunWith( ( object person ) =>
			   {
				   var elements = new[] { 1d, 2d, 3d, 4d, 5d };
				   var methodInfo = person.UnwrapIfWrapped().GetType().Method( "Walk", new [] { typeof(double) }, Flags.InstanceAnyVisibility );
				   Assert.NotNull(methodInfo);
				   elements.ForEach( element => methodInfo.Call( person, element ) );
				   Assert.AreEqual( elements.Sum(), person.GetFieldValue( "metersTravelled" ) );
			   } );
		}

		[Test]
		public void TestInvokeWithCoVariantReturnAndParamType()
		{
			var person = PersonType.CreateInstance();
			var friend = EmployeeType.CreateInstance();
			var result = person.CallMethod( "AddFriend", friend );
			Assert.AreSame( friend, result );
		}

		[Test]
		public void TestInvokeMethodWithOutArgument()
		{
			RunWith( ( object person ) =>
			   {
				   var arguments = new object[] { 10d, null };
				   person.CallMethod( "Walk", new[] { typeof(double), typeof(double).MakeByRefType() }, arguments );
				   Assert.AreEqual( person.GetFieldValue( "metersTravelled" ), arguments[ 1 ] );
			   } );
		}

		[Test]
		public void TestInvokeExplicitlyImplementedMethod()
		{
			var employee = EmployeeType.CreateInstance();
			var currentMeters = (double) employee.GetFieldValue( "metersTravelled" );
			employee.CallMethod("Swim", Flags.InstanceAnyVisibility | Flags.TrimExplicitlyImplemented, 100d);
			VerifyFields( employee, new { metersTravelled = currentMeters + 100 } );
		}

		[Test]
		public void TestInvokeBaseClassMethods()
		{
			var employee = EmployeeType.CreateInstance();
			var currentMeters = (double) employee.GetFieldValue( "metersTravelled" );
			employee.CallMethod( "Walk", 100d );
			VerifyFields( employee, new { metersTravelled = currentMeters + 100 } );
		}

		[Test]
		public void TestInvokeStaticMethod()
		{
			RunWith( ( Type type ) =>
			   {
				   var totalPeopleCreated = (int) type.GetFieldValue( "totalPeopleCreated" );
				   Assert.AreEqual( totalPeopleCreated, type.CallMethod( "GetTotalPeopleCreated" ) );
			   } );
		}

		[Test]
		[ExpectedException(typeof(MissingMethodException))]
		public void TestInvokePublicStaticMethodUnderNonPublicBindingFlags()
		{
			RunWith((Type type) => type.CallMethod("GetTotalPeopleCreated", Flags.NonPublic | Flags.Static));
		}

		//TODO: Find out why we're getting missing member exception and not missing method.
		[Test]
		[ExpectedException(typeof(MissingMethodException))]
		public void TestInvokePublicStaticMethodUnderInstanceBindingFlags()
		{
			bool result = true;
			RunWith((Type type) =>
			{
				try
				{
					type.CallMethod("GetTotalPeopleCreated", Flags.InstanceAnyVisibility);
				}
				catch (Exception e)
				{
					result = false;
				}
			});

			if (!result)
				throw new MissingMemberException();
		}

		[Test]
		public void TestInvokePublicStaticMethodUnderPublicBindingFlags()
		{
			RunWith((Type type) => type.CallMethod("GetTotalPeopleCreated", Flags.Public | Flags.Static));
		}

		[Test]
		public void TestInvokeStaticMethodViaMethodInfo()
		{
			RunWith( ( Type type ) =>
			   {
				   var totalPeopleCreated = (int) type.GetFieldValue( "totalPeopleCreated" );
				   Assert.AreEqual( totalPeopleCreated,
									type.Method( "GetTotalPeopleCreated", Flags.StaticAnyVisibility ).Call() );
			   } );
		}

		[Test]
		public void TestInvokeStaticMethodsWithArgument()
		{
			RunWith( ( Type type ) =>
			   {
				   var totalPeopleCreated = (int) type.GetFieldValue( "totalPeopleCreated" );
				   Assert.AreEqual( totalPeopleCreated + 20, type.CallMethod( "AdjustTotalPeopleCreated", 20 ) );
			   } );
		}

		[Test]
		[ExpectedException( typeof(MissingMethodException))]
		public void TestInvokeNonExistentInstanceMethod()
		{
			RunWith( ( object person ) => person.CallMethod( "not_exist" ) );
		}

		[Test]
		[ExpectedException( typeof(MissingMethodException) )]
		public void TestInvokeNonExistentStaticMethod()
		{
			RunWith( ( Type type ) => type.CallMethod( "not_exist" ) );
		}
	}
}