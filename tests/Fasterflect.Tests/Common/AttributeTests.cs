using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Fasterflect.Tests
{
	[TestFixture]
	public static class AttributeTests
	{
		[Test]
		public static void Test_Can_Read_Attribute_From_Simple_Class()
		{
			//assert
			Assert.True(typeof(SimpleClass).MembersWith<TestSampleAttribute>(MemberTypes.Field | MemberTypes.Property, Flags.InstanceAnyVisibility).Count() != 0);
			Assert.True(typeof(SimpleClass).MembersWith<TestSampleAttribute>(MemberTypes.Field | MemberTypes.Property, Flags.InstanceAnyVisibility).Any(x => x.Name == "i"));
			Assert.True(typeof(SimpleClass).MembersWith<TestSampleAttribute>(MemberTypes.Field | MemberTypes.Property, Flags.InstanceAnyVisibility)
				.FirstOrDefault().Attribute<TestSampleAttribute>() != null);
		}

		[TestSampleAttribute]
		public class SimpleClass
		{
			[TestSampleAttribute]
			public int i;
		}

		public class TestSampleAttribute : Attribute
		{
			
		}
	}
}
