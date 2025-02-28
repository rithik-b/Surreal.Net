namespace SurrealDB.Driver.Tests.Queries.Typed;

public class RpcDoubleQueryTests : DoubleQueryTests<DatabaseRpc> {
    public RpcDoubleQueryTests(ITestOutputHelper logger) : base(logger) {
    }
}
public class RestDoubleQueryTests : DoubleQueryTests<DatabaseRest> {
    public RestDoubleQueryTests(ITestOutputHelper logger) : base(logger) {
    }
}

public abstract class DoubleQueryTests <T> : MathQueryTests<T, double, double>
    where T : IDatabase, IDisposable, new() {

    private static IEnumerable<double> TestValues {
        get {
            yield return 1000; // Can't go too high otherwise the maths operations might overflow
            yield return 0;
            yield return -1000;
        }
    }

    public static IEnumerable<object[]> KeyAndValuePairs {
        get {
            return TestValues.Select(e => new object[] { e, e });
        }
    }
    
    public static IEnumerable<object[]> ValuePairs {
        get {
            foreach (var testValue1 in TestValues) {
                foreach (var testValue2 in TestValues) {
                    yield return new object[] { testValue1, testValue2 };
                }
            }
        }
    }

    protected override string ValueCast() {
        return "<float>";
    }

    protected override void AssertEquivalency(double a, double b) {
        b.Should().BeApproximately(a, 0.1d);
    }

    protected DoubleQueryTests(ITestOutputHelper logger) : base(logger) {
    }
}
