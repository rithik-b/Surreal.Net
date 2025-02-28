using SurrealDB.Models.Result;

using DriverResponse = SurrealDB.Models.Result.DriverResponse;

namespace SurrealDB.Driver.Tests.Queries;

public abstract class MathQueryTests<T, TKey, TValue> : InequalityQueryTests<T, TKey, TValue>
    where T : IDatabase, IDisposable, new() {

    protected abstract string ValueCast();
    protected abstract void AssertEquivalency(TValue a, TValue b);

    [Theory]
    [MemberData("ValuePairs")]
    public async Task AdditionQueryTest(TValue val1, TValue val2) => await DbHandle<T>.WithDatabase(
        async db => {
            var expectedResult = (dynamic)val1! + (dynamic)val2!; // Can't do operator overloads on generic types, so force it by casting to a dynamic

            string sql = $"SELECT * FROM {ValueCast()}($val1 + $val2)";
            Dictionary<string, object?> param = new() { ["val1"] = val1, ["val2"] = val2, };

            DriverResponse response = await db.Query(sql, param);

            TestHelper.AssertOk(response);
            ResultValue result = response.FirstValue();
            var resultValue = result.AsObject<TValue>();
            AssertEquivalency(resultValue, expectedResult);
        }
    );

    [Theory]
    [MemberData("ValuePairs")]
    public async Task SubtractionQueryTest(TValue val1, TValue val2) => await DbHandle<T>.WithDatabase(
        async db => {
            var expectedResult = (dynamic)val1! - (dynamic)val2!; // Can't do operator overloads on generic types, so force it by casting to a dynamic

            string sql = $"SELECT * FROM {ValueCast()}($val1 - $val2)";
            Dictionary<string, object?> param = new() { ["val1"] = val1, ["val2"] = val2, };

            var response = await db.Query(sql, param);

            TestHelper.AssertOk(response);
            ResultValue result = response.FirstValue();
            var value = result.AsObject<TValue>();
            AssertEquivalency(value, expectedResult);
        }
    );

    [Theory]
    [MemberData("ValuePairs")]
    public async Task MultiplicationQueryTest(TValue val1, TValue val2) => await DbHandle<T>.WithDatabase(
        async db => {
            var expectedResult = (dynamic)val1! * (dynamic)val2!; // Can't do operator overloads on generic types, so force it by casting to a dynamic

            string sql = $"SELECT * FROM {ValueCast()}($val1 * $val2)";
            Dictionary<string, object?> param = new() { ["val1"] = val1, ["val2"] = val2, };

            var response = await db.Query(sql, param);

            TestHelper.AssertOk(response);
            ResultValue result = response.FirstValue();
            var value = result.AsObject<TValue>();
            AssertEquivalency(value, expectedResult);
        }
    );

    [Theory]
    [MemberData("ValuePairs")]
    public async Task DivisionQueryTest(TValue val1, TValue val2) => await DbHandle<T>.WithDatabase(
        async db => {
            var divisorIsZero = false;
            dynamic? expectedResult;
            if ((dynamic)val2! != 0) {
                expectedResult = (dynamic)val1! / (dynamic)val2!; // Can't do operator overloads on generic types, so force it by casting to a dynamic
            } else {
                divisorIsZero = true;
                expectedResult = default(TValue);
            }

            if (divisorIsZero) {
                // TODO: Remove this when divide by zero works
                // Pass the test right now as Surreal crashes when it tries to divide by 0
                return;
            }

            string sql = $"SELECT * FROM {ValueCast()}($val1 / $val2)";
            Dictionary<string, object?> param = new() { ["val1"] = val1, ["val2"] = val2, };

            var response = await db.Query(sql, param);

            TestHelper.AssertOk(response);
            ResultValue result = response.FirstValue();

            if (!divisorIsZero) {
                var value = result.AsObject<TValue>();
                AssertEquivalency(value, expectedResult);
            } else {
                Assert.True(false); // TODO: Test for the expected result when doing a divide by zero
            }
        }
    );

    protected MathQueryTests(ITestOutputHelper logger) : base(logger) {
    }
}
