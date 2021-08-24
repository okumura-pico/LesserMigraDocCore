# MigraDoc Foundation for .NET Standard 2.0

This is a port of [MigraDoc Foundation](https://github.com/empira/MigraDoc) for .NET Standard 2.0.

I ported just only used functions.

# Getting Started

This refers PdfSharpCore. Update a submodule.

```console
git submodule init
git submodule update
```

Build all.

```cosnole
dotnet build -c Release
```

# How to execute samples

Move to a sample directory and execute dotnet command.

```console
cd Samples/JapaneseFont
dotnet run output.pdf
```
