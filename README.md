# AgileSwitch
A small and fast library to allow your code switch-case on runtime values, also it improves multiple if-else/switch-case statements, making them clearer and easier to maintain.

[![Version](https://img.shields.io/nuget/v/AgileSwitch.svg)](https://www.nuget.org/packages/AgileSwitch)

[![Build Status](https://travis-ci.org/FDUdannychen/AgileSwitch.svg)](https://travis-ci.org/FDUdannychen/AgileSwitch)

#Release Notes
- v1.1.2 portable .NET CORE
- v1.1.1 add async/await support
- v1.1.1 add T comparand support

Examples:
```
  Switch.On(10)
      .Case(100, n => Console.WriteLine("can't happen"))
          .Break()
      .Case<string>(s => Console.WriteLine("can't happen"))
      .Case(n => n > 5, n => Console.WriteLine("matches"))
      .Default(n => Console.WriteLine("default because n>5 doesn't break"));
      
  await Switch.On(10)
      .Case(100, n => Console.WriteLine("can't happen"))                
      .Case<string>(s => Console.WriteLine("can't happen"))
      .CaseAsync(100, n => PrintAsync("can't happen"))
      .CaseAsync(n => n < 5, async n => { await PrintAsync("can't happen"); })
          .Break()
      .Case(n => n > 5, n => Console.WriteLine("back to sync"))
          .Break()
      .DefaultAsync(async n => { await PrintAsync("can't happen because n>5 breaks"); });
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
