![odinLZ4](https://user-images.githubusercontent.com/29813954/111915103-582fba00-8a7d-11eb-865d-1bd6b880bece.png)
# OdinLZ4Extension

The fastest and most efficient binary compression solution for [Odin](https://odininspector.com)

## Navigation

- [OdinLZ4Extension](#odinlz4extension)
  - [Navigation](#navigation)
  - [About](#about)
    - [What's under the hood](#whats-under-the-hood)
  - [Roadmap](#roadmap)
  - [Compatibility](#compatibility)
  - [Dependencies](#dependencies)
  - [How to use](#how-to-use)
  - [How to install System libraries for LZ4](#how-to-install-system-libraries-for-lz4)
  - [How to help the project](#how-to-help-the-project)
  - [Contanct](#contanct)

## About

[Odin](https://odininspector.com) is one of the most popular development tools for Unity,
its power and functionality is incredible, one of the main functions is de/serialization of data,
it is extremely important function for most who use "Odin".

If you use cloud solutions in your projects and store some data in a serialized form,
then imagine how much data space you can save with fast and efficient compression.

The best available algorithm was chosen as the compression engine, this is [LZ4](https://en.wikipedia.org/wiki/LZ4_(compression_algorithm)), it is extremely fast and efficient.

Combining these two engineering wonders we get extremely fast de/serialization and more free space for data.

So, Odin + LZ4 = PROFIT :muscle:

### What's under the hood

The best [realization](https://github.com/MiloszKrajewski/K4os.Compression.LZ4) of the LZ4 algorithm. This solution is provided by Milos Krajewski under the MIT license.

Original LZ4 has been written by Yann Collet and original 'C' sources can be found [here](https://github.com/lz4/lz4)

More info [here](http://lz4.github.io/lz4)

>If you find this project useful, star it, I will be grateful.

## Roadmap

|  Status   | Milestone                                                                     |
| :-------: | :---------------------------------------------------------------------------- |
| :rocket:  | Implement features that are available in the `SerializationUtility` using LZ4 |
| :pushpin: | Cut out the main parts of the LZ4 engine and compile a lightweight `.dll`     |
| :pushpin: | Multithread de/compressing and de/serializing                                 |
| :pushpin: | De/Serialization performance tests with and without compression               |

## Compatibility

| Backend | .Net  | Odin  |     Compatible     |
| :-----: | :---: | :---: | :----------------: |
| IL2CPP  |  4.x  |  3.x  | :white_check_mark: |

## Dependencies

- [Odin Serializer](https://odininspector.com/odin-serializer)
- System libraries
  - [System.Memory](https://www.nuget.org/packages/System.Memory/)
  - [System.Runtime.CompilerServices.Unsafe](https://www.nuget.org/packages/System.Runtime.CompilerServices.Unsafe/)

> The `OdinLZ4Extension` does not contain any source materials of the `Sirenix`, you can get `Odin Serializer` [on the official website](https://odininspector.com/download)

## How to use

1. Include LZ4 necessary libraries ([how to do it](#how-to-install-system-libraries-for-lz4))

2. Add the OdinLZ4Extension to your project, you can do it in the following way:
   - import [latest](https://github.com/inc8877/OdinLZ4Extension/releases) extension version via [UPM](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@2.0/manual/index.html)
   - add extension as a [submodule](https://git-scm.com/book/en/v2/Git-Tools-Submodules)
   - import source code

3. Plugin namespaces

```c#
using K4os.Compression.LZ4;
using OdinLZ4Extension;
```

<br> Code example:

```c#
[Serializable] public class SomeClass { public int a; public string someData; }
[SerializeField] private SomeClass compressMe = new SomeClass() { a = 100, someData = "It's work!" };

//Serialize
byte[] data = OdinLZ4API.SerializeValue(compressMe, DataFormat.Binary, compressionLevel:LZ4Level.L00_FAST);

//Deserialize
SomeClass deserializedData = OdinLZ4API.DeserializeValue<SomeClass>(data, DataFormat.Binary);
```

## How to install System libraries for LZ4

Unity does not provide the necessary libraries for the LZ4, so you need to install them manually.
The libraries can be downloaded via NuGet, but since the Unity does not work well with it, you need to get the `.dll` from the downloaded packages and put them into the `Assets` folder.

If you don't want to bother with it then you can download the necessary `.dll` that are available in each [Release](https://github.com/inc8877/OdinLZ4Extension/releases)

A good location for `.dll` in the project would be the `Assets/Plugins` folder.

## How to help the project

At the moment the project has just begun its life and requires a lot of work,
if you are reading this means you may have liked this project and you can help by doing the following:

- do performance tests (compare data serialization with and without compression and make a graph)
- expand functionality:
  - serialization with compressing via multithreading
  - a pool of serialization tasks with callbacks

## Contanct

email: <inc8877@gmail.com>
