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

using Fasterflect;
using FasterflectTest.Common;
using NUnit.Framework;
using FasterflectTest.SampleModel.People;

namespace FasterflectTest.Services
{
	[TestFixture]
	public class MapTest : BaseTest
	{
		#region Helper class for testing Map with fields-based source
		private class Source
		{
			private int employeeId;
			private string name;
			private int age;
			private double metersTravelled;

			public Source( int employeeId, string name, int age, double metersTravelled )
			{
				this.employeeId = employeeId;
				this.name = name;
				this.age = age;
				this.metersTravelled = metersTravelled;
			}

			public Source( string name, int age, double metersTravelled )
			{
				this.name = name;
				this.age = age;
				this.metersTravelled = metersTravelled;
			}

			public int EmployeeId
			{
				get { return employeeId; }
				set { employeeId = value; }
			}

			public string Name
			{
				get { return name; }
				set { name = value; }
			}

			public int Age
			{
				get { return age; }
				set { age = value; }
			}

			public double MetersTravelled
			{
				get { return metersTravelled; }
				set { metersTravelled = value; }
			}
		}
		#endregion

		public MapTest() : base( new[] { typeof(Person), typeof(PersonStruct) } )
		{
		}

		#region Map
		[Test]
		public void TestMapHashCode()
		{
			Assert.AreNotEqual( typeof(Person).DelegateForMap( typeof(PersonStruct) ), typeof(PersonStruct).DelegateForMap( typeof(Person) ) );
		}

		[Test]
		public void TestMap()
		{
			RunWith( ( object person ) =>
					 {
						var source = new { name = "John", age = 10, metersTravelled = 120d };
						source.Map( person );
						VerifyFields( person, source );
					 } );
		}
		#endregion

		#region MapProperties
		[Test]
		public void TestMapProperties()
		{
			RunWith( ( object person ) =>
					 {
						var source = new { Name = "John", Age = 10, MetersTravelled = 120d };
						source.MapProperties( person );
						VerifyProperties( person, source );
					 } );
		}

		[Test]
		public void TestMapPropertiesWithFilter()
		{
			RunWith( ( object person ) =>
					 {
						var currentAge = (int) person.GetPropertyValue( "Age" );
						var source = new { Name = "John", Age = currentAge + 10, MetersTravelled = 120d };
						source.MapProperties( person, "Name", "MetersTravelled" );
						VerifyProperties( person, new { Age = currentAge } );
					 } );
		}

		[Test]
		public void TestMapPropertiesViaSubclass()
		{
			var employee = typeof(Employee).CreateInstance();
			var source = new { EmployeeId = 5, Name = "John", Age = 20, MetersTravelled = 120d };
			source.MapProperties( employee );
			VerifyProperties( employee, source );
		}
		#endregion

		#region MapPropertiesToFields
		[Test]
		public void TestMapPropertiesToFields()
		{
			RunWith( ( object person ) =>
					 {
						var source = new { name = "John", age = 10, metersTravelled = 120d };
						source.MapPropertiesToFields( person );
						VerifyFields( person, source );
					 } );
			//DateTime birthday = new DateTime( 1973, 1, 27 );
			//object source = new { id = 42, birthDay = birthday, name = "Arthur Dent" };
			//Person target = new Person();
			//source.MapPropertiesToFields( target );
			//Verify( new Person( 42, birthday, "Arthur Dent" ), target );
		}

		//[Test]
		//public void TestMapPropertiesToFieldsWithStatic()
		//{
		//    RunWith( ( Type type ) =>
		//       {
		//           var totalPeopleCreated = (int) type.GetFieldValue( "totalPeopleCreated" ) + 1;
		//           type.SetFields( new { totalPeopleCreated } );
		//           VerifyFields( type, new { totalPeopleCreated } );
		//       } );
		//}

		[Test]
		public void TestMapPropertiesToFieldsWithFilter()
		{
			RunWith( ( object person ) =>
					 {
						var currentAge = (int) person.GetFieldValue( "age" );
						var source = new { name = "John", age = currentAge + 10, metersTravelled = 120d };
						source.MapPropertiesToFields( person, "name", "metersTravelled" );
						VerifyFields( person, new { age = currentAge } );
					 } );
		}

		[Test]
		public void TestMapPropertiesToFieldsViaSubclass()
		{
			var employee = typeof(Employee).CreateInstance();
			var source = new { employeeId = 5, name = "John", age = 20, metersTravelled = 120d };
			source.MapPropertiesToFields( employee );
			VerifyFields( employee, source );
		}
		#endregion

		#region MapFields
		[Test]
		public void TestMapFields()
		{
			RunWith( ( object person ) =>
					 {
						var source = new Source( "John", 10, 120d );
						source.MapFields( person );
						VerifyFields( person, new { name = "John", age = 10, metersTravelled = 120d } );
					 } );
		}

		[Test]
		public void TestMapFieldsWithFilter()
		{
			RunWith( ( object person ) =>
					 {
						var currentAge = (int) person.GetFieldValue( "age" );
						var source = new Source( "John", currentAge + 10, 120d );
						source.MapFields( person, "Name", "MetersTravelled" );
						VerifyFields( person, new { age = currentAge } );
					 } );
		}

		[Test]
		public void TestMapFieldsViaSubclass()
		{
			var employee = typeof(Employee).CreateInstance();
			var source = new Source( 5, "John", 10, 120d );
			source.MapFields( employee );
			VerifyFields( employee, source );
		}
		#endregion

		#region MapFieldsToProperties
		[Test]
		public void TestMapFieldsToProperties()
		{
			RunWith( ( object person ) =>
					 {
						var source = new Source( "John", 10, 120d );
						source.MapFieldsToProperties( person );
						VerifyProperties( person, new { Name = "John", Age = 10, MetersTravelled = 120d } );
					 } );
		}

		[Test]
		public void TestMapFieldsToPropertiesWithFilter()
		{
			RunWith( ( object person ) =>
					 {
						var currentAge = (int) person.GetPropertyValue( "Age" );
						var source = new Source( "John", currentAge + 10, 120d );
						source.MapFieldsToProperties( person, "name", "metersTravelled" );
						VerifyProperties( person, new { Age = currentAge } );
					 } );
		}

		[Test]
		public void TestMapFieldsToPropertiesViaSubclass()
		{
			var employee = typeof(Employee).CreateInstance();
			var source = new Source( 5, "John", 20, 120d );
			source.MapFieldsToProperties( employee );
			VerifyProperties( employee, source );
		}
		#endregion
	}
}