using System.Runtime.CompilerServices;

namespace AniNexus.Testing.PerformanceTests.Extensions.ByteExtensions;

public sealed class ToBoolTest
{
    /*
        |        Method | TestValue |      Mean |     Error |    StdDev |    Median |
        |-------------- |---------- |----------:|----------:|----------:|----------:|
     ** |   ToBoolLocal |         0 | 0.0010 ns | 0.0032 ns | 0.0028 ns | 0.0000 ns |
        | ToBoolNoLocal |         0 | 0.0193 ns | 0.0272 ns | 0.0255 ns | 0.0085 ns |
        |   ToBoolLocal |         1 | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns |
        | ToBoolNoLocal |         1 | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns |
        |   ToBoolLocal |       254 | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns |
        | ToBoolNoLocal |       254 | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns |
        |   ToBoolLocal |       255 | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns |
        | ToBoolNoLocal |       255 | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns |

        ** = Chosen implementation
        
        We opt for creating a local since it is faster. In addition it will help prevent
        register spills. The JIT will optimize it out if it needs to. What is interestingly
        consistent is that there is only a performance overhead if the value is 0.
    */

    [Params(0, 1, 254, 255)]
    public byte TestValue { get; set; }

    [Benchmark]
    public bool ToBoolLocal()
    {
        return TestValue.ToBoolLocal();
    }

    [Benchmark]
    public bool ToBoolNoLocal()
    {
        return TestValue.ToBoolNoLocal();
    }
}

public static class ToBoolTestExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ToBoolLocal(this byte b)
    {
        /*         
            IL_0000	ldarg.0	
            IL_0001	ldc.i4.0	
            IL_0002	cgt.un	
            IL_0004	ret
        */
        byte copy = b;

        return copy != 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ToBoolNoLocal(this byte b)
    {
        /*         
            IL_0000	ldarg.0	
            IL_0001	ldc.i4.0	
            IL_0002	cgt.un	
            IL_0004	ret
        */
        return b != 0;
    }
}
