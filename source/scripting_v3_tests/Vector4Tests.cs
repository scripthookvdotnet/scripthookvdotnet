//
// Copyright (C) 2026 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//
using System;
using System.Globalization;
using GTA.Math;
using Xunit;
namespace ScriptHookVDotNet_APIv3_Tests.Math
{
    public class Vector4Tests
    {
        public static TheoryData<Vector4, float> Length_Data =>
            new TheoryData<Vector4, float>
            {
                { new Vector4(0f, 0f, 0f, 0f), 0f },
                { new Vector4(3f, 4f, 0f, 0f), 5f },
                { new Vector4(1f, 2f, 2f, 4f), 5f },
            };
        public static TheoryData<Vector4, float> LengthSquared_Data =>
            new TheoryData<Vector4, float>
            {
                { new Vector4(0f, 0f, 0f, 0f), 0f },
                { new Vector4(3f, 4f, 0f, 0f), 25f },
                { new Vector4(1f, 2f, 2f, 4f), 25f },
            };
        public static TheoryData<Vector4, Vector4, float> Dot_Data =>
            new TheoryData<Vector4, Vector4, float>
            {
                { new Vector4(1f, 0f, 0f, 0f), new Vector4(0f, 1f, 0f, 0f), 0f },
                { new Vector4(1f, 2f, 3f, 4f), new Vector4(5f, 6f, 7f, 8f), 70f },
                { new Vector4(-1f, -2f, -3f, -4f), new Vector4(1f, 2f, 3f, 4f), -30f },
            };
        public static TheoryData<Vector4, Vector4> Normalize_Data =>
            new TheoryData<Vector4, Vector4>
            {
                { new Vector4(1f, 0f, 0f, 0f), new Vector4(1f, 0f, 0f, 0f) },
                { new Vector4(0f, 1f, 0f, 0f), new Vector4(0f, 1f, 0f, 0f) },
                { new Vector4(0f, 0f, 1f, 0f), new Vector4(0f, 0f, 1f, 0f) },
                { new Vector4(0f, 0f, 0f, 1f), new Vector4(0f, 0f, 0f, 1f) },
                { new Vector4(1f, 1f, 1f, 1f), new Vector4(0.5f, 0.5f, 0.5f, 0.5f) },
                { new Vector4(3f, 4f, 0f, 0f), new Vector4(0.6f, 0.8f, 0f, 0f) },
            };
        public static TheoryData<Vector4, Vector4, Vector4, Vector4> Clamp_Data =>
            new TheoryData<Vector4, Vector4, Vector4, Vector4>
            {
                {
                    new Vector4(-10f, 0.5f, 3f, 20f),
                    new Vector4(-5f, -1f, 1f, 0f),
                    new Vector4(5f, 1f, 2f, 10f),
                    new Vector4(-5f, 0.5f, 2f, 10f)
                },
                {
                    new Vector4(0f, 0f, 0f, 0f),
                    new Vector4(-1f, -1f, -1f, -1f),
                    new Vector4(1f, 1f, 1f, 1f),
                    new Vector4(0f, 0f, 0f, 0f)
                },
            };
        public static TheoryData<Vector4, Vector4, float, Vector4> Lerp_Data =>
            new TheoryData<Vector4, Vector4, float, Vector4>
            {
                {
                    new Vector4(1f, 2f, 3f, 4f),
                    new Vector4(5f, 6f, 7f, 8f),
                    0f,
                    new Vector4(1f, 2f, 3f, 4f)
                },
                {
                    new Vector4(1f, 2f, 3f, 4f),
                    new Vector4(5f, 6f, 7f, 8f),
                    0.5f,
                    new Vector4(3f, 4f, 5f, 6f)
                },
                {
                    new Vector4(1f, 2f, 3f, 4f),
                    new Vector4(5f, 6f, 7f, 8f),
                    1f,
                    new Vector4(5f, 6f, 7f, 8f)
                },
            };
        public static TheoryData<Vector4, Vector4, Vector4> Min_Data =>
            new TheoryData<Vector4, Vector4, Vector4>
            {
                {
                    new Vector4(1f, 10f, -2f, 6f),
                    new Vector4(2f, 9f, -3f, 7f),
                    new Vector4(1f, 9f, -3f, 6f)
                },
            };
        public static TheoryData<Vector4, Vector4, Vector4> Max_Data =>
            new TheoryData<Vector4, Vector4, Vector4>
            {
                {
                    new Vector4(1f, 10f, -2f, 6f),
                    new Vector4(2f, 9f, -3f, 7f),
                    new Vector4(2f, 10f, -2f, 7f)
                },
            };
        public static TheoryData<Vector4, Vector4, Vector4> Addition_Data =>
            new TheoryData<Vector4, Vector4, Vector4>
            {
                {
                    new Vector4(1f, 2f, 3f, 4f),
                    new Vector4(5f, 6f, 7f, 8f),
                    new Vector4(6f, 8f, 10f, 12f)
                },
            };
        public static TheoryData<Vector4, Vector4, Vector4> Subtraction_Data =>
            new TheoryData<Vector4, Vector4, Vector4>
            {
                {
                    new Vector4(5f, 6f, 7f, 8f),
                    new Vector4(1f, 2f, 3f, 4f),
                    new Vector4(4f, 4f, 4f, 4f)
                },
            };
        public static TheoryData<Vector4, Vector4> Negation_Data =>
            new TheoryData<Vector4, Vector4>
            {
                { new Vector4(1f, -2f, 3f, -4f), new Vector4(-1f, 2f, -3f, 4f) },
            };
        public static TheoryData<Vector4, float, Vector4> Scale_Data =>
            new TheoryData<Vector4, float, Vector4>
            {
                { new Vector4(1f, 2f, 3f, 4f), 2.5f, new Vector4(2.5f, 5f, 7.5f, 10f) },
            };
        public static TheoryData<Vector4, float, Vector4> Division_Data =>
            new TheoryData<Vector4, float, Vector4>
            {
                { new Vector4(4f, 8f, 12f, 16f), 2f, new Vector4(2f, 4f, 6f, 8f) },
            };
        public static TheoryData<Vector4, Vector4> Equality_Data_Same =>
            new TheoryData<Vector4, Vector4>
            {
                { new Vector4(0f, 0f, 0f, 0f), new Vector4(0f, 0f, 0f, 0f) },
                { new Vector4(1f, 2f, 3f, 4f), new Vector4(1f, 2f, 3f, 4f) },
            };
        public static TheoryData<Vector4, Vector4> Equality_Data_Different =>
            new TheoryData<Vector4, Vector4>
            {
                { new Vector4(0f, 0f, 0f, 0f), new Vector4(0.0001f, 0f, 0f, 0f) },
                { new Vector4(0f, 0f, 0f, 0f), new Vector4(0f, 1f, 0f, 0f) },
                { new Vector4(1f, 2f, 3f, 4f), new Vector4(4f, 3f, 2f, 1f) },
            };
        public static TheoryData<Vector4, object> Equals_Object_Data_Not_Vector4 =>
            new TheoryData<Vector4, object>
            {
                { new Vector4(0f, 0f, 0f, 0f), new object() },
                { new Vector4(1f, 2f, 3f, 4f), "not a vector" },
            };
        public static TheoryData<Vector3, Vector4> Explicit_Cast_From_Vector3_Data =>
            new TheoryData<Vector3, Vector4>
            {
                { new Vector3(1f, 2f, 3f), new Vector4(1f, 2f, 3f, 0f) },
                { new Vector3(-1f, -2f, -3f), new Vector4(-1f, -2f, -3f, 0f) },
            };
        public static TheoryData<Vector4, Vector3> Explicit_Cast_To_Vector3_Data =>
            new TheoryData<Vector4, Vector3>
            {
                { new Vector4(1f, 2f, 3f, 0f), new Vector3(1f, 2f, 3f) },
                { new Vector4(-1f, -2f, -3f, 0f), new Vector3(-1f, -2f, -3f) },
            };
        public static TheoryData<Vector2, Vector4> Explicit_Cast_From_Vector2_Data =>
            new TheoryData<Vector2, Vector4>
            {
                { new Vector2(1f, 2f), new Vector4(1f, 2f, 0f, 0f) },
                { new Vector2(-1f, -2f), new Vector4(-1f, -2f, 0f, 0f) },
            };
        public static TheoryData<Vector4, Vector2> Explicit_Cast_To_Vector2_Data =>
            new TheoryData<Vector4, Vector2>
            {
                { new Vector4(1f, 2f, 3f, 0f), new Vector2(1f, 2f) },
                { new Vector4(-1f, -2f, -3f, 0f), new Vector2(-1f, -2f) },
            };
        public static TheoryData<Vector4> ToString_Data =>
            new TheoryData<Vector4>
            {
                { new Vector4(0f, 0f, 0f, 0f) },
                { new Vector4(1f, 2f, 3f, 4f) },
                { new Vector4(0.25f, 0.5f, 2.5f, -12.5f) },
            };
        public class ToStringFormat_Data_Class : TheoryData<Vector4, string>
        {
            public ToStringFormat_Data_Class()
            {
                Vector4[] vectors = new Vector4[]
                {
                    new Vector4(1f, 2f, 3f, 4f),
                    new Vector4(0.25f, 0.5f, 2.5f, -12.5f),
                    new Vector4(1000f, -20000.5f, -300000.75f, -400000.5f)
                };
                string[] formats = new string[]
                {
                    "N",
                    "F3",
                    "e4"
                };
                foreach (Vector4 vector in vectors)
                {
                    foreach (string format in formats)
                    {
                        Add(vector, format);
                    }
                }
            }
        }
        public class ToStringIFormatProvider_Data_Class : TheoryData<Vector4, string, IFormatProvider>
        {
            public ToStringIFormatProvider_Data_Class()
            {
                Vector4[] vectors = new Vector4[]
                {
                    new Vector4(1f, 2f, 3f, 4f),
                    new Vector4(0.25f, 0.5f, 2.5f, -12.5f),
                    new Vector4(1000f, -20000.5f, -300000.75f, -400000.5f)
                };
                string[] formats = new string[]
                {
                    "N",
                    "F3",
                    "e4"
                };
                IFormatProvider[] providers = new IFormatProvider[]
                {
                    CultureInfo.InvariantCulture,
                    new CultureInfo("en-US"),
                    new CultureInfo("fr-FR")
                };
                
                foreach (Vector4 vector in vectors)
                {
                    foreach (string format in formats)
                    {
                        foreach (IFormatProvider provider in providers)
                        {
                            Add(vector, format, provider);
                        }
                    }
                }
            }
        }
        [Fact]
        public void Zero_returns_vector_with_all_components_zero()
        {
            Vector4 actual = Vector4.Zero;
            Assert.Equal(0f, actual.X);
            Assert.Equal(0f, actual.Y);
            Assert.Equal(0f, actual.Z);
            Assert.Equal(0f, actual.W);
        }
        [Fact]
        public void One_returns_vector_with_all_components_one()
        {
            Vector4 actual = Vector4.One;
            Assert.Equal(1f, actual.X);
            Assert.Equal(1f, actual.Y);
            Assert.Equal(1f, actual.Z);
            Assert.Equal(1f, actual.W);
        }
        [Theory]
        [InlineData(0, 1f)]
        [InlineData(1, 2f)]
        [InlineData(2, 3f)]
        [InlineData(3, 4f)]
        public void Indexer_getter_returns_component_at_index(int index, float expected)
        {
            Vector4 value = new Vector4(1f, 2f, 3f, 4f);
            Assert.Equal(expected, value[index]);
        }
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Indexer_setter_sets_component_at_index(int index)
        {
            Vector4 value = new Vector4(1f, 2f, 3f, 4f);
            value[index] = 5f;
            Assert.Equal(5f, value[index]);
        }
        [Theory]
        [InlineData(-1)]
        [InlineData(4)]
        public void Indexer_getter_throws_argument_out_of_range_exception_on_invalid_index(int index)
        {
            Vector4 value = new Vector4(1f, 2f, 3f, 4f);
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = value[index]);
        }
        [Theory]
        [InlineData(-1)]
        [InlineData(4)]
        public void Indexer_setter_throws_argument_out_of_range_exception_on_invalid_index(int index)
        {
            Vector4 value = new Vector4(1f, 2f, 3f, 4f);
            Assert.Throws<ArgumentOutOfRangeException>(() => value[index] = 42f);
        }
        [Theory]
        [MemberData(nameof(Length_Data))]
        public void Length_returns_magnitude(Vector4 input, float expected)
        {
            float actual = input.Length();
            Assert.True(System.Math.Abs(actual - expected) <= 1e-5f,
                $"Assert failed. actual: {actual}, expected: {expected}");
        }
        [Theory]
        [MemberData(nameof(LengthSquared_Data))]
        public void LengthSquared_returns_squared_magnitude(Vector4 input, float expected)
        {
            float actual = input.LengthSquared();
            Assert.True(System.Math.Abs(actual - expected) <= 1e-5f,
                $"Assert failed. actual: {actual}, expected: {expected}");
        }
        [Theory]
        [MemberData(nameof(Dot_Data))]
        public void Dot_returns_dot_product(Vector4 left, Vector4 right, float expected)
        {
            float actual = Vector4.Dot(left, right);
            Assert.True(System.Math.Abs(actual - expected) <= 1e-5f,
                $"Assert failed. actual: {actual}, expected: {expected}");
        }
        [Theory]
        [MemberData(nameof(Normalize_Data))]
        public void Normalize_instance_method_normalizes_vector_in_place(Vector4 input, Vector4 expected)
        {
            Vector4 actual = input;
            actual.Normalize();
            EqualsApprox(actual, expected, 1e-5f);
        }
        [Fact]
        public void Normalize_instance_method_does_not_change_zero_vector()
        {
            Vector4 value = Vector4.Zero;
            value.Normalize();
            EqualsApprox(value, Vector4.Zero, 1e-6f);
        }
        [Theory]
        [MemberData(nameof(Normalize_Data))]
        public void Normalized_property_returns_normalized_copy(Vector4 input, Vector4 expected)
        {
            Vector4 origValue = input;
            Vector4 actual = origValue.Normalized;
            EqualsApprox(actual, expected, 1e-5f);
            EqualsApprox(origValue, input, 1e-5f);
        }
        [Theory]
        [MemberData(nameof(Normalize_Data))]
        public void Static_normalize_returns_normalized_copy(Vector4 input, Vector4 expected)
        {
            Vector4 origValue = input;
            Vector4 actual = Vector4.Normalize(input);
            EqualsApprox(actual, expected, 1e-5f);
            EqualsApprox(origValue, input, 1e-5f);
        }
        [Theory]
        [MemberData(nameof(Clamp_Data))]
        public void Clamp_restricts_components_to_range(Vector4 value, Vector4 min, Vector4 max, Vector4 expected)
        {
            Vector4 actual = Vector4.Clamp(value, min, max);
            EqualsExact(actual, expected);
        }
        [Theory]
        [MemberData(nameof(Lerp_Data))]
        public void Lerp_returns_linear_interpolation(Vector4 start, Vector4 end, float amount, Vector4 expected)
        {
            Vector4 actual = Vector4.Lerp(start, end, amount);
            EqualsApprox(actual, expected, 1e-5f);
        }
        [Theory]
        [MemberData(nameof(Min_Data))]
        public void Min_returns_component_wise_minimum(Vector4 left, Vector4 right, Vector4 expected)
        {
            Vector4 actual = Vector4.Min(left, right);
            EqualsExact(actual, expected);
        }
        [Theory]
        [MemberData(nameof(Max_Data))]
        public void Max_returns_component_wise_maximum(Vector4 left, Vector4 right, Vector4 expected)
        {
            Vector4 actual = Vector4.Max(left, right);
            EqualsExact(actual, expected);
        }
        [Theory]
        [MemberData(nameof(Addition_Data))]
        public void Addition_operator_returns_sum(Vector4 left, Vector4 right, Vector4 expected)
        {
            Vector4 actual = left + right;
            EqualsApprox(actual, expected, 1e-6f);
        }
        [Theory]
        [MemberData(nameof(Subtraction_Data))]
        public void Subtraction_operator_returns_difference(Vector4 left, Vector4 right, Vector4 expected)
        {
            Vector4 actual = left - right;
            EqualsApprox(actual, expected, 1e-6f);
        }
        [Theory]
        [MemberData(nameof(Negation_Data))]
        public void Unary_minus_operator_negates_components(Vector4 value, Vector4 expected)
        {
            Vector4 actual = -value;
            EqualsExact(actual, expected);
        }
        [Theory]
        [MemberData(nameof(Scale_Data))]
        public void Vector_times_scalar_operator_scales_components(Vector4 value, float scale, Vector4 expected)
        {
            Vector4 actual = value * scale;
            EqualsApprox(actual, expected, 1e-6f);
        }
        [Theory]
        [MemberData(nameof(Scale_Data))]
        public void Scalar_times_vector_operator_scales_components(Vector4 value, float scale, Vector4 expected)
        {
            Vector4 actual = scale * value;
            EqualsApprox(actual, expected, 1e-6f);
        }
        [Theory]
        [MemberData(nameof(Division_Data))]
        public void Division_operator_scales_components(Vector4 value, float scale, Vector4 expected)
        {
            Vector4 actual = value / scale;
            EqualsApprox(actual, expected, 1e-6f);
        }
        [Theory]
        [MemberData(nameof(Equality_Data_Same))]
        public void Equality_operator_returns_true_if_all_components_are_equal(Vector4 left, Vector4 right)
        {
            Assert.True(left.X == right.X &&
                        left.Y == right.Y &&
                        left.Z == right.Z &&
                        left.W == right.W,
                $"Assert failed. left: {left.ToString()}, right: {right.ToString()}");
            Assert.True(left == right);
        }
        [Theory]
        [MemberData(nameof(Equality_Data_Same))]
        public void Inequality_operator_returns_false_if_all_components_are_equal(Vector4 left, Vector4 right)
        {
            Assert.True(left.X == right.X &&
                        left.Y == right.Y &&
                        left.Z == right.Z &&
                        left.W == right.W,
                $"Assert failed. left: {left.ToString()}, right: {right.ToString()}");
            Assert.False(left != right);
        }
        [Theory]
        [MemberData(nameof(Equality_Data_Different))]
        public void Equality_operator_returns_false_if_some_components_are_different(Vector4 left, Vector4 right)
        {
            Assert.False(left.X == right.X &&
                        left.Y == right.Y &&
                        left.Z == right.Z &&
                        left.W == right.W,
                $"Assert failed. left: {left.ToString()}, right: {right.ToString()}");
            Assert.False(left == right);
        }
        [Theory]
        [MemberData(nameof(Equality_Data_Different))]
        public void Inequality_operator_returns_true_if_some_components_are_different(Vector4 left, Vector4 right)
        {
            Assert.False(left.X == right.X &&
                        left.Y == right.Y &&
                        left.Z == right.Z &&
                        left.W == right.W,
                $"Assert failed. left: {left.ToString()}, right: {right.ToString()}");
            Assert.True(left != right);
        }
        [Theory]
        [MemberData(nameof(Equality_Data_Same))]
        public void Equals_method_returns_true_if_all_components_are_equal(Vector4 left, Vector4 right)
        {
            Assert.True(left.X == right.X &&
                        left.Y == right.Y &&
                        left.Z == right.Z &&
                        left.W == right.W,
                $"Assert failed. left: {left.ToString()}, right: {right.ToString()}");
            Assert.True(left.Equals(right));
        }
        [Theory]
        [MemberData(nameof(Equality_Data_Same))]
        public void Equals_method_returns_true_if_passed_argument_is_object_casted_from_the_same_vector(Vector4 left, Vector4 right)
        {
            Assert.True(left.X == right.X &&
                        left.Y == right.Y &&
                        left.Z == right.Z &&
                        left.W == right.W,
                $"Assert failed. left: {left.ToString()}, right: {right.ToString()}");
            Assert.True(left.Equals((object)right));
        }
        [Theory]
        [MemberData(nameof(Equals_Object_Data_Not_Vector4))]
        public void Equals_method_returns_false_if_passed_argument_is_not_a_Vector4(Vector4 input, object obj)
        {
            Assert.False(obj.GetType() == typeof(Vector4));
            Assert.False(input.Equals(obj));
        }
        [Theory]
        [MemberData(nameof(Equality_Data_Same))]
        public void GetHashCode_returns_same_value_for_equal_vectors(Vector4 left, Vector4 right)
        {
            Assert.True(left.X == right.X &&
                        left.Y == right.Y &&
                        left.Z == right.Z &&
                        left.W == right.W,
                $"Assert failed. left: {left.ToString()}, right: {right.ToString()}");
            Assert.Equal(left.GetHashCode(), right.GetHashCode());
        }
        [Theory]
        [MemberData(nameof(Explicit_Cast_From_Vector3_Data))]
        public void Explicit_cast_from_vector3_sets_w_to_zero(Vector3 input, Vector4 expected)
        {
            Vector4 actual = (Vector4)input;
            EqualsExact(actual, expected);
        }
        [Theory]
        [MemberData(nameof(Explicit_Cast_To_Vector3_Data))]
        public void Explicit_cast_to_vector3_discards_w_component(Vector4 input, Vector3 expected)
        {
            Vector3 actual = (Vector3)input;
            Assert.Equal(expected.X, actual.X);
            Assert.Equal(expected.Y, actual.Y);
            Assert.Equal(expected.Z, actual.Z);
        }
        [Theory]
        [MemberData(nameof(Explicit_Cast_From_Vector2_Data))]
        public void Explicit_cast_from_vector2_sets_z_and_w_to_zero(Vector2 input, Vector4 expected)
        {
            Vector4 actual = (Vector4)input;
            EqualsApprox(actual, expected, 1e-6f);
        }
        [Theory]
        [MemberData(nameof(Explicit_Cast_To_Vector2_Data))]
        public void Explicit_cast_to_vector2_discards_z_and_w_components(Vector4 input, Vector2 expected)
        {
            Vector2 actual = (Vector2)input;
            Assert.Equal(expected.X, actual.X);
            Assert.Equal(expected.Y, actual.Y);
        }
        [Theory]
        [MemberData(nameof(ToString_Data))]
        public void ToString_without_format_returns_expected_component_labels(Vector4 value)
        {
            string expected = $"X:{value.X.ToString()} Y:{value.Y.ToString()} Z:{value.Z.ToString()} W:{value.W.ToString()}";
            string actual = value.ToString();
            Assert.Equal(expected, actual);
        }
        [Theory]
        [MemberData(nameof(ToString_Data))]
        public void ToString_with_null_format_matches_default_ToString(Vector4 value)
        {
            Assert.Equal(value.ToString(), value.ToString(null));
        }
        [Theory]
        [ClassData(typeof(ToStringFormat_Data_Class))]
        public void ToString_with_format_formats_all_components(Vector4 value, string format)
        {
            string expected = $"X:{value.X.ToString(format)} Y:{value.Y.ToString(format)} Z:{value.Z.ToString(format)} W:{value.W.ToString(format)}";
            string actual = value.ToString(format);
            Assert.Equal(expected, actual);
        }
        [Theory]
        [ClassData(typeof(ToStringIFormatProvider_Data_Class))]
        public void ToString_with_format_and_provider_formats_all_components(Vector4 value, string format, IFormatProvider provider)
        {
            string expected = $"X:{value.X.ToString(format, provider)} Y:{value.Y.ToString(format, provider)} Z:{value.Z.ToString(format, provider)} W:{value.W.ToString(format, provider)}";
            string actual = value.ToString(format, provider);
            Assert.Equal(expected, actual);
        }
        private static void EqualsExact(Vector4 left, Vector4 right)
        {
            Assert.True(left == right,
                $"Assert failed. left: {left.ToString()}, right: {right.ToString()}");
        }
        private static void EqualsApprox(Vector4 left, Vector4 right, float tolerance)
        {
            Assert.True(System.Math.Abs(left.X - right.X) <= tolerance &&
                        System.Math.Abs(left.Y - right.Y) <= tolerance &&
                        System.Math.Abs(left.Z - right.Z) <= tolerance &&
                        System.Math.Abs(left.W - right.W) <= tolerance,
                $"Assert failed. left: {left.ToString()}, right: {right.ToString()}");
        }
    }
}