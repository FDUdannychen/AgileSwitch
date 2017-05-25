# Note
Since [pattern matching](https://github.com/dotnet/roslyn/blob/master/docs/features/patterns.md) is introduced in C#7, the project is supposed to be used in old versions of the language only.

# AgileSwitch
A small and fast library to allow your code switch-case on runtime values, also it improves multiple if-else/switch-case statements, making them clearer and easier to maintain.

[![Version](https://img.shields.io/nuget/v/AgileSwitch.svg)](https://www.nuget.org/packages/AgileSwitch)

[![Build Status](https://travis-ci.org/FDUdannychen/AgileSwitch.svg)](https://travis-ci.org/FDUdannychen/AgileSwitch)

#Release Notes
- v2.0.0 add return value support (break change)
- v1.1.2 portable .NET CORE
- v1.1.1 add async/await support
- v1.1.1 add T comparand support

Examples:
```csharp
Switch.On(number)
    .When(100)
        .Then(n => Print("it's 100"))
    .When(n => n > 10)
        .Then(n => Print("it's greater than 10"))
    .Default(n => Print("the default message will be shown because no .Break() calls"));

await Switch.On(number)
    .When(100)
        .Then(n => Print("it's 100"))
        .Break()
    .When(n => n > 10)
        .ThenAsync(async n => await PrintAsync("it's greater than 10"))
        .Break()
    .WhenAsync(async n => await Task.FromResult(n < 5))
        .Then(n => Print("it's less than 5"))
        .Break()
    .WhenAsync(async n => await Task.FromResult(n == 1))
        .ThenAsync(async n => await PrintAsync("it's 1"))
        .Break()
    .DefaultAsync(async n => await PrintAsync("the default message won't be shown if any previous case meets"));

string description = await Switch.On(number)
    .When(100)
        .Return("it's 100")
    .When(n => n > 10)
        .ReturnAsync(async n => await Task.FromResult("it's greater than 10"))
    .WhenAsync(async n => await Task.FromResult(n < 5))
        .Return(n => "it's less than 5")
    .WhenAsync(async n => await Task.FromResult(n == 1))
        .ReturnAsync(async n => await Task.FromResult("it's 1"))
    .Default("i have no idea");
```



The MIT License (MIT)

Copyright (c) 2016 Danny Chen

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
